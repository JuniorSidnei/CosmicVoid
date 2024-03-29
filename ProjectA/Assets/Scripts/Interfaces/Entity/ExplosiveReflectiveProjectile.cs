using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectA.Entity {

    public class ExplosiveReflectiveProjectile : EntityProcessDamage {

        public override void ProcessDamage(bool isCharged) {
            if (!isCharged) {
                GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
                Destroy(this);
                GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            } else {
                GameManager.Instance.Dispatcher.Emit(new  OnReflectFeedback());
                GameManager.Instance.Dispatcher.Emit(new OnReflectEntity(this, true));    
            }
        }

        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
            Destroy(this);
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
        }
    }
}
