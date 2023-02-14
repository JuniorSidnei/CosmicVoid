using System.Collections;
using System.Collections.Generic;
using ProjectA.Modals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectA.Input {
    
    public class InputManager : MonoBehaviour {

        private InputSource m_inputSource;

        public InputAction MoveUp => m_inputSource.Player.Up;
        public InputAction MoveDown => m_inputSource.Player.Down;
        public InputAction Attack => m_inputSource.Player.Attack;
        public InputSource.PlayerActions PlayerActions => m_inputSource.Player;
        
        private void OnEnable() {
            m_inputSource = new InputSource();
            m_inputSource.Enable();
            
            m_inputSource.Player.Pause.performed += ctx => PauseGame();
        }

        private void PauseGame() {
            if (PauseModal.Instance.IsGamePaused) {
                PauseModal.Instance.ResumeGame();
            }
            else {
                PauseModal.Instance.PauseGame();
            }
        }

        private void OnDisable() {
            m_inputSource.Disable();
        }
    }
}