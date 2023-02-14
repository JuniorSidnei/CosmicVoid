using DG.Tweening;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.Boss {
    
    public class BossEntity : EntityProcessDamage {

        public float XPosition;
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnSpawnBoss>(OnSpawnBoss);
            transform.localPosition = new Vector3(2.5f, 0, 0);
        }

        private void OnSpawnBoss(OnSpawnBoss ev) {
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(1.2f, 2.5f));
            GameManager.Instance.InputManager.PlayerActions.Disable();
            //show cutscene animation
            transform.DOLocalMoveX(XPosition, 2f).OnComplete(() => {
                GameManager.Instance.InputManager.PlayerActions.Enable();
            });
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            GameManager.Instance.UpdateHitCount();
            GameManager.Instance.Dispatcher.Emit(new OnHitBoss(this, isReflected, damagePower));
        }
    }
}