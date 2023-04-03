using System.Collections;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;

namespace ProjectA.Interface {
    
    public class DestructibleEntity : EntityProcessDamage {
        
        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(true);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.STUNNED));
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            Destroy(this);
            gameObject.SetActive(false);
        }
        
        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount();
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            Destroy(this);
            gameObject.SetActive(false);
        }
    }
}