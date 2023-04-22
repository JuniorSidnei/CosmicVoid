using ProjectA.Actions;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;

namespace ProjectA.Entity {
    
    public class ShooterEntity : EntityProcessDamage {

        public override void ProcessDamage(bool isCharged) {
            DestroySettings();
            GameManager.Instance.UpdateHitCount(0,true);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.STUNNED));
            base.ProcessDamage(isCharged);
        }

        public override void ProcessPlayerDamage(bool isCharged) {
            DestroySettings();
            GameManager.Instance.UpdateHitCount(isCharged ? 2 : 1);
            base.ProcessPlayerDamage(isCharged);
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            DestroySettings();
            base.ProcessProjectileDamage(isReflected, damagePower);
        }

        private void DestroySettings() {
            Destroy(GetComponent<Shoot>());
            Destroy(transform.GetChild(1).gameObject);
        }
    }
}
