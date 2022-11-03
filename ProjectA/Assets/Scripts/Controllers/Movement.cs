using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectA.Actions {
    
    public class Movement : MonoBehaviour {

        public float Speed;
        public float Drag;
        
        private Vector2 m_velocity;
        private Vector3 m_velocitySmoothing;

        private void FixedUpdate() {

            m_velocity *= (1 - Time.deltaTime * Drag);
            
            var targetVelocity = new Vector2(-1, 0) * Speed;
            m_velocity = Vector3.SmoothDamp(m_velocity, targetVelocity, ref m_velocitySmoothing, .5f);
            
            transform.Translate(m_velocity * Time.deltaTime); 
        }
    }
}