using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        
        private int m_currentHitsToWeakSpot;
        private float m_timeStaggered = 4f;
        private bool m_triggerRageMode;

        protected override void OnHitBoss(OnHitBoss ev) {
            HitsHealth -= ev.Damage;

            if (!m_triggerRageMode) {
                m_currentHitsToWeakSpot -= 1;    
            }

            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ShakeForce.MEDIUM));

            if (m_currentHitsToWeakSpot <= 0) {
                m_currentHitsToWeakSpot = HitsToWeakSpot;
                var randomSpot = Random.Range(0, WeakSpots.Count);

                GameManager.Instance.Dispatcher.Emit(new OnBossStopAttack());
                transform.DOLocalMoveX(XPosition, 3f).OnComplete(() => {
                    WeakSpots[randomSpot].SetOpen();
                    
                    var timeStaggeredCoroutine = WaitDelayStaggered(m_timeStaggered);
                    StartCoroutine(timeStaggeredCoroutine);
                });
            }
            
            if (HitsHealth <= 0) {
                //play boss death animation
                GameManager.Instance.OnBossDeath();
            }
        }

        private void Start() {
            GameManager.Instance.Dispatcher.Subscribe<OnWeakSpotDeath>(OnWeakSpotDeath);
            
            foreach (var weakSpot in WeakSpots) {
                weakSpot.EnableCollider(false);    
            }
            
            m_currentHitsToWeakSpot = HitsToWeakSpot;
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
            
            transform.DOLocalMoveX(-2f, 3f);
        }
    }
}
