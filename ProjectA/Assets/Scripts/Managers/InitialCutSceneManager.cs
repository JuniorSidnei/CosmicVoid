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
            if (!GameManager.Instance.GameSettings.HasInitialCutSceneShow) {
                GameManager.Instance.Dispatcher.Emit(new OnInitialCutsceneStarted());
            }
            
            PlayerTransform.DOMoveX(PlayerWalkPositionStart, 2.5f).OnComplete(() => {
                GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(PlayerMovement.PlayerStates.IDLE));
                GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.STRONG));
                Destroy(Instantiate(ExplosionPrefab, ExplosionPosition.position, Quaternion.identity, transform), 1f);
                StartCoroutine(nameof(WaitToMovePlayer));
            });;
        }

        private IEnumerator WaitToMovePlayer() {
            yield return new WaitForSeconds(1f);
            GameManager.Instance.Dispatcher.Emit(new OnExplosionActivated());
            GameManager.Instance.GameSettings.SaveExplosionStatus();
            PlayerTransform.DOMoveX(PlayerWalkPositionFinish, 2f).OnComplete(() => {
                GameManager.Instance.Dispatcher.Emit(new OnInitialCutSceneFinished());
                GameManager.Instance.GameSettings.SaveInitialCutsceneStatus();
                GameManager.Instance.InputManager.EnablePlayerMovement();
            });
        }
    }
}
