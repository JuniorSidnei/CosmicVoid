using ProjectA.Managers;
using UnityEngine;

namespace ProjectA.Actions {
    
    public class Shoot : MonoBehaviour {

        public GameObject Projectile;
        public Transform Spawn;
        public float ShootInterval;

        private float m_shootInterval;

        private void Start() {
            m_shootInterval = ShootInterval;
        }

        private void Update() {
            m_shootInterval -= Time.deltaTime;

            if (m_shootInterval <= 0) {
                ShootProjectile();
            }
        }

        private void ShootProjectile() {
            // var projectile = SpawnManager.Instance.ReflectivePool.GetFromPool();
            // projectile.transform.position = Spawn.position;
            // projectile.transform.SetParent(SpawnManager.Instance.transform);
            // m_shootInterval = ShootInterval;
        }
    }
}
