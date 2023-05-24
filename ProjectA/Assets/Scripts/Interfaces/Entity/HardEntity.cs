using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;

namespace ProjectA.Interface {
    
    public class HardEntity : EntityProcessDamage {

        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(0,true);
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(1, ShakeForce.STRONG));
        }
        
        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(0,true);
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
        }
    }
}