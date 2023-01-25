using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Managers {
    
    public class SpawnManager : MonoBehaviour {

        public WaveData WaveData;
        
        private float m_timeToNextSpawn;
        private int m_currentEntityIndex = 0;
        private bool m_waveFinishedSpawn;
        
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
            var entityObject = Instantiate(WaveData.GetEntity(entity.Type), transform);
            entityObject.GetComponent<EntityPosition>().SetPosition(entity.Position);

            m_timeToNextSpawn = entity.TimeToNextEntity;
            m_currentEntityIndex += 1;

            if (m_currentEntityIndex < WaveData.EntityInfos.Count) return;
            
            m_currentEntityIndex = WaveData.EntityInfos.Count;
            m_waveFinishedSpawn = true;
        }
    }
}