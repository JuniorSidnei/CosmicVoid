using System.Collections;
using DG.Tweening;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Managers {

    public class InitialCutSceneManager : MonoBehaviour {

        public Transform PlayerTransform;
        public Transform ExplosionPosition;
        public GameObject ExplosionPrefab;
        public float PlayerWalkPositionStart;
        public float PlayerWalkPositionFinish;
        public GameObject ExclamationPrefab;
        
        private void Start() {
            GameManager.Instance.Dispatcher.Emit(new OnCutsceneStarted());
            PlayerTransform.DOMoveX(PlayerWalkPositionStart, 2.5f).SetEase(Ease.Linear).OnComplete(() => {
                GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(PlayerMovement.PlayerStates.IDLE));
                Invoke(nameof(WaitToExplosion), 1.8f);
            });
        } 

        private void WaitToExplosion() {
            Destroy(Instantiate(ExclamationPrefab, PlayerTransform.position + new Vector3(0, 1f, 0), Quaternion.identity), .3f);
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShakeWithValues(5.0f, 0.5f));
            Destroy(Instantiate(ExplosionPrefab, ExplosionPosition.position, Quaternion.identity, transform), .5f);
            StartCoroutine(nameof(WaitToMovePlayer));    
        }
        
        private IEnumerator WaitToMovePlayer() {
            yield return new WaitForSeconds(.35f);
            GameManager.Instance.Dispatcher.Emit(new OnExplosionActivated());
            GameManager.Instance.GameSettings.SaveExplosionStatus();
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(PlayerMovement.PlayerStates.RUNNING));
            PlayerTransform.DOMoveX(PlayerWalkPositionFinish, 2f).SetEase(Ease.Linear).OnComplete(() => {
                GameManager.Instance.Dispatcher.Emit(new OnCutSceneFinished());
                GameManager.Instance.GameSettings.SaveInitialCutsceneStatus();
                GameManager.Instance.InputManager.EnablePlayerMovement();
            });
        }
    }
}
