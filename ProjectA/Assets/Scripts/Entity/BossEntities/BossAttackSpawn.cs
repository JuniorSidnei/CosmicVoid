using System.Collections.Generic;
using ProjectA.Entity.Position;
using ProjectA.Scriptables.Boss;
using UnityEngine;

namespace ProjectA.Controllers {
    
    public class BossAttackSpawn : MonoBehaviour {

        public BossAttackWave BossAttackWave;
        
        private Queue<BossAttackWave.EntityInfo> m_entityQueue = new Queue<BossAttackWave.EntityInfo>();

        private float m_timeToNextSpawn;
        private bool m_waveFinishedSpawn;
        
        private void Awake() {
            foreach (var entity in BossAttackWave.EntityInfos) {
                m_entityQueue.Enqueue(entity);
            }

            m_timeToNextSpawn = BossAttackWave.InitialTimeSpawn;
        }
        
        private void Update() {

            if (m_waveFinishedSpawn) return;
            
            m_timeToNextSpawn -= Time.deltaTime;

            if (m_timeToNextSpawn > 0) return;

            SpawnEntity();
        }
        
        private void SpawnEntity() {
            var entity = m_entityQueue.Dequeue();
            var entityObject = Instantiate(BossAttackWave.GetEntity(entity.Type), transform.parent);
            entityObject.GetComponent<EntityPosition>().SetPosition(entity.Position);

            m_timeToNextSpawn = entity.TimeToNextEntity;

            if (m_entityQueue.Count > 0) return;
            
            m_waveFinishedSpawn = true;
        }
    }
}