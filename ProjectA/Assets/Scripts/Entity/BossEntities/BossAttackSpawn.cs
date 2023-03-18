using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Managers;
using ProjectA.Pools;
using ProjectA.Scriptables.Boss;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Controllers {
    
    public class BossAttackSpawn : MonoBehaviour {

        public List<WaveData> BossPatternsDatas;
        public WaveData BossPatternRageData;
        
        private Queue<WaveData.EntityInfo> m_entityQueue = new Queue<WaveData.EntityInfo>();
        
        private float m_timeToNextSpawn;
        private bool m_waveFinishedSpawn;
        private bool m_isRageActivated;
        private WaveData m_currentPatternData;
        
        private void Awake() {
            m_currentPatternData = BossPatternsDatas[Random.Range(0, BossPatternsDatas.Count)];

            GameManager.Instance.Dispatcher.Subscribe<OnBossRageMode>(OnBossRageMode);
            GameManager.Instance.Dispatcher.Subscribe<OnBossStartAttack>(OnBossStartAttack);
            GameManager.Instance.Dispatcher.Subscribe<OnBossStopAttack>(OnBossStopAttack);
            GameManager.Instance.Dispatcher.Subscribe<OnBossDeath>(OnBossDeath);
        }

        private void OnBossStopAttack(OnBossStopAttack arg0) {
            m_waveFinishedSpawn = true;
            m_timeToNextSpawn = Mathf.Infinity;
            m_entityQueue.Clear();
        }

        private void OnBossDeath(OnBossDeath arg0) {
            m_waveFinishedSpawn = true;
        }

        private void OnBossStartAttack(OnBossStartAttack ev) {
            m_waveFinishedSpawn = false;
            StartCoroutine(nameof(WaitToEnqueuePattern));
        }

        private IEnumerator WaitToEnqueuePattern() {
            yield return new WaitForSeconds(5);
             EnqueueWave(m_currentPatternData);
        }

        private void OnBossRageMode(OnBossRageMode ev) {
            if (m_isRageActivated) return;
            
            m_isRageActivated = true;
            m_entityQueue.Clear();
            m_currentPatternData = BossPatternRageData;
            m_waveFinishedSpawn = false;
            EnqueueWave(BossPatternRageData);
        }

        private void Update() {

            if (m_waveFinishedSpawn) return;
            
            m_timeToNextSpawn -= Time.deltaTime;

            if (m_timeToNextSpawn > 0 || m_entityQueue.Count <= 0) return;

            SpawnEntity();
        }

        private void EnqueueWave(WaveData wave) {
            m_entityQueue.Clear();
            
            foreach (var entity in wave.EntityInfos) {
                m_entityQueue.Enqueue(entity);
            }
            
            m_timeToNextSpawn = wave.InitialTimeSpawn;
        }
        
        private void SpawnEntity() {
            var entityInfo = m_entityQueue.Dequeue();

            EntityPosition entity = SpawnManager.Instance.ProjectilesPool.GetFromPool();

            switch (entityInfo.Type) {
                case WaveData.EntityType.Reflective:
                    entity.ReflectiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ReflectiveBossEntity, SpawnManager.Instance.EntitiesPool.Layers());
                    break;
                case WaveData.EntityType.HardProjectile:
                    entity.HardProjectileSetup(SpawnManager.Instance.ProjectilesPool.HardProjectileEntity, SpawnManager.Instance.EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.Explosive:
                    entity.ExplosiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ExplosiveEntity, SpawnManager.Instance.EntitiesPool.Layers());
                    break;
            }

            entity.SetPosition(entityInfo, transform);    
            
            m_timeToNextSpawn = entityInfo.TimeToNextEntity;

            switch (m_entityQueue.Count) {
                case <= 0 when !m_isRageActivated: {
                    m_currentPatternData = BossPatternsDatas[Random.Range(0, BossPatternsDatas.Count)];
                    EnqueueWave(m_currentPatternData);
                    break;
                }
                case <= 0 when m_isRageActivated:
                    m_currentPatternData = BossPatternRageData;
                    EnqueueWave(BossPatternRageData);
                    break;
            }
        }
    }
}