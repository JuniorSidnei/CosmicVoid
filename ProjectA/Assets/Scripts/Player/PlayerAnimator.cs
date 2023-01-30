using System;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Animator {

    public class PlayerAnimator : MonoBehaviour {

        private UnityEngine.Animator m_animator;

        private static readonly int m_idle = UnityEngine.Animator.StringToHash("idle");
        private static readonly int m_moveUp = UnityEngine.Animator.StringToHash("move_up");
        private static readonly int m_moveDown = UnityEngine.Animator.StringToHash("move_down");
        private static readonly int m_attack = UnityEngine.Animator.StringToHash("attack");
        private static readonly int m_chargedAttack = UnityEngine.Animator.StringToHash("charged_attack");
        private static readonly int m_charged = UnityEngine.Animator.StringToHash("charged");
        private static readonly int m_moveUpCharged = UnityEngine.Animator.StringToHash("move_up_charged");
        private static readonly int m_moveDownCharged = UnityEngine.Animator.StringToHash("move_down_charged");
        private static readonly int m_stunned = UnityEngine.Animator.StringToHash("stunned");

        private PlayerMovement.PlayerStates m_playerCurrentState;
        
        public void Charged() {
            m_animator.CrossFade(m_charged, 0f, 0);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.CHARGED));
        }
        
        public void ResetToIdle() {
            if (m_playerCurrentState == PlayerMovement.PlayerStates.STUNNED) return;
            
            m_animator.CrossFade(m_idle, 0f, 0);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.IDLE));
        }
        
        private void Awake() {
            m_animator = GetComponent<UnityEngine.Animator>();
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerStateChange>(OnPlayerStateChange);
        }

        private void OnPlayerStateChange(OnPlayerStateChange ev) {
            m_playerCurrentState = ev.NewState;
        }

        private void Update() {
            m_animator.CrossFade(GetHashByState(m_playerCurrentState), 0f, 0);
        }

        private int GetHashByState(PlayerMovement.PlayerStates state) {
            
            switch (state) {
                case PlayerMovement.PlayerStates.MOVEUP:
                    return m_moveUp;
                case PlayerMovement.PlayerStates.MOVEDOWN:
                    return m_moveDown;
                case PlayerMovement.PlayerStates.ATTACK:
                    return m_attack;
                case PlayerMovement.PlayerStates.CHARGEDATTACK:
                    return m_chargedAttack;
                case PlayerMovement.PlayerStates.IDLE:
                    return m_idle;
                case PlayerMovement.PlayerStates.CHARGED:
                    return m_charged;
                case PlayerMovement.PlayerStates.UP_CHARGED:
                    return m_moveUpCharged;
                case PlayerMovement.PlayerStates.DOWN_CHARGED:
                    return m_moveDownCharged;
                case PlayerMovement.PlayerStates.STUNNED:
                    return m_stunned;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}