using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class DestructibleEntity : EntityProcessDamage {
        
        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(true);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.STUNNED));
            GameManager.Instance.Dispatcher.Emit(new OnDestructibleEntityRelease(GetComponent<DestructibleEntity>()));
        }
        
        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount();
            GameManager.Instance.Dispatcher.Emit(new OnDestructibleEntityRelease(GetComponent<DestructibleEntity>()));
        }
    }
}