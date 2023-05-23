using UnityEngine;

namespace ProjectA.Scriptables {
    
    [CreateAssetMenu(menuName = "ProjectA/Entity/Info", fileName = "_info")]
    public class EntityInfo : ScriptableObject {
        
        public RuntimeAnimatorController Controller;
        public int DamagePower;
        public float Speed;
        public LayerMask PlayerLayer;
        public Color EntityColor;
        public GameObject EntityExplosionParticle;
    }
}