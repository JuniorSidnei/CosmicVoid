using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ProjectA.Animators;
using ProjectA.Entity;
using ProjectA.Entity.Boss;
using ProjectA.Singletons.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectA.Actions {

    public class BossWeakSpotAction : BossEntityHealth {

        public List<BossWeakSpot> WeakSpots;
        public int HitsToWeakSpot;
        public float XPosition;
        public GameObject FallSmokePrefab;
        public Boss2Animator Boss2Animator;

        private int m_currentHitsToWeakSpot;
        private float m_timeStaggered = 4f;
        private bool m_triggerRageMode;

        protected override void OnHitBoss(OnHitBoss ev) {
            HitsHealth -= ev.Damage;
            Boss2Animator.AnimateHit();
            
            if (!m_triggerRageMode) {
                m_currentHitsToWeakSpot -= 1;    
            }

            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.MEDIUM));

            if (m_currentHitsToWeakSpot <= 0) {
                m_currentHitsToWeakSpot = HitsToWeakSpot;
                GameManager.Instance.Dispatcher.Emit(new OnBossStopAttack());

                Boss2Animator.AnimateFall(() => {
                    transform.localPosition = new Vector3(transform.localPosition.x, 0f, 1f);
                    var transformPosition = transform.position;
                    var particlePosition = new Vector3(transformPosition.x - 2f, transformPosition.y - 4f, transformPosition.z);
                    Instantiate(FallSmokePrefab, particlePosition, Quaternion.identity, transform);
                    GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.MEDIUM));
                    var randomSpot = Random.Range(0, WeakSpots.Count);

                    transform.DOLocalMoveX(XPosition, 3f).OnComplete(() => {
                        WeakSpots[randomSpot].SetOpen();
                    
                        var timeStaggeredCoroutine = WaitDelayStaggered(m_timeStaggered);
                        StartCoroutine(timeStaggeredCoroutine);
                    });
                });
            }
            
            if (HitsHealth <= 0) {
                GameManager.Instance.Dispatcher.Emit(new OnBossStopAttack());
                GameManager.Instance.InputManager.PlayerActions.Disable();
                Boss2Animator.AnimateDeath();
                Invoke(nameof(SpawnParticleEndStage),2.5f);
            }
        }

        private void Start() {
            GameManager.Instance.Dispatcher.Subscribe<OnWeakSpotDeath>(OnWeakSpotDeath);
            
            foreach (var weakSpot in WeakSpots) {
                weakSpot.EnableCollider(false);    
            }
            
            m_currentHitsToWeakSpot = HitsToWeakSpot;
            transform.localPosition = new Vector3(transform.localPosition.x, 1.3f, 1f);
        }

        private void OnWeakSpotDeath(OnWeakSpotDeath ev) {
            WeakSpots.Remove(ev.WeakSpot);

            m_timeStaggered += 4f;
            var timeStaggeredCoroutine = WaitDelayStaggered(0);
            StartCoroutine(timeStaggeredCoroutine);
            
            if (WeakSpots.Count <= 0) {
                m_triggerRageMode = true;
            }
        }
        
        private IEnumerator WaitDelayStaggered(float delay) {
            yield return new WaitForSeconds(delay);

            foreach (var weakSpot in WeakSpots) {
                weakSpot.SetIdle();    
            }

            if (m_triggerRageMode) {
                GameManager.Instance.Dispatcher.Emit(new OnBossRageMode());    
            } else {
                GameManager.Instance.Dispatcher.Emit(new OnBossStartAttack());    
            }
            
            Boss2Animator.AnimateStandUp(() => {
                transform.localPosition = new Vector3(-2f, 1.3f, 1f);    
            });
        }

        private void SpawnParticleEndStage() {
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShakeWithValues(0.6f, 2f));
            var transformPosition = transform.position;
            var particlePosition = new Vector3(transformPosition.x - 2f, transformPosition.y - 2f, transformPosition.z);
            Instantiate(DeathParticlePrefab, particlePosition, Quaternion.identity);
            GameManager.Instance.OnBossDeath();
            Destroy(gameObject);
        }
    }
}
