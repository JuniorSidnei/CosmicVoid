using System;
using DG.Tweening;
using ProjectA.Attack;
using ProjectA.Input;
using UnityEngine;

namespace ProjectA.Movement {

    public class PlayerMovement : MonoBehaviour {

        public enum PlayerPosition {
            UP, MID, DOWN
        }

        public enum PlayerStates {
            MOVEUP, MOVEDOWN, ATTACK, CHARGEDATTACK, IDLE, CHARGED, UP_CHARGED, DOWN_CHARGED
        }
        
        public InputManager InputManager;
        public PlayerPosition Position;
        public PlayerStates State;

        private PlayerAttack m_playerAttack;
        
        public bool IsMoving { get; private set; }

        private void Awake() {
            m_playerAttack = GetComponent<PlayerAttack>();
        }

        private void Start() {
            State = PlayerStates.IDLE;
            
            InputManager.MoveDown.performed += ctx => MoveDown();
            InputManager.MoveUp.performed += ctx => MoveUp();
        }

        private void MoveUp() {
            if (IsMoving) return;
            
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    return;
                case PlayerPosition.MID:
                    State = m_playerAttack.IsCharged() ? PlayerStates.UP_CHARGED : PlayerStates.MOVEUP;
                    newPos = new Vector2(-6f, 2.5f);
                    Position = PlayerPosition.UP;
                    break;
                case PlayerPosition.DOWN:
                    State = m_playerAttack.IsCharged() ? PlayerStates.UP_CHARGED : PlayerStates.MOVEUP;
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
            }

            IsMoving = true;
            transform.DOMove(newPos, 0.15f).SetEase(Ease.InBounce).OnComplete(()=> {
                State = PlayerStates.IDLE;
                IsMoving = false;
            });
        }
        
        private void MoveDown() {
            if (IsMoving) return;
            
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    State = m_playerAttack.IsCharged() ? PlayerStates.DOWN_CHARGED : PlayerStates.MOVEDOWN;
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
                case PlayerPosition.MID:
                    State = m_playerAttack.IsCharged() ? PlayerStates.DOWN_CHARGED : PlayerStates.MOVEDOWN;;
                    newPos = new Vector2(-6f, -1.5f);
                    Position = PlayerPosition.DOWN;
                    break;
                case PlayerPosition.DOWN:
                    return;
            }

            IsMoving = true;
            transform.DOMove(newPos, 0.15f).SetEase(Ease.InBounce).OnComplete(()=> {
                State = m_playerAttack.IsCharged() ? PlayerStates.CHARGED : PlayerStates.IDLE;
                IsMoving = false;
            });
        }
    }
}