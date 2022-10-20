using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectA.Input {
    
    public class InputManager : MonoBehaviour {

        private InputSource m_inputsource;

        public InputAction MoveUp => m_inputsource.Player.Up;
        public InputAction MoveDown => m_inputsource.Player.Down;
        public InputAction Attack => m_inputsource.Player.Attack;

        private void OnEnable() {
            m_inputsource = new InputSource();
            m_inputsource.Enable();
        }

        private void OnDisable() {
            m_inputsource.Disable();
        }
    }
}