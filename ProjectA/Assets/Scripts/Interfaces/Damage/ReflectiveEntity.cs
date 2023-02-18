using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {
    
    public class ReflectiveEntity : EntityProcessDamage {
        
        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.Dispatcher.Emit(new OnReflectEntity(this, isCharged));
        }

        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.BASIC));
            Destroy(gameObject);
        }
    }
}