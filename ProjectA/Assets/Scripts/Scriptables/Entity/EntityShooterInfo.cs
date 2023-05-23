using UnityEngine;

namespace ProjectA.Scriptables {
    
    [CreateAssetMenu(menuName = "ProjectA/Entity/ShooterInfo", fileName = "_info")]
    public class EntityShooterInfo : EntityInfo {
        
        public GameObject SpawnPrefab;
        public float ShootInterval;
    }
}