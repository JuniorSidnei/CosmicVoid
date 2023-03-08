using System;
using System.Collections.Generic;
using ProjectA.Actions;
using ProjectA.Data.Wave;
using ProjectA.Interface;
using ProjectA.Scriptables;
using ProjectA.Scriptables.Boss;
using UnityEditor.Animations;
using UnityEngine;

namespace ProjectA.Entity.Position {
    
    public class EntityPosition : MonoBehaviour {

        public WaveData.EntityType Type;
        
        private WaveData.EntityPosition m_position;
        
        public void SetPosition(WaveData.EntityInfo info, Transform parent) {
            Type = info.Type;
            m_position = info.Position;
            
            var transformLocalPosition = GetPosition(info.Position);

            transform.parent = parent;
            
            transformLocalPosition.x = transform.parent.localPosition.x;
            transform.localPosition = transformLocalPosition;
        }
        
        public void SetBossPosition(WaveData.EntityInfo entityInfo) { }

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

        public Vector3 GetPositionWithData() {
            var transformLocalPosition = transform.localPosition;

            transformLocalPosition.y = m_position switch {
                WaveData.EntityPosition.Up => 2.5f,
                WaveData.EntityPosition.Middle => .5f,
                WaveData.EntityPosition.Down => -1.5f,
                _ => throw new ArgumentOutOfRangeException(nameof(m_position), m_position, null)
            };

            return transformLocalPosition;  
        }
        
        public void SetPosition(BossAttackWave.EntityInfo info, Transform parent) {
            Type = info.Type switch {
                BossAttackWave.ProjectileType.Reflective => WaveData.EntityType.Reflective,
                BossAttackWave.ProjectileType.Hard => WaveData.EntityType.HardProjectile,
                _ => throw new ArgumentOutOfRangeException()
            };

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
            gameObject.AddComponent<EnemyEntity>().Setup(entityInfo, playerLayer);
        }
        
        public void ShooterSetup(EntityShooterInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<EnemyEntity>().Setup(entityInfo, playerLayer);
            var shoot = gameObject.AddComponent<Shoot>();
            shoot.SpawnPrefab = entityInfo.SpawnPrefab;
            shoot.ShootInterval = entityInfo.ShootInterval;
            shoot.Spawn = new Vector3(-0.809f, 0f, 0f);
        }
        
        public void ReflectiveProjectileSetup(EntityInfo entityInfo, List<LayerMask> playerLayer) {
            gameObject.AddComponent<ReflectiveEntity>().Setup(entityInfo, playerLayer);
        }
        
        public void HardProjectileSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<HardEntity>().Setup(entityInfo, playerLayer);
        }
    }
}