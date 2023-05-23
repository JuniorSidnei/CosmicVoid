using System;
using System.Collections.Generic;
using ProjectA.Actions;
using ProjectA.Data.Wave;
using ProjectA.Interface;
using ProjectA.Scriptables;
using ProjectA.Scriptables.Boss;
using UnityEngine;

namespace ProjectA.Entity.Position {
    
    public class EntityPosition : MonoBehaviour {

        public WaveData.EntityType Type;
        
        public void SetType(WaveData.EntityInfo cloakingEntity) {
            Type = cloakingEntity.Type;
        }
        
        public void SetPositionAndType(WaveData.EntityInfo info, Transform parent) {
            Type = info.Type;
            
            var transformLocalPosition = GetPosition(info.Position);

            transform.parent = parent;
            
            transformLocalPosition.x = transform.parent.localPosition.x;
            transform.localPosition = transformLocalPosition;
        }
        
        public void SetPositionAndTypeWithX(WaveData.EntityInfo info, Transform parent, float positionX) {
            Type = info.Type;
            
            var transformLocalPosition = GetPosition(info.Position);

            transform.parent = parent;
            
            transformLocalPosition.x = positionX;
            transform.localPosition = transformLocalPosition;
        }

        private Vector3 GetPosition(WaveData.EntityPosition position) {
            var transformLocalPosition = transform.localPosition;

            transformLocalPosition.y = position switch {
                WaveData.EntityPosition.Up => 2.3f,
                WaveData.EntityPosition.Middle => .2f,
                WaveData.EntityPosition.Down => -2f,
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };

            return transformLocalPosition;
        }
        
        public void SetPosition(BossAttackWave.EntityInfo info, Transform parent) {
            Type = info.Type switch {
                BossAttackWave.ProjectileType.Reflective => WaveData.EntityType.Reflective,
                BossAttackWave.ProjectileType.Hard => WaveData.EntityType.HardProjectile,
                BossAttackWave.ProjectileType.Explosive => WaveData.EntityType.Explosive,
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
            gameObject.AddComponent<ShooterEntity>().Setup(entityInfo, playerLayer);
            var shoot = gameObject.AddComponent<Shoot>();
            shoot.SpawnPrefab = entityInfo.SpawnPrefab;
            shoot.ShootInterval = entityInfo.ShootInterval;
            shoot.Spawn = new Vector3(-0.809f, 0.5f, 0f);
        }
        
        public void LinkerSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<LinkerEntity>().Setup(entityInfo, playerLayer);
            GetComponent<LinkerEntity>().SetupLinker();
        }
        
        public void LinkedSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<LinkerEntity>().Setup(entityInfo, playerLayer);
        }

        public void CloakingSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<CloakingEntity>().Setup(entityInfo, playerLayer);
        }
        
        public void ReflectiveProjectileSetup(EntityInfo entityInfo, List<LayerMask> playerLayer) {
            gameObject.AddComponent<ReflectiveEntity>().Setup(entityInfo, playerLayer);
        }
        
        public void HardProjectileSetup(EntityInfo entityInfo, LayerMask playerLayer) {
            gameObject.AddComponent<HardEntity>().Setup(entityInfo, playerLayer);
        }

        public void ExplosiveProjectileSetup(EntityInfo entityInfo, List<LayerMask> playerLayer) {
            gameObject.AddComponent<ExplosiveReflectiveProjectile>().Setup(entityInfo, playerLayer);
        }
    }
}