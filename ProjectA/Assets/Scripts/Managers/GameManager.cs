using System;
using HaremCity.Utils;
using ProjectA.Controllers;
using ProjectA.Input;
using ProjectA.Movement;
using ProjectA.Utils;
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
        
        private void Awake() {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            InputManager.DisablePlayerMovement();
            GameSettings.SetStatus();
        }

        private void Update() {
            m_dispatcher.DispatchAll();
        }

        public void LoadNextScene() {
            var nextScene = "GameScene_" + NextSceneIndex;
            TransitionModal.DoTransitionIn(()=> SceneManager.LoadScene(nextScene));
        }
    }
}