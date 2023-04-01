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
        
        private void Start() {
            GameManager.Instance.Dispatcher.Emit(new OnCutsceneStarted());
            PlayerTransform.DOMoveX(PlayerWalkPositionStart, 2.5f).SetEase(Ease.Linear).OnComplete(() => {
                GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(PlayerMovement.PlayerStates.IDLE));
                Invoke(nameof(WaitToExplosion), 2f);
            });
        }

        private void WaitToExplosion() {
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.STRONG));
            Destroy(Instantiate(ExplosionPrefab, ExplosionPosition.position, Quaternion.identity, transform), .5f);
            StartCoroutine(nameof(WaitToMovePlayer));    
        }
        
        private IEnumerator WaitToMovePlayer() {
            yield return new WaitForSeconds(.25f);
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
