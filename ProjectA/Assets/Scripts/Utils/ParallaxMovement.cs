using System;
using UnityEngine;

namespace ProjectA.Movement {
    
    public class ParallaxMovement : MonoBehaviour {

        public float Speed;
        private float m_limit = -1392.099f;
        private Vector2 m_velocity;
        private Vector3  m_velocitySmoothing;
        private Vector3 m_targetVelocity;

        private void Awake() {
            m_targetVelocity = new Vector3(-1, 0, 0) * Speed;
        }

        private void FixedUpdate() {
            m_velocity *= (1 - Time.deltaTime * 1);
            
            m_velocity = Vector3.SmoothDamp(m_velocity, m_targetVelocity, ref m_velocitySmoothing, 0);
            
            transform.Translate(m_velocity * Time.deltaTime);
        }

        private void Update() {
            
            if (!(transform.localPosition.x <= m_limit)) return;
            
            var transformPosition = transform.localPosition;
            transformPosition.x = -1362.24f;
            transform.localPosition = transformPosition;
        }
    }
}