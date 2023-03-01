using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.Position;
using ProjectA.Scriptables.Boss;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Controllers {
    
    public class BossAttackSpawn : MonoBehaviour {

        public List<BossAttackWave> BossPatternsDatas;
        public BossAttackWave BossPatternRageData;
        
        private Queue<BossAttackWave.EntityInfo> m_entityQueue = new Queue<BossAttackWave.EntityInfo>();
        
        private float m_timeToNextSpawn;
        private bool m_waveFinishedSpawn;
        private bool m_isRageActivated;
        private BossAttackWave m_currentPatternData;
        
        private void Awake() {
            m_currentPatternData = BossPatternsDatas[Random.Range(0, 4)];

            GameManager.Instance.Dispatcher.Subscribe<OnBossRageMode>(OnBossRageMode);
            GameManager.Instance.Dispatcher.Subscribe<OnBossStartAttack>(OnBossStartAttack);
        }

        private void OnBossStartAttack(OnBossStartAttack ev) {
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
            EnqueueWave(BossPatternRageData);
        }

        private void Update() {

            if (m_waveFinishedSpawn) return;
            
            m_timeToNextSpawn -= Time.deltaTime;

            if (m_timeToNextSpawn > 0 || m_entityQueue.Count <= 0) return;

            SpawnEntity();
        }

        private void EnqueueWave(BossAttackWave wave) {
            foreach (var entity in wave.EntityInfos) {
                m_entityQueue.Enqueue(entity);
            }
            
            m_timeToNextSpawn = wave.InitialTimeSpawn;
        }
        
        private void SpawnEntity() {
            var entity = m_entityQueue.Dequeue();
            var entityObject = Instantiate(m_currentPatternData.GetEntity(entity.Type), transform);
            entityObject.GetComponent<EntityPosition>().SetPosition(entity, transform);

            m_timeToNextSpawn = entity.TimeToNextEntity;

            switch (m_entityQueue.Count) {
                case <= 0 when !m_isRageActivated: {
                    m_currentPatternData = BossPatternsDatas[Random.Range(0, 4)];
                    Debug.Log("current data: " + m_currentPatternData.name);
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