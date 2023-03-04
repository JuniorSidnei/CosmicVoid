using ProjectA.Data.Wave;
using ProjectA.Entity;
using ProjectA.Managers;
using UnityEngine;

namespace ProjectA.Actions {
    
    public class Shoot : MonoBehaviour {

        public GameObject SpawnPrefab;
        public Vector3 Spawn;
        public float ShootInterval;

        private float m_shootInterval;
        private GameObject m_spawnGameObject;
        private float m_minimumDistanceToShoot = 5f;
        private Transform m_playerTransform;
        
        private void Start() {
            m_shootInterval = ShootInterval;
            m_spawnGameObject = Instantiate(SpawnPrefab, transform);
            m_spawnGameObject.transform.localPosition = Spawn;
            m_playerTransform = GameObject.FindWithTag("Player").transform;
        }

        private void Update() {
            if(Vector3.Distance(transform.position, m_playerTransform.position) < m_minimumDistanceToShoot) return;
            
            m_shootInterval -= Time.deltaTime;

            if (m_shootInterval <= 0) {
                ShootProjectile();
            }
        }

        private void ShootProjectile() {
            var projectile = SpawnManager.Instance.ProjectilesPool.GetFromPool();
            projectile.ReflectiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ReflectiveEntity, SpawnManager.Instance.EntitiesPool.Layers());
            projectile.Type = WaveData.EntityType.Reflective;
            projectile.transform.SetParent(SpawnManager.Instance.transform);
            projectile.name = projectile.Type.ToString();
            var localPositionX = transform.localPosition.x + m_spawnGameObject.transform.localPosition.x;
            var localPositionY = transform.localPosition.y;
            projectile.transform.localPosition = new Vector3(localPositionX, localPositionY, 1f);
            m_shootInterval = ShootInterval;
        }
    }
}
