using System.Collections;
using ProjectA.Actions;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;

namespace ProjectA.Interface {
    
    public class DestructibleEntity : EntityProcessDamage {
        
        public override void ProcessPlayerDamage(bool isCharged) {
            var type = GetComponent<EntityPosition>().Type;

            switch (type) {
                case WaveData.EntityType.Shooter:
                    Destroy(GetComponent<Shoot>());
                    Destroy(transform.GetChild(1).gameObject);
                    break;
            }
            
            GameManager.Instance.UpdateHitCount(true);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.STUNNED));
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            Destroy(this);
            gameObject.SetActive(false);
        }
        
        public override void ProcessDamage(bool isCharged) {
            var type = GetComponent<EntityPosition>().Type;

            switch (type) {
                case WaveData.EntityType.Shooter:
                    Destroy(GetComponent<Shoot>());
                    Destroy(transform.GetChild(1).gameObject);
                    break;
            }
            
            GameManager.Instance.UpdateHitCount();
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            Destroy(this);
            gameObject.SetActive(false);
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            var type = GetComponent<EntityPosition>().Type;

            switch (type) {
                case WaveData.EntityType.Shooter:
                    Destroy(GetComponent<Shoot>());
                    Destroy(transform.GetChild(1).gameObject);
                    break;
            }

            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            Destroy(this);
            gameObject.SetActive(false);
        }
    }
}