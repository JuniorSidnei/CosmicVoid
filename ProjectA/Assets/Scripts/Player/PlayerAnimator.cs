using System;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectA.Animators {

    public class PlayerAnimator : MonoBehaviour {
        
        private Animator m_animator;
        
        private static readonly int m_idle = Animator.StringToHash("idle");
        private static readonly int m_moveUp = Animator.StringToHash("move_up");
        private static readonly int m_moveDown = Animator.StringToHash("move_down");
        private static readonly int m_attack = Animator.StringToHash("attack");
        private static readonly int m_chargedAttack = Animator.StringToHash("charged_attack");
        private static readonly int m_charged = Animator.StringToHash("charged");
        private static readonly int m_moveUpCharged = Animator.StringToHash("move_up_charged");
        private static readonly int m_moveDownCharged = Animator.StringToHash("move_down_charged");
        private static readonly int m_stunned = Animator.StringToHash("stunned");
        private static readonly int m_running = Animator.StringToHash("running");
        private static readonly int m_hit = Animator.StringToHash("hit");

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
            m_animator = GetComponent<Animator>();
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
                case PlayerMovement.PlayerStates.RUNNING:
                    return m_running;
                case PlayerMovement.PlayerStates.HIT:
                    return m_hit;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}