using System;
using System.Collections;
using System.Net;
using ProjectA.Controllers;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Pools;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectA.Managers {
    
    public class SpawnManager : Singleton<SpawnManager> {

        public WaveData WaveData;
        
        public EntitiesPool EntitiesPool;
        public ProjectilesPool ProjectilesPool;
        
        private float m_timeToNextSpawn;
        private int m_currentEntityIndex = 0;
        private bool m_waveFinishedSpawn;
        private bool m_isBossSpawned;

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);
        }
        
        private void Start() {
            if (!WaveData.IsTutorialWave) {
                m_timeToNextSpawn = WaveData.InitialTimeSpawn;
                m_waveFinishedSpawn = false;
            }

            m_waveFinishedSpawn = true;
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
            entity.name = entityInfo.Type.ToString();
            
            if (entityInfo.Type != WaveData.EntityType.Boss) {
                entity.SetPosition(entityInfo, transform);
            }
            
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
                    entity.ShooterSetup((EntityShooterInfo)EntitiesPool.ShooterEntity, EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.Linker:
                    entity.LinkerSetup(EntitiesPool.LinkerEntity, EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.Boss:
                    Instantiate(WaveData.WavePrefabs.Boss, transform);
                    StartCoroutine(nameof(SpawnBoss));
                    m_waveFinishedSpawn = true;
                    EntitiesPool.Release(entity);
                    break;
            }

            m_timeToNextSpawn = entityInfo.TimeToNextEntity;
            m_currentEntityIndex += 1;

            if (m_currentEntityIndex < WaveData.EntityInfos.Count) return;
            
            m_waveFinishedSpawn = true;

            if (PlayerPrefs.GetInt("tutorial_finished") == 1) return;
            
            GameManager.Instance.GameSettings.SetTutorialStatus(1);
            StartCoroutine(nameof(LoadGameScene));
        }

        private IEnumerator SpawnBoss() {
            yield return new WaitForSeconds(6);
            GameManager.Instance.Dispatcher.Emit(new OnSpawnBoss());
        }

        private IEnumerator LoadGameScene() {
            yield return new WaitForSeconds(5);
            TransitionModal.DoTransitionIn(() => { SceneManager.LoadScene("GameScene_1");});
        }
        
        private void OnInitialCutSceneFinished(OnCutSceneFinished arg0) {
            m_waveFinishedSpawn = false;
            m_timeToNextSpawn = WaveData.InitialTimeSpawn;
            GameManager.Instance.Dispatcher.Unsubscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);
        }
    }
}