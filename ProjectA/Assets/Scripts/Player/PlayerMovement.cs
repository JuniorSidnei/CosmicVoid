using System;
using DG.Tweening;
using ProjectA.Input;
using UnityEngine;

namespace ProjectA.Movement {

    public class PlayerMovement : MonoBehaviour {

        public enum PlayerPosition {
            UP, MID, DOWN
        }

        public enum PlayerStates {
            MOVEUP, MOVEDOWN, ATTACK, CHARGEDATTACK, IDLE
        }
        
        public InputManager InputManager;
        public PlayerPosition Position;
        public PlayerStates State;

        private bool m_isMoving;
        
        private void Start() {
            State = PlayerStates.IDLE;
            
            InputManager.MoveDown.performed += ctx => MoveDown();
            InputManager.MoveUp.performed += ctx => MoveUp();
        }

        private void MoveUp() {
            if (m_isMoving) return;
            
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    return;
                case PlayerPosition.MID:
                    State = PlayerStates.MOVEUP;
                    newPos = new Vector2(-6f, 2.5f);
                    Position = PlayerPosition.UP;
                    break;
                case PlayerPosition.DOWN:
                    State = PlayerStates.MOVEUP;
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
            }

            m_isMoving = true;
            transform.DOMove(newPos, 0.15f).SetEase(Ease.InBounce).OnComplete(()=> {
                State = PlayerStates.IDLE;
                m_isMoving = false;
            });
        }
        
        private void MoveDown() {
            if (m_isMoving) return;
            
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    State = PlayerStates.MOVEDOWN;
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
                case PlayerPosition.MID:
                    State = PlayerStates.MOVEDOWN;
                    newPos = new Vector2(-6f, -1.5f);
                    Position = PlayerPosition.DOWN;
                    break;
                case PlayerPosition.DOWN:
                    return;
            }

            m_isMoving = true;
            transform.DOMove(newPos, 0.15f).SetEase(Ease.InBounce).OnComplete(()=> {
                State = PlayerStates.IDLE;
                m_isMoving = false;
            });
        }
    }
}