using System;
using ProjectA.Data.Wave;
using ProjectA.Interface;
using ProjectA.Scriptables;
using ProjectA.Scriptables.Boss;
using UnityEditor.Animations;
using UnityEngine;

namespace ProjectA.Entity.Position {
    
    public class EntityPosition : MonoBehaviour {

        public WaveData.EntityType Type;

        public void SetPosition(WaveData.EntityInfo info, Transform parent) {
            Type = info.Type;
            var transformLocalPosition = GetPosition(info.Position);

            transform.parent = parent;
            
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

        public void SetPosition(BossAttackWave.EntityInfo info, Transform parent) {
            var transformLocalPosition = GetPosition(info.Position);

            transform.parent = parent;
            
            transformLocalPosition.x = transform.parent.localPosition.x;
            transform.localPosition = transformLocalPosition;
        }

        public void DestructibleSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<DestructibleEntity>().Setup(entityInfo, playerLayer);
        }

        public void HardPropSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<HardEntity>().Setup(entityInfo, playerLayer);
            
        }

        public void EnemySetup(EntityInfo entityInfo, LayerMask playerLayer) {
            
        }

        public void ShooterSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            
        }
    }
}