using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.Boss {
    
    public class BossEntityHealth : MonoBehaviour {

        public int HitsHealth;
        public int RageHpActivation;
        public Animator Animator;
        public GameObject DeathParticlePrefab;
        
        private EntityProcessDamage m_entityProcessDamage;
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnHitBoss>(OnHitBoss);
            m_entityProcessDamage = GetComponent<EntityProcessDamage>();
        }

        protected virtual void OnHitBoss(OnHitBoss ev) {
            if (ev.Entity != m_entityProcessDamage || ev.WillOverride) return;

            HitsHealth -= ev.Damage;
            Animator.CrossFade("hit", 0.1f);
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.MEDIUM));
            
            if (HitsHealth <= RageHpActivation) {
                GameManager.Instance.Dispatcher.Emit(new OnBossRageMode());
            }

            if (HitsHealth <= 0) {
                Animator.CrossFade("death", 0.1f);
                GameManager.Instance.Dispatcher.Emit(new OnBossStopAttack());
                GameManager.Instance.InputManager.PlayerActions.Disable();
                Invoke(nameof(SpawnParticleEndStage),2.5f);
            }
        }
        
        private void SpawnParticleEndStage() {
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShakeWithValues(0.6f, 2f));
            Instantiate(DeathParticlePrefab, transform.position, Quaternion.identity);
            GameManager.Instance.OnBossDeath();
            Destroy(gameObject);
        }
    }
}