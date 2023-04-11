using ProjectA.Entity;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class HardEntity : EntityProcessDamage {

        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(true);
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
        }

        public override void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity) { base.ProcessProjectileDamage(reflectiveEntity); }

    }
}