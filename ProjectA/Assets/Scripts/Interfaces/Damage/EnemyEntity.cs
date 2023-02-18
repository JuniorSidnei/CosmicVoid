using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;

namespace ProjectA.Interface {
    
    public class EnemyEntity : EntityProcessDamage {
        
        public override void ProcessPlayerDamage(bool isCharged) {
             GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.BASIC));
             GameManager.Instance.UpdateHitCount(true);
        }
        
        public override void ProcessDamage(bool isCharged) {
            if (!isCharged) {
                GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.BASIC));
                GameManager.Instance.UpdateHitCount(true);
            }
            else {
                GameManager.Instance.UpdateHitCount();
                Destroy(gameObject);
            }
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            Destroy(gameObject);
        }
    }
}