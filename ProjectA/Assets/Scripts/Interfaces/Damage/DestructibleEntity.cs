using System;
using ProjectA.Entity.ProcessDamage;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class DestructibleEntity : EntityProcessDamage, IDamageable {
        
        public void ProcessDamage(bool isCharged, PlayerHealth playerHealth) {
            Destroy(gameObject);
        }
    }
}