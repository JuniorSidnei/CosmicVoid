using System.Collections;
using System.Net;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Pools;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Managers {
    
    public class SpawnManager : Singleton<SpawnManager> {

        public WaveData WaveData;

        // public DestructiblePropPool DestructiblePool;
        // public HardPropPool HardPropPool;
        // public EnemyPool EnemyPool;
        // public EnemyShooterPool EnemyShooterPool;
        // public ReflectivePool ReflectivePool;

        public EntitiesPool EntitiesPool;
        
        private float m_timeToNextSpawn;
        private int m_currentEntityIndex = 0;
        private bool m_waveFinishedSpawn;
        private bool m_isBossSpawned;

        private void Start() {
            m_timeToNextSpawn = WaveData.InitialTimeSpawn;
        }

        private void Update() {

            if (m_waveFinishedSpawn) return;
            
            m_timeToNextSpawn -= Time.deltaTime;

            if (m_timeToNextSpawn > 0) return;

            SpawnEntity();
        }

        private void SpawnEntity() {
            var entityInfo = WaveData.EntityInfos[m_currentEntityIndex];
            
            EntityPosition entity = EntitiesPool.GetFromPool();
            
            switch (entityInfo.Type) {
                case WaveData.EntityType.DestructibleProp:
                    entity.DestructibleSetup(EntitiesPool.DestructibleEntity, EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.HardProp:
                    entity.HardPropSetup(EntitiesPool.HardPorpEntity, EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.Enemy:
                    entity.EnemySetup(EntitiesPool.EnemyEntity, EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.Shooter:
                    entity.ShooterSetup(EntitiesPool.ShooterEntity, EntitiesPool.PlayerLayer);
                    break;
                default:
                    return;
            }
            
            if (entityInfo.Type == WaveData.EntityType.Boss) {
                StartCoroutine(nameof(SpawnBoss));
            }
            else {
                entity.GetComponent<EntityPosition>().SetPosition(entityInfo, transform);    
            }

            m_timeToNextSpawn = entityInfo.TimeToNextEntity;
            m_currentEntityIndex += 1;

            if (m_currentEntityIndex < WaveData.EntityInfos.Count) return;
            
            m_waveFinishedSpawn = true;

            if (PlayerPrefs.GetInt("tutorial_finished") == 1) return;
            
            PlayerPrefs.SetInt("tutorial_finished", 1);
            PlayerPrefs.Save();
        }

        private IEnumerator SpawnBoss() {
            yield return new WaitForSeconds(6);
            GameManager.Instance.Dispatcher.Emit(new OnSpawnBoss());
        }
    }
}