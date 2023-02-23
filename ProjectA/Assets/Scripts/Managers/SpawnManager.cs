using System.Collections;
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
            var entityObject = Instantiate(WaveData.WavePrefabs.GetEntity(entity.Type), transform);

            if (entity.Type == WaveData.EntityType.Boss) {
                GameManager.Instance.Dispatcher.Emit(new OnSpawnBoss());
            }
            else {
                entityObject.GetComponent<EntityPosition>().SetPosition(entity.Position);    
            }

            m_timeToNextSpawn = entity.TimeToNextEntity;
            m_currentEntityIndex += 1;

            if (m_currentEntityIndex < WaveData.EntityInfos.Count) return;
            
            m_waveFinishedSpawn = true;
            
            if (!WaveData.IsTutorialWave) {
                StartCoroutine(nameof(LoadNextScene));
                return;
            }

            if (PlayerPrefs.GetInt("tutorial_finished") == 1) return;
            
            PlayerPrefs.SetInt("tutorial_finished", 1);
            PlayerPrefs.Save();
        }

        private IEnumerator LoadNextScene() {
            yield return new WaitForSeconds(6);
            GameManager.Instance.LoadNextScene();
        }
    }
}