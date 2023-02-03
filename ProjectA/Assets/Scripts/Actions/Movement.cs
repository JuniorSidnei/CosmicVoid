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

        private void Awake() {
            m_entityProcessDamage = GetComponent<EntityProcessDamage>();
            
            GameManager.Instance.Dispatcher.Subscribe<OnReflectEntity>(OnReflectEntity);
            
            m_targetVelocity = new Vector2(-1, 0) * Speed;
        }

        private void OnReflectEntity(OnReflectEntity ev) {
            if(ev.Entity != m_entityProcessDamage) return;

            ApplyReflect(ev.IsCharged);
        }

        private void ApplyReflect(bool isCharged) {
            m_entityProcessDamage.DamagePower *= 2;
            m_entityProcessDamage.IsReflected = true;
            Acceleration = 0f;
            var speed = isCharged ? Speed * 4 : Speed * 2;
            m_targetVelocity = new Vector2(1, 0) * speed;
        }

        private void FixedUpdate() {

            m_velocity *= (1 - Time.deltaTime * Drag);
            
            m_velocity = Vector3.SmoothDamp(m_velocity, m_targetVelocity, ref m_velocitySmoothing, Acceleration);
            
            transform.Translate(m_velocity * Time.deltaTime);
        }
    }
}