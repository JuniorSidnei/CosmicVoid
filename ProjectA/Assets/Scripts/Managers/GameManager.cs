using System;
using HaremCity.Utils;
using ProjectA.Input;
using ProjectA.Movement;
using ProjectA.Utils;
using UnityEngine.SceneManagement;


namespace ProjectA.Singletons.Managers {
    
    public class GameManager : Singleton<GameManager> {

        private QueuedEventDispatcher m_dispatcher = new QueuedEventDispatcher();

        public QueuedEventDispatcher Dispatcher => m_dispatcher;

        public InputManager InputManager;

        public int HitCount = 0;

        public void UpdateHitCount(bool willReset = false) {
            if (willReset) HitCount = 0;
            else HitCount += 1;
            
            Dispatcher.Emit(new OnHitCountUpdate(HitCount));
        }
        
        private void Awake() {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        private void Update() {
            m_dispatcher.DispatchAll();
        }
    }
}