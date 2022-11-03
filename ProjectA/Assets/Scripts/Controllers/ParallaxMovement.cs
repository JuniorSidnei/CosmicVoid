using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Media;
using UnityEngine;

namespace ProjectA.Movement {
    
    public class ParallaxMovement : MonoBehaviour {

        private float m_limit = -23f;

        private void Update() {
            if (!(transform.position.x <= m_limit)) return;
            
            var transformPosition = transform.position;
            transformPosition.x = 28.56f;
            transform.position = transformPosition;
        }
    }
}