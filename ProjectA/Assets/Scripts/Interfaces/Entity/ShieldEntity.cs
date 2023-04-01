using System;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {

    public class ShieldEntity : EntityProcessDamage {

        public int Health;
        
        private int m_heatlh;
        
        public override void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity) {
            var entityPosition = reflectiveEntity.gameObject.GetComponent<EntityPosition>();
            
            if (entityPosition.Type != WaveData.EntityType.ShieldBreaker) {
                GameManager.Instance.Dispatcher.Emit(new OnReflectEntity(reflectiveEntity, true));
                return;
            }

            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(entityPosition));
            m_heatlh -= 1;

            if (m_heatlh <= 0) {
                Destroy(gameObject);
            }
        }

        private void Start() {
            m_heatlh = Health;
        }
    }
}
