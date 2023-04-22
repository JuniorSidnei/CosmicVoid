using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;

namespace ProjectA.Interface {
    
    public class EnemyEntity : EntityProcessDamage {
        
        public override void ProcessPlayerDamage(bool isCharged) {
             GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.BASIC));
             GameManager.Instance.UpdateHitCount(0, true);
        }
        
        public override void ProcessDamage(bool isCharged) {
            if (!isCharged) {
                GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.BASIC));
                GameManager.Instance.UpdateHitCount(0, true);
            }
            else {
                GameManager.Instance.UpdateHitCount(2);
                base.ProcessDamage(true);
            }
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            ReleaseEntity();
        }

        private void ReleaseEntity() {
            Destroy(this);
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
        }
    }
}