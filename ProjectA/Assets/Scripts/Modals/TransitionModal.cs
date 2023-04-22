using System;
using DG.Tweening;
using ProjectA.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectA.Controllers {
    
    public class TransitionModal : Singleton<TransitionModal> {
        
        private static Image m_transition;
        
        private void Awake() {
            m_transition = GetComponent<Image>();
        }

        public void DoTransitionIn(Action onFinishTransition = null) {
            m_transition.DOFade(1f, 0.8f).OnComplete(() => {
                onFinishTransition?.Invoke();
            });
        }
        
        public void DoTransitionOut(Action onFinishTransition = null) {
            m_transition.DOFade(0, 0.8f).OnComplete(() => {
                onFinishTransition?.Invoke();
            });
        }
    }
}
