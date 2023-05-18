using DG.Tweening;
using GameToBeNamed.Utils.Sound;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity.Boss {
    
    public class BossEntity : EntityProcessDamage {

        public float XPosition;
        public bool WillOverride;
        public ExtraTutorialType ExtraTutorialType;
        public AudioClip BossTheme;
        public AudioClip BossHit;
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnSpawnBoss>(OnSpawnBoss);
            transform.localPosition = new Vector3(2.5f, 0, 0);
        }

        private void OnSpawnBoss(OnSpawnBoss ev) {
            GameManager.Instance.Dispatcher.Emit(new OnCutsceneStarted());
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShakeWithValues(1.2f, 2.5f));
            GameManager.Instance.InputManager.PlayerActions.Disable();
            AudioController.Instance.Pause();
            AudioController.Instance.Play(BossTheme, AudioController.SoundType.Music, GameManager.Instance.GameSettings.GetMusicVolumeReduceScale(), true);
            transform.DOLocalMoveX(XPosition, 2f).OnComplete(() => {
                GameManager.Instance.InputManager.PlayerActions.Enable();
                GameManager.Instance.Dispatcher.Emit(new OnBossStartAttack());
                GameManager.Instance.Dispatcher.Emit(new OnCutSceneFinished());
                GameManager.Instance.Dispatcher.Emit(new OnShowExtraTutorial(ExtraTutorialType));
            });
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            GameManager.Instance.UpdateHitCount(isReflected ? 2 : 1);
            GameManager.Instance.Dispatcher.Emit(new OnHitBoss(this, isReflected, damagePower, WillOverride));
            AudioController.Instance.Play(BossHit, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale());
        }
    }
}