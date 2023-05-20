using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {

    public class ExplosiveReflectiveProjectile : EntityProcessDamage {

        public override void ProcessDamage(bool isCharged) {
            if (!isCharged) {
                GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
                Destroy(this);
                GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
                Instantiate(Resources.Load("MultipleExplosions"), transform.position, Quaternion.identity);
            } else {
                GameManager.Instance.Dispatcher.Emit(new OnReflectFeedback());
                GameManager.Instance.Dispatcher.Emit(new OnReflectEntity(this, true));    
            }
        }

        public override void ProcessPlayerDamage(bool isCharged) {
            Instantiate(Resources.Load("MultipleExplosions"), transform.position, Quaternion.identity);
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
            Destroy(this);
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
        }
    }
}
