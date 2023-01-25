using System;
using System.Collections;
using ProjectA.Parabola;
using UnityEngine;

namespace HaremCity.Controllers {
    
    public class ParabolaController : MonoBehaviour {

        public ParabolaInfoData ParabolaInfoData;
        
        private float m_elapsedTime;
        private Vector3 m_target;
        private Vector3 m_startPosition;
        private Action m_onParabolaFinished;
        private bool m_isLocal;
        
        public void SetTarget(Vector3 startPosition, Vector3 target, bool isLocal, Action onParabolaFinished = null) {
            m_startPosition = startPosition;
            m_target = target;
            m_isLocal = isLocal;
            m_onParabolaFinished = onParabolaFinished;
            StartCoroutine(nameof(Curve));
        }

        public IEnumerator Curve() {

            while (m_elapsedTime < ParabolaInfoData.DurationToTarget) {
                
                m_elapsedTime += Time.deltaTime;

                var percentTime = m_elapsedTime / ParabolaInfoData.DurationToTarget;
                var heightCurve = ParabolaInfoData.AnimationCurve.Evaluate(percentTime);

                var height = Mathf.Lerp(0f, ParabolaInfoData.HeightApex, heightCurve);

                if (m_isLocal) {
                    transform.localPosition = Vector2.Lerp(m_startPosition, m_target, percentTime) + new Vector2(0f, height);
                }
                else {
                    transform.position = Vector2.Lerp(m_startPosition, m_target, percentTime) + new Vector2(0f, height);    
                }
                
                yield return null;
            }

            m_onParabolaFinished?.Invoke();
        }
    }
}