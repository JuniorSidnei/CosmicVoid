using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Animator;
using ProjectA.Data.Wave;
using ProjectA.Entity.ProcessDamage;
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

            m_elapsedAttackCooldown = AttackCooldown;
        }
        
        private void OnPlayerStateChange(OnPlayerStateChange ev) {
            m_currentPlayerState = ev.NewState;

            if (m_currentPlayerState != PlayerMovement.PlayerStates.IDLE && m_currentPlayerState != PlayerMovement.PlayerStates.STUNNED) return;
            
            m_elapsedtimeCharged = 0;
            m_isChargingAttack = false;
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
            } else if (m_elapsedtimeCharged > m_chargedAttackTreshold && m_elapsedtimeCharged > TimeToChargedAttack) {
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
            
            if (!m_isChargingAttack) return;

            m_elapsedtimeCharged += Time.deltaTime;

            if (m_elapsedtimeCharged >= TimeToChargedAttack) {
                PlayerAnimator.Charged();
                m_isCharged = true;
            }
        }

        private void Attack() {

            m_elapsedAttackCooldown = AttackCooldown;
            m_isCharged = false;
            var rayPosition = new Vector3(transform.position.x + m_circleCollider2D.radius + 0.25f, transform.position.y);
            
            var hit = Physics2D.Raycast(rayPosition, Vector2.right, 1f, EntityLayer);

            if (!hit) return;

            var o = hit.collider.gameObject;
            if(o == null) return;
            
            o.GetComponent<IDamageable>().ProcessDamage(IsCharged());
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