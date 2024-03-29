using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Actions {
    
    public class Movement : MonoBehaviour {

        public float Speed;
        public float Drag;
        public float Acceleration;

        private Vector2 m_velocity;
        private Vector3 m_velocitySmoothing;
        private Vector2 m_targetVelocity;
        private EntityProcessDamage m_entityProcessDamage;

        public void SetSpeed(float newSpeed) {
            Speed = newSpeed;
            m_targetVelocity = Vector2.zero;
            m_targetVelocity = new Vector2(-1, 0) * Speed;
        }
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnReflectEntity>(OnReflectEntity);
            GameManager.Instance.Dispatcher.Subscribe<OnExplosionActivated>(OnExplosionActivated);
        }

        private void OnExplosionActivated(OnExplosionActivated arg0) {
            m_targetVelocity = new Vector2(-1, 0) * Speed;
        }

        private void Start() {
            m_entityProcessDamage = GetComponent<EntityProcessDamage>();
        }

        private void OnReflectEntity(OnReflectEntity ev) {
            if(ev.Entity != m_entityProcessDamage) return;

            ApplyReflect(ev.IsCharged);
        }

        private void ApplyReflect(bool isCharged) {
            if (m_entityProcessDamage.IsReflected) {
                Acceleration = 0f;
                var speedReflected = Speed * 6;
                m_targetVelocity = new Vector2(-1, 0) * speedReflected;
                m_entityProcessDamage.IsReflected = false;
                transform.localScale = new Vector3(1, 1, 1);
                return;
            }
            
            m_entityProcessDamage.DamagePower *= 2;
            m_entityProcessDamage.IsReflected = true;
            Acceleration = 0f;
            var speed = isCharged ? Speed * 4 : Speed * 2;
            m_targetVelocity = new Vector2(1, 0) * speed;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        private void FixedUpdate() {

            m_velocity *= (1 - Time.deltaTime * Drag);
            
            m_velocity = Vector3.SmoothDamp(m_velocity, m_targetVelocity, ref m_velocitySmoothing, Acceleration);
            
            transform.Translate(m_velocity * Time.deltaTime);
        }

        public void ResetVelocity() {
            m_targetVelocity = Vector2.zero;
            m_targetVelocity = new Vector2(-1, 0) * Speed;
            m_entityProcessDamage = GetComponent<EntityProcessDamage>();
        }
    }
}