using System.Collections;
using System.Collections.Generic;
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
        }

        private void OnDisable() {
            m_inputSource.Disable();
        }
    }
}