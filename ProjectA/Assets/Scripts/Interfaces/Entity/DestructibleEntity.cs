using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;

namespace ProjectA.Interface {
    
    public class DestructibleEntity : EntityProcessDamage {
        
        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(0, true);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.STUNNED));
            base.ProcessPlayerDamage(isCharged);
        }
        
        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(1);
            base.ProcessDamage(isCharged);
        }
    }
}