using System;
using ProjectA.Movement;
using UnityEngine;

namespace ProjectA.Animator {

    public class PlayerAnimator : MonoBehaviour {

        public PlayerMovement PlayerMovement;

        private UnityEngine.Animator m_animator;

        private static readonly int m_idle = UnityEngine.Animator.StringToHash("idle");
        private static readonly int m_moveUp = UnityEngine.Animator.StringToHash("move_up");
        private static readonly int m_moveDown = UnityEngine.Animator.StringToHash("move_down");
        private static readonly int m_attack = UnityEngine.Animator.StringToHash("attack");
        private static readonly int m_chargedAttack = UnityEngine.Animator.StringToHash("charged_attack");

        public void ResetToIdle() {
            m_animator.CrossFade(m_idle, 0f, 0);
            PlayerMovement.State = PlayerMovement.PlayerStates.IDLE;
        }
        
        private void Awake() {
            m_animator = GetComponent<UnityEngine.Animator>();
        }

        private void Update() {
            m_animator.CrossFade(GetHashByState(PlayerMovement.State), 0f, 0);
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}