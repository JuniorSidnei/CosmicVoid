using System.Collections;
using System.Net;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Pools;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Managers {
    
    public class SpawnManager : MonoBehaviour {

        public WaveData WaveData;

        public DestructiblePropPool DestructiblePool;
        public HardPropPool HardPropPool;
        public EnemyPool EnemyPool;
        public EnemyShooterPool EnemyShooterPool;
        
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
            var entity = WaveData.EntityInfos[m_currentEntityIndex];
            GameObject entityObject = gameObject;
            
            switch (entity.Type) {
                case WaveData.EntityType.DestructibleProp:
                    entityObject = DestructiblePool.GetFromPool();
                    break;
                case WaveData.EntityType.HardProp:
                    entityObject = HardPropPool.GetFromPool();
                    break;
                case WaveData.EntityType.Enemy:
                    entityObject = EnemyPool.GetFromPool();
                    break;
                case WaveData.EntityType.Shooter:
                    entityObject = EnemyShooterPool.GetFromPool();
                    break;
                default:
                    return;
            }
            
            if (entity.Type == WaveData.EntityType.Boss) {
                StartCoroutine(nameof(SpawnBoss));
            }
            else {
                entityObject.GetComponent<EntityPosition>().SetPosition(entity, transform);    
            }

            m_timeToNextSpawn = entity.TimeToNextEntity;
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