using System;
using ProjectA.Actions;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

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
                ReleaseEntity();
            }
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            ReleaseEntity();
        }

        private void ReleaseEntity() {
            var type = GetComponent<EntityPosition>().Type;
            switch (type) {
                case WaveData.EntityType.Shooter:
                    Destroy(GetComponent<Shoot>());
                    Destroy(transform.GetChild(1).gameObject);
                    break;
            }
            
            Destroy(this);
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
        }
    }
}