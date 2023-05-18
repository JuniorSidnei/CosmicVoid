using System.Collections;
using GameToBeNamed.Utils.Sound;
using HaremCity.Utils;
using ProjectA.Input;
using ProjectA.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectA.Singletons.Managers {
    
    public class GameManager : Singleton<GameManager> {

        private QueuedEventDispatcher m_dispatcher = new QueuedEventDispatcher();

        public QueuedEventDispatcher Dispatcher => m_dispatcher;

        public SaveLoadManager SaveLoadManager;
        public GameSettings GameSettings;
        public int SceneIndex;
        public int NextSceneIndex;
        public InputManager InputManager;
        public int HitCount = 0;

        public void UpdateHitCount(int value, bool willReset = false) {
            if (willReset) HitCount = 0;
            else HitCount += value;
            
            Dispatcher.Emit(new OnHitCountUpdate(HitCount));
        }
        
        public void OnBossDeath() {
            AudioController.Instance.Pause();
            AudioController.Instance.Play(GameSettings.BigExplosion, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            GameSettings.UpgradePlayerLife();
            InputManager.Disable();
            m_dispatcher.Emit(new OnCutsceneStarted());
            m_dispatcher.Emit(new OnBossDeath());
            Invoke(nameof(ShowHighScoreHit), 6);
        }

        public void ShowHighScoreHit() {
            Dispatcher.Emit(new OnShowStageScore(HitCount, NextSceneIndex));
        }

        private void Awake() {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            InputManager.DisablePlayerMovement();
            
            Dispatcher.Subscribe<OnReflectFeedback>(OnReflectFeedback);
            Dispatcher.Subscribe<OnCutSceneFinished>(OnCutSceneFinished);
            AudioController.Instance.ChangeVolume(AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale());
            AudioController.Instance.Play(GameSettings.GameTheme, AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale(), true);
        }

        private void OnCutSceneFinished(OnCutSceneFinished ev) {
            switch (SceneIndex) {
                case 2:
                    Dispatcher.Emit(new OnShowExtraTutorial(ExtraTutorialType.LINKER));
                    break;
                case 3:
                    Dispatcher.Emit(new OnShowExtraTutorial(ExtraTutorialType.CLOAKING));
                    break;
            }
        }

        private void Update() {
            m_dispatcher.DispatchAll();
        }

        private void OnReflectFeedback(OnReflectFeedback ev) {
            AudioController.Instance.Play(GameSettings.ReflectSound, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.BASIC));
            Time.timeScale = 0.01f;
            StartCoroutine(nameof(ReturnTimeScale));
        }
        
        private IEnumerator ReturnTimeScale() {
            yield return new WaitForSeconds(.0025f);
            Time.timeScale = 1f;
        }
    }
}