using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Managers;
using ProjectA.Singletons.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectA.Controllers {
    
    public class BossAttackSpawn : MonoBehaviour {

        public List<WaveData> BossPatternsDatas;
        public WaveData BossPatternRageData;
        public GameObject MuzzleShootReflectivePrefab;
        public GameObject MuzzleShootHardPrefab;
        public bool UseReplacedTransformSpawn;
        public Transform ReplacedTransformSpawn;
        protected Queue<WaveData.EntityInfo> m_entityQueue = new Queue<WaveData.EntityInfo>();
        
        private float m_timeToNextSpawn;
        protected bool m_waveFinishedSpawn;
        protected bool m_isRageActivated;
        protected WaveData m_currentPatternData;
        
        public int CurrentPatternIndex { get; set; }
        
        private void Awake() {
            CurrentPatternIndex = Random.Range(0, BossPatternsDatas.Count);
            m_currentPatternData = BossPatternsDatas[CurrentPatternIndex];

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

        protected virtual void OnBossStartAttack(OnBossStartAttack ev) {
            m_waveFinishedSpawn = false;
            StartCoroutine(nameof(WaitToEnqueuePattern));
        }

        protected virtual IEnumerator WaitToEnqueuePattern() {
            yield return new WaitForSeconds(3);
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

        protected void EnqueueWave(WaveData wave) {
            m_entityQueue.Clear();
            
            foreach (var entity in wave.EntityInfos) {
                m_entityQueue.Enqueue(entity);
            }
            
            m_timeToNextSpawn = wave.InitialTimeSpawn;
        }
        
        private void SpawnEntity() {
            var entityInfo = m_entityQueue.Dequeue(); 

            switch (entityInfo.Type) {
                case WaveData.EntityType.LaserUp:
                    GameManager.Instance.Dispatcher.Emit(new OnShootLaser(LaserPosition.UP));
                    m_timeToNextSpawn = entityInfo.TimeToNextEntity;
                    SelectRandomPattern();
                    return;
                case WaveData.EntityType.LaserMid:
                    GameManager.Instance.Dispatcher.Emit(new OnShootLaser(LaserPosition.MID));
                    m_timeToNextSpawn = entityInfo.TimeToNextEntity;
                    SelectRandomPattern();
                    return;
                case WaveData.EntityType.LaserDown:
                    GameManager.Instance.Dispatcher.Emit(new OnShootLaser(LaserPosition.BOTTOM));
                    m_timeToNextSpawn = entityInfo.TimeToNextEntity;
                    SelectRandomPattern();
                    return;
            }
            
            //GameManager.Instance.Dispatcher.Emit(new OnAnimateShootPosition(entityInfo));
            
            EntityPosition entity = SpawnManager.Instance.ProjectilesPool.GetFromPool();
            entity.transform.localScale = Vector3.one;
            
            var portal = MuzzleShootReflectivePrefab;
            
            switch (entityInfo.Type) {
                case WaveData.EntityType.Reflective:
                    entity.ReflectiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ReflectiveBossEntity, SpawnManager.Instance.EntitiesPool.Layers());
                    break;
                case WaveData.EntityType.HardProjectile:
                    portal = MuzzleShootHardPrefab;
                    entity.HardProjectileSetup(SpawnManager.Instance.ProjectilesPool.HardProjectileEntity, SpawnManager.Instance.EntitiesPool.PlayerLayer);
                    break;
                case WaveData.EntityType.Explosive:
                    entity.ExplosiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ExplosiveEntity, SpawnManager.Instance.EntitiesPool.Layers());
                    break;
                case WaveData.EntityType.ShieldBreaker:
                    //GameManager.Instance.Dispatcher.Emit(new OnShootLaser(LaserPosition.UP));
                    entity.ReflectiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ShieldBreakerEntity, SpawnManager.Instance.EntitiesPool.Layers());
                    break;
            }
            
            entity.SetPositionAndTypeWithX(entityInfo, UseReplacedTransformSpawn ? ReplacedTransformSpawn : transform,  -2f);

            var offsetPosition = new Vector3(4f, UseReplacedTransformSpawn ? 0.5f : 0f, 1);
            
            switch (entityInfo.Position) {
                case WaveData.EntityPosition.Up:
                    offsetPosition.y = UseReplacedTransformSpawn ? 2.8f : 2.3f;
                    break;
                case WaveData.EntityPosition.Down:
                    offsetPosition.y = UseReplacedTransformSpawn ? -2.0f : -2.5f;
                    break;
            }

            Instantiate(portal, offsetPosition, Quaternion.identity, transform);
            m_timeToNextSpawn = entityInfo.TimeToNextEntity;
            SelectRandomPattern();
        }

        protected virtual void SelectRandomPattern() {
            switch (m_entityQueue.Count) {
                case <= 0 when !m_isRageActivated: {
                    CurrentPatternIndex = Random.Range(0, BossPatternsDatas.Count);
                    m_currentPatternData = BossPatternsDatas[CurrentPatternIndex];
                    EnqueueWave(m_currentPatternData);
                    break;
                }
                case <= 0 when m_isRageActivated:
                    CurrentPatternIndex = BossPatternsDatas.Count + 1;
                    m_currentPatternData = BossPatternRageData;
                    EnqueueWave(BossPatternRageData);
                    break;
            }
        }

        public void Shoot(WaveData.EntityInfo entityInfo) {
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
                case WaveData.EntityType.ShieldBreaker:
                    //GameManager.Instance.Dispatcher.Emit(new OnShootLaser(LaserPosition.UP));
                    entity.ReflectiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ShieldBreakerEntity, SpawnManager.Instance.EntitiesPool.Layers());
                    break;
            }

            entity.SetPositionAndTypeWithX(entityInfo, transform, transform.localPosition.x);    
            
            m_timeToNextSpawn = entityInfo.TimeToNextEntity;
            SelectRandomPattern();
        }
    }
}