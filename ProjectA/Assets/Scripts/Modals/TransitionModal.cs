using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LustyGod.Controllers {
    
    public class TransitionModal : MonoBehaviour {
        
        private static Image m_transition;

        private void Awake() {
            m_transition = GetComponent<Image>();
        }

        public static void DoTransitionIn(Action onFinishTransition = null) {
            m_transition.DOFade(1, 0.8f).OnComplete(() => {
                onFinishTransition?.Invoke();
            });
        }
        
        public static void DoTransitionOut(Action onFinishTransition = null) {
            m_transition.DOFade(0, 0.8f).OnComplete(() => {
                onFinishTransition?.Invoke();
            });
        }
    }
}
