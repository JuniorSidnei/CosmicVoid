using System.Collections;
using System.Collections.Generic;
using ProjectA.Modals;
using ProjectA.Singletons.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectA.Input {
    
    public class InputManager : MonoBehaviour {

        private InputSource m_inputSource;

        public InputAction MoveUp => m_inputSource.Player.Up;
        public InputAction MoveDown => m_inputSource.Player.Down;
        public InputAction Attack => m_inputSource.Player.Attack;
        public InputSource.PlayerActions PlayerActions => m_inputSource.Player;

        public void Disable() {
            m_inputSource.Disable();    
        }

        public void Enable() {
            m_inputSource.Enable();
        }
        
        public void DisablePlayerMovement() {
            m_inputSource ??= new InputSource();
            
            m_inputSource.Player.Up.Disable();
            m_inputSource.Player.Down.Disable();
            m_inputSource.Player.Attack.Disable();
        }
        
        public void EnablePlayerMovement() {
            m_inputSource.Player.Up.Enable();
            m_inputSource.Player.Down.Enable();
            m_inputSource.Player.Attack.Enable();
        }
        
        private void OnEnable() {
            if(!GameManager.Instance.GameSettings.HasInitialCutSceneShow) return;
            
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