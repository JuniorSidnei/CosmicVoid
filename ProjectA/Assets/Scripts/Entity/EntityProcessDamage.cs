using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
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
            if(other.gameObject.CompareTag("Wall")) {
                ReleaseEntity();
                return;
            }
            
            if (IsReflected) {
                ProcessEntityCollision(other.gameObject);
            }
            else {
                ProcessPlayerCollision(other.gameObject);    
            }
        }

        private void ReleaseEntity() {
            
            var type = GetComponent<EntityPosition>().Type;
            switch (type) {
                case WaveData.EntityType.DestructibleProp:
                    GameManager.Instance.Dispatcher.Emit(new OnDestructibleEntityRelease(GetComponent<DestructibleEntity>()));
                    break;
                case WaveData.EntityType.HardProp:
                    GameManager.Instance.Dispatcher.Emit(new OnHardPropEntityRelease(GetComponent<HardEntity>()));
                    break;
                case WaveData.EntityType.Enemy:
                    GameManager.Instance.Dispatcher.Emit(new OnEnemyEntityRelease(GetComponent<EnemyEntity>()));
                    break;
                case WaveData.EntityType.Shooter:
                    GameManager.Instance.Dispatcher.Emit(new OnEnemyShooterEntityRelease(GetComponent<EnemyEntity>()));
                    break;
                case WaveData.EntityType.Reflective:
                    GameManager.Instance.Dispatcher.Emit(new OnReflectiveEntityRelease(GetComponent<ReflectiveEntity>()));
                    break;
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
            ReleaseEntity();
        }
        
        
    }
}