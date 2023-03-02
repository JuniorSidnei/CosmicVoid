using System;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class EnemyEntity : EntityProcessDamage {
        private void OnDestroy()
        {
            Debug.Log("fui destruido");
        }

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
                case WaveData.EntityType.Enemy:
                    GameManager.Instance.Dispatcher.Emit(new OnEnemyEntityRelease(GetComponent<EnemyEntity>()));        
                    break;
                case WaveData.EntityType.Shooter:
                    GameManager.Instance.Dispatcher.Emit(new OnEnemyShooterEntityRelease(GetComponent<EnemyEntity>()));   
                    break;
            }
        }
    }
}