using System.Collections;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {

    public class ShieldEntity : EntityProcessDamage {

        public int Health;
        public Animator Animator;
        
        private int m_heatlh;
        
        public override void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity) {
            var entityPosition = reflectiveEntity.gameObject.GetComponent<EntityPosition>();
            
            if (entityPosition.Type != WaveData.EntityType.ShieldBreaker) {
                GameManager.Instance.Dispatcher.Emit(new OnReflectEntity(reflectiveEntity, true));
                return;
            }

            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(entityPosition));
            m_heatlh -= 1;
            Animator.CrossFade("shield_hit", 0.1f);
            
            if (m_heatlh <= 0) {
                Animator.CrossFade("shield_destroy", 0.1f);
                Invoke(nameof(DestroyShield), 0.35f);
            }
        }

        private void Start() {
            m_heatlh = Health;

            StartCoroutine(ShowExtraTutorialText());
        }

        private void DestroyShield() {
            Destroy(gameObject);
        }

        private IEnumerator ShowExtraTutorialText() {
            yield return new WaitForSeconds(6f);
            
            if (GameManager.Instance.GameSettings.HasExtratutorialStepFourShowed) {
                yield break;
            }

            GameManager.Instance.Dispatcher.Emit(new OnShowExtraTutorial(ExtraTutorialType.SHIELDBREAKER));
            GameManager.Instance.GameSettings.HasExtratutorialStepFourShowed = true;
        }
    }
}
