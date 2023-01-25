using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Animator;
using ProjectA.Data.Wave;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Input;
using ProjectA.Interface;
using ProjectA.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectA.Attack {
    
    public class PlayerAttack : MonoBehaviour {
        
        public InputManager InputManager;
        public PlayerMovement PlayerMovement;
        public PlayerAnimator PlayerAnimator;
        public float TimeToChargedAttack;
        public LayerMask EntityLayer;

        private float m_elapsedtimeCharged;
        private bool m_isChargingAttack;
        private float m_chargedAttackTreshold = .5f;
        private CircleCollider2D m_circleCollider2D;
        
        public bool IsCharged() {
            return m_elapsedtimeCharged >= TimeToChargedAttack;
        }

        private bool IsIdle() {
            return PlayerMovement.State == PlayerMovement.PlayerStates.IDLE;
        }
        
        private void Start() {
            InputManager.Attack.performed += ctx => StartAttack();
            InputManager.Attack.canceled += ctx => AnimateAttack();
            m_circleCollider2D = GetComponent<CircleCollider2D>();
        }

        private void StartAttack() {
            m_isChargingAttack = true;
        }

        private void AnimateAttack() {
            if (m_elapsedtimeCharged <= m_chargedAttackTreshold) {
                PlayerMovement.State = PlayerMovement.PlayerStates.ATTACK;
                Attack();
            } else if (m_elapsedtimeCharged > m_chargedAttackTreshold && m_elapsedtimeCharged > TimeToChargedAttack) {
                PlayerMovement.State = PlayerMovement.PlayerStates.CHARGEDATTACK;
                Attack();
            }

            m_elapsedtimeCharged = 0;
            m_isChargingAttack = false;
        }
        
        private void Update() {
            if (!m_isChargingAttack) return;

            m_elapsedtimeCharged += Time.deltaTime;

            if (m_elapsedtimeCharged >= TimeToChargedAttack && !PlayerMovement.IsMoving) {
                PlayerAnimator.Charged();
            }
        }

        private void Attack() {

            var rayPosition = new Vector3(transform.position.x + m_circleCollider2D.radius + 0.15f, transform.position.y);
            
            var hit = Physics2D.Raycast(rayPosition, Vector2.right, 1f, EntityLayer);

            if (!hit) return;

            var o = hit.collider.gameObject;
            
            o.GetComponent<IDamageable>().ProcessDamage(IsCharged(),gameObject.GetComponent<PlayerHealth>());
        }

        private void OnDrawGizmos() {
            
            if (m_circleCollider2D == null)
                m_circleCollider2D = GetComponent<CircleCollider2D>();
            
            Gizmos.color = Color.red;
            var rayPosition = new Vector3(transform.position.x + m_circleCollider2D.radius + 0.15f, transform.position.y);
            Gizmos.DrawRay(rayPosition, Vector3.right);
        }
    }
}