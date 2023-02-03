using System;
using ProjectA.Data.Wave;
using UnityEngine;

namespace ProjectA.Entity.Position {
    
    public class EntityPosition : MonoBehaviour {

        public void SetPosition(WaveData.EntityPosition position) {
            var transformLocalPosition = GetPosition(position);

            transformLocalPosition.x = transform.parent.localPosition.x;
            transform.localPosition = transformLocalPosition;
        }
        
        public void SetBossPosition(WaveData.EntityInfo entityInfo) {
            
        }

        private Vector3 GetPosition(WaveData.EntityPosition position) {
            var transformLocalPosition = transform.localPosition;

            transformLocalPosition.y = position switch {
                WaveData.EntityPosition.Up => 2.5f,
                WaveData.EntityPosition.Middle => .5f,
                WaveData.EntityPosition.Down => -1.5f,
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };

            return transformLocalPosition;
        }
    }
}