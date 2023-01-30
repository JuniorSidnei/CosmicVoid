using ProjectA.Interface;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.ProcessDamage {
    
    public abstract class EntityProcessDamage : MonoBehaviour, IDamageable {

        public int DamagePower;
        public LayerMask PlayerLayer;

        private void OnTriggerEnter2D(Collider2D other) {
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            var o = other.gameObject;

            var playerState = o.GetComponent<PlayerMovement>().State;
            
            
            ProcessPlayerDamage(playerState == PlayerMovement.PlayerStates.ATTACK || playerState == PlayerMovement.PlayerStates.CHARGEDATTACK);
        }

        public virtual void ProcessDamage(bool isCharged) {
        }

        public virtual void ProcessPlayerDamage(bool isCharged) {
            
        }
    }
}