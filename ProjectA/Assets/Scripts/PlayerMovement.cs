using System;
using DG.Tweening;
using ProjectA.Input;
using UnityEngine;

namespace ProjectA.Movement {

    public class PlayerMovement : MonoBehaviour {

        public enum PlayerPosition {
            UP, MID, DOWN
        }
        
        public InputManager InputManager;
        public PlayerPosition Position;
        
        private void Start() {
            InputManager.MoveDown.performed += ctx => MoveDown();
            InputManager.MoveUp.performed += ctx => MoveUp();
        }

        private void MoveUp() {
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    return;
                case PlayerPosition.MID:
                    newPos = new Vector2(-6f, 2.5f);
                    Position = PlayerPosition.UP;
                    break;
                case PlayerPosition.DOWN:
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
            }

            transform.DOMove(newPos, 0.15f).SetEase(Ease.InBounce);
        }
        
        private void MoveDown() {
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
                case PlayerPosition.MID:
                    newPos = new Vector2(-6f, -1.5f);
                    Position = PlayerPosition.DOWN;
                    break;
                case PlayerPosition.DOWN:
                    return;
            }

            transform.DOMove(newPos, 0.15f).SetEase(Ease.InBounce);
        }
    }
}