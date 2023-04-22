using System.Collections;
using HaremCity.Utils;
using ProjectA.Input;
using ProjectA.Movement;
using ProjectA.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ProjectA.Singletons.Managers {
    
    public class GameManager : Singleton<GameManager> {

        private QueuedEventDispatcher m_dispatcher = new QueuedEventDispatcher();

        public QueuedEventDispatcher Dispatcher => m_dispatcher;

        public GameSettings GameSettings;
        public int NextSceneIndex;
        public InputManager InputManager;
        public int HitCount = 0;

        public void UpdateHitCount(int value, bool willReset = false) {
            if (willReset) HitCount = 0;
            else HitCount += value;
            
            Dispatcher.Emit(new OnHitCountUpdate(HitCount));
        }
        
        public void OnBossDeath() {
            var currentMaxLife = PlayerPrefs.GetInt("player_max_life", 3);
            PlayerPrefs.SetInt("player_max_life", currentMaxLife + 1);
            PlayerPrefs.Save();
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
            GameSettings.SetStatus();
            
            Dispatcher.Subscribe<OnReflectFeedback>(OnReflectFeedback);
        }

        private void Update() {
            m_dispatcher.DispatchAll();
        }

        private void OnReflectFeedback(OnReflectFeedback ev) {
            Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.BASIC));
            Time.timeScale = 0.01f;
            StartCoroutine(nameof(ReturnTimeScale));
        }
        
        private IEnumerator ReturnTimeScale() {
            yield return new WaitForSeconds(.001f);
            Time.timeScale = 1f;
        }
    }
}