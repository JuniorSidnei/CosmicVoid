using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.Boss {
    
    public class BossEntity : EntityProcessDamage {
        
        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            GameManager.Instance.Dispatcher.Emit(new OnHitBoss(this, isReflected, damagePower));
        }
    }
}