using GameToBeNamed.Utils.Sound;
using ProjectA.Animators;
using ProjectA.Input;
using ProjectA.Interface;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Attack {
    
    public class PlayerAttack : MonoBehaviour {
        
        public InputManager InputManager;
        public PlayerAnimator PlayerAnimator;
        public float TimeToChargedAttack;
        public float AttackCooldown;
        public LayerMask EntityLayer;
        public GameObject HitEffectPrefab;
        
        private float m_elapsedtimeCharged;
        private bool m_isChargingAttack;
        private float m_chargedAttackTreshold = .5f;
        private CircleCollider2D m_circleCollider2D;
        private PlayerMovement.PlayerStates m_currentPlayerState;
        private float m_elapsedAttackCooldown;
        private bool m_isCharged;
        
        public bool IsCharged() {
            return m_isCharged;
        }
        
        private void Start() {
            InputManager.Attack.performed += ctx => StartAttack();
            InputManager.Attack.canceled += ctx => AnimateAttack();
            m_circleCollider2D = GetComponent<CircleCollider2D>();
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerStateChange>(OnPlayerStateChange);
            GameManager.Instance.Dispatcher.Subscribe<OnSpawnBoss>(OnSpawnBoss);

            m_elapsedAttackCooldown = AttackCooldown;
        }

        private void OnSpawnBoss(OnSpawnBoss arg0) {
            m_elapsedtimeCharged = 0f;
            m_isChargingAttack = false;
            m_isCharged = false;
            m_currentPlayerState = PlayerMovement.PlayerStates.IDLE;
        }

        private void OnPlayerStateChange(OnPlayerStateChange ev) {
            m_currentPlayerState = ev.NewState;

            if (m_currentPlayerState != PlayerMovement.PlayerStates.IDLE && m_currentPlayerState != PlayerMovement.PlayerStates.STUNNED) return;
            
            m_elapsedtimeCharged = 0;
            m_isChargingAttack = false;
            m_isCharged = false;
        }

        private void StartAttack() {
            if (m_currentPlayerState == PlayerMovement.PlayerStates.STUNNED) return;
            m_isChargingAttack = true;
        }

        private void AnimateAttack() {
            if (m_currentPlayerState == PlayerMovement.PlayerStates.STUNNED) return;
            
            if(m_elapsedAttackCooldown > 0) return;
            
            if (m_elapsedtimeCharged <= m_chargedAttackTreshold) {
                GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.ATTACK));
                Attack();
            } else if (m_elapsedtimeCharged > m_chargedAttackTreshold && m_isCharged) {
                GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.CHARGEDATTACK));
                Attack();
            }

            m_elapsedtimeCharged = 0;
            m_isChargingAttack = false;
        }
        
        private void Update() {
            
            if(m_isCharged) return;
            
            m_elapsedAttackCooldown -= Time.deltaTime;
            
            if(m_elapsedAttackCooldown > 0) return;

            if (m_elapsedAttackCooldown <= 0) m_elapsedAttackCooldown = 0;

            if (!m_isChargingAttack) {
                m_elapsedtimeCharged -= Time.deltaTime;

                if (m_elapsedtimeCharged <= 0) m_elapsedtimeCharged = 0;
                return;
            }

            m_elapsedtimeCharged += Time.deltaTime;

            if (m_elapsedtimeCharged >= TimeToChargedAttack) {
                PlayerAnimator.Charged();
                m_isCharged = true;
                AudioController.Instance.Play(GameManager.Instance.GameSettings.PlayerCharged, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale(), true);
            }
        }

        private void Attack() {
            AudioController.Instance.Play(m_isCharged ? GameManager.Instance.GameSettings.PlayerAttackCharged : GameManager.Instance.GameSettings.PlayerAttack, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale());
            m_elapsedAttackCooldown = AttackCooldown;
            var rayPosition = new Vector3(transform.position.x + m_circleCollider2D.radius + 0.25f, transform.position.y);
            
            var hit = Physics2D.Raycast(rayPosition, Vector2.right, 1f, EntityLayer);

            AudioController.Instance.StopSound(GameManager.Instance.GameSettings.PlayerCharged, true);
            if (!hit) return;

            var o = hit.collider.gameObject;
            if(o == null) return;
            
            var transformPosition = transform.position;
            var newPosition = new Vector3(transformPosition.x + 1f, transformPosition.y);
            Instantiate(HitEffectPrefab, newPosition, Quaternion.identity, transform);
            o.GetComponent<IDamageable>()?.ProcessDamage(IsCharged());
            m_isCharged = false;

            AudioController.Instance.StopSound(GameManager.Instance.GameSettings.PlayerCharged, true);
        }

        private void OnDrawGizmos() {
            
            if (m_circleCollider2D == null)
                m_circleCollider2D = GetComponent<CircleCollider2D>();
            
            Gizmos.color = Color.red;
            var rayPosition = new Vector3(transform.position.x + m_circleCollider2D.radius + 0.25f, transform.position.y);
            Gizmos.DrawRay(rayPosition, Vector3.right);
        }
    }
}