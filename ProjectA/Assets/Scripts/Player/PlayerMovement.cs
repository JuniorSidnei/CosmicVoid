using System;
using DG.Tweening;
using ProjectA.Attack;
using ProjectA.Input;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Movement {

    public class PlayerMovement : MonoBehaviour {

        public enum PlayerPosition {
            UP, MID, DOWN
        }

        public enum PlayerStates {
            MOVEUP, MOVEDOWN, ATTACK, CHARGEDATTACK, IDLE, CHARGED, UP_CHARGED, DOWN_CHARGED, STUNNED, RUNNING
        }
        
        public InputManager InputManager;
        public PlayerPosition Position;
        public PlayerStates State;
        public float TimeStunned;

        private PlayerAttack m_playerAttack;
        private float m_timeStunned;

        private bool m_isMoving;

        private void Awake() {
            m_playerAttack = GetComponent<PlayerAttack>();
        }

        private void Start() {
            State = PlayerStates.RUNNING;
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(State));
            m_timeStunned = TimeStunned;
            InputManager.MoveDown.performed += ctx => MoveDown();
            InputManager.MoveUp.performed += ctx => MoveUp();
            GameManager.Instance.Dispatcher.Subscribe<OnPlayerStateSet>(OnPlayerStateSet);
        }

        private void OnPlayerStateSet(OnPlayerStateSet ev) {
            State = ev.NewState;
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(State));
        }
        
        private void Update() {
            if (State != PlayerStates.STUNNED) return;

            m_timeStunned -= Time.deltaTime;

            if (m_timeStunned > 0) return;
            
            State = PlayerStates.RUNNING;
            OnPlayerStateSet(new OnPlayerStateSet(State));
            m_timeStunned = TimeStunned;
        }

        private void MoveUp() {
            if (m_isMoving || State == PlayerStates.STUNNED) return;
            
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    return;
                case PlayerPosition.MID:
                    State = m_playerAttack.IsCharged() ? PlayerStates.UP_CHARGED : PlayerStates.MOVEUP;
                    OnPlayerStateSet(new OnPlayerStateSet(State));
                    newPos = new Vector2(-6f, 2.5f);
                    Position = PlayerPosition.UP;
                    break;
                case PlayerPosition.DOWN:
                    State = m_playerAttack.IsCharged() ? PlayerStates.UP_CHARGED : PlayerStates.MOVEUP;
                    OnPlayerStateSet(new OnPlayerStateSet(State));
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
            }

            m_isMoving = true;
            transform.DOMove(newPos, 0.25f).SetEase(Ease.Linear).OnComplete(SetStateAfterMoving);
        }
        
        private void MoveDown() {
            if (m_isMoving || State == PlayerStates.STUNNED) return;
            
            var newPos = Vector2.zero;
            
            switch (Position) {
                case PlayerPosition.UP:
                    State = m_playerAttack.IsCharged() ? PlayerStates.DOWN_CHARGED : PlayerStates.MOVEDOWN;
                    OnPlayerStateSet(new OnPlayerStateSet(State));
                    newPos = new Vector2(-6f, .5f);
                    Position = PlayerPosition.MID;
                    break;
                case PlayerPosition.MID:
                    State = m_playerAttack.IsCharged() ? PlayerStates.DOWN_CHARGED : PlayerStates.MOVEDOWN;
                    OnPlayerStateSet(new OnPlayerStateSet(State));
                    newPos = new Vector2(-6f, -1.5f);
                    Position = PlayerPosition.DOWN;
                    break;
                case PlayerPosition.DOWN:
                    return;
            }

            m_isMoving = true;
            GameManager.Instance.Dispatcher.Emit(new OnPlayerMoving(true));
            transform.DOMove(newPos, 0.25f).SetEase(Ease.Linear).OnComplete(SetStateAfterMoving);
        }

        private void SetStateAfterMoving() {
            if(m_playerAttack.IsCharged() && State != PlayerStates.STUNNED) {
                State = PlayerStates.CHARGED;
            } else if (!m_playerAttack.IsCharged() && State != PlayerStates.STUNNED) {
                State = PlayerStates.RUNNING;
            } else if (State == PlayerStates.STUNNED) {
                State = PlayerStates.STUNNED;
            }
            
            m_isMoving = false;
            GameManager.Instance.Dispatcher.Emit(new OnPlayerMoving(false));
            OnPlayerStateSet(new OnPlayerStateSet(State));
        }
    }
}