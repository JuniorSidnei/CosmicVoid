using ProjectA.Interface;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.ProcessDamage {
    
    public abstract class EntityProcessDamage : MonoBehaviour, IDamageable {

        public int DamagePower;
        public LayerMask PlayerLayer;
        public LayerMask EntityLayer;
        
        public bool IsReflected { get; set; }

        public virtual void ProcessDamage(bool isCharged) { }
        public virtual void ProcessPlayerDamage(bool isCharged) { }
        public virtual void ProcessProjectileDamage(bool isReflected, int damagePower) { }

        private void OnTriggerEnter2D(Collider2D other) {
            if (IsReflected) {
                ProcessEntityCollision(other.gameObject);
            }
            else {
                ProcessPlayerCollision(other.gameObject);    
            }
        }

        private void ProcessPlayerCollision(GameObject player) {
            if(((1 << player.layer) & PlayerLayer) == 0) {
                return;
            }

            var playerState = player.GetComponent<PlayerMovement>().State;
            
            ProcessPlayerDamage(playerState == PlayerMovement.PlayerStates.ATTACK || playerState == PlayerMovement.PlayerStates.CHARGEDATTACK);
        }

        private void ProcessEntityCollision(GameObject entity) {
            if(((1 << entity.layer) & EntityLayer) == 0) {
                return;
            }

            entity.GetComponent<IDamageable>().ProcessProjectileDamage(IsReflected, DamagePower);
        }
    }
}