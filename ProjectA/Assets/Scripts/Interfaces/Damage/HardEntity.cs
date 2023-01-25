using ProjectA.Entity.ProcessDamage;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class HardEntity : EntityProcessDamage, IDamageable {

        public void ProcessDamage(bool isCharged, PlayerHealth playerHealth) {
            playerHealth.TakeDamage(DamagePower);
        }
    }
}