using System;
using System.Collections;
using System.Collections.Generic;
using HaremCity.Utils;
using ProjectA.Controllers;
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

        public void UpdateHitCount(bool willReset = false) {
            if (willReset) HitCount = 0;
            else HitCount += 1;
            
            Dispatcher.Emit(new OnHitCountUpdate(HitCount));
        }
        
        public void OnBossDeath() {
            InputManager.Disable();
            m_dispatcher.Emit(new OnCutsceneStarted());
            m_dispatcher.Emit(new OnBossDeath());
            Invoke(nameof(LoadNextScene), 10f);
        }
        
        public void LoadNextScene() {
            var nextScene = "GameScene_" + NextSceneIndex;
            TransitionModal.DoTransitionIn(()=> SceneManager.LoadScene(nextScene));
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