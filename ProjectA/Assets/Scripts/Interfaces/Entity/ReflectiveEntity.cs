using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {
    
    public class ReflectiveEntity : EntityProcessDamage {

        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.Dispatcher.Emit(new  OnReflectFeedback());
            GameManager.Instance.Dispatcher.Emit(new OnReflectEntity(this, isCharged));
        }

        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.MEDIUM));
            Destroy(this);
            GameManager.Instance.Dispatcher.Emit(new OnProjectileEntityRelease(GetComponent<EntityPosition>()));
        }
    }
}