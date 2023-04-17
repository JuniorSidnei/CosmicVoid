using ProjectA.Data.Wave;
using ProjectA.Managers;
using UnityEngine;

namespace ProjectA.Actions {
    
    public class Shoot : MonoBehaviour {

        public GameObject SpawnPrefab;
        public Vector3 Spawn;
        public float ShootInterval;

        private float m_shootInterval;
        private GameObject m_spawnGameObject;
        private float m_minimumDistanceToShoot = 10f;
        private Transform m_playerTransform;
        private Animator m_animator;
        
        private void Start() {
            m_shootInterval = ShootInterval;
            m_spawnGameObject = Instantiate(SpawnPrefab, transform);
            m_spawnGameObject.transform.localPosition = Spawn;
            m_playerTransform = GameObject.FindWithTag("Player").transform;
            m_animator = GetComponentInChildren<Animator>();
        }

        private void Update() {
            if(Vector3.Distance(transform.position, m_playerTransform.position) < m_minimumDistanceToShoot) return;
            
            m_shootInterval -= Time.deltaTime;

            if (m_shootInterval <= 0) {
                ShootProjectile();
            }
        }

        private void ShootProjectile() {
            m_animator.CrossFade("shoot", 0.0f);
            Invoke(nameof(SpawnProjectile), 0.2f);
            m_shootInterval = ShootInterval;
        }

        private void SpawnProjectile() {
            var projectile = SpawnManager.Instance.ProjectilesPool.GetFromPool();
            projectile.ReflectiveProjectileSetup(SpawnManager.Instance.ProjectilesPool.ReflectiveEntity, SpawnManager.Instance.EntitiesPool.Layers());
            projectile.Type = WaveData.EntityType.Reflective;
            projectile.transform.SetParent(SpawnManager.Instance.transform);
            projectile.name = projectile.Type.ToString();
            var localPositionX = transform.localPosition.x + m_spawnGameObject.transform.localPosition.x;
            var localPositionY = transform.localPosition.y + 0.25f;
            projectile.transform.localPosition = new Vector3(localPositionX, localPositionY, 1f);
        }
    }
}
