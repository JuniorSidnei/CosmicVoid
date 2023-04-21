using System;
using DG.Tweening;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Animators {
    
    public class Boss2Animator : MonoBehaviour {

        public Material DissolveMaterial;
        
        private Animator m_animator;
        private Action m_onFinishFallCallback;
        private Action m_onFinishStandUpCallback;
        
        
        public void AnimateFall(Action onFinishFallCallback) {
            m_onFinishFallCallback = onFinishFallCallback;
            m_animator.CrossFade("fall", 0.1f);    
        }

        public void OnFinishFall() {
            m_onFinishFallCallback?.Invoke();
        }

        public void AnimateStandUp(Action onFinishStandUpCallback) {
            m_onFinishStandUpCallback = onFinishStandUpCallback;
            m_animator.CrossFade("standup", 0.1f);
        }

        public void OnFinishStandUp() {
            DissolveMaterial.DOFloat(0f, "_DissolveRange", 1f).OnComplete(() => {
                m_onFinishStandUpCallback?.Invoke();
                DissolveMaterial.DOFloat(1f, "_DissolveRange", 2f);
            });  
        }
        
        public void AnimateHit() {
            m_animator.CrossFade("hit", 0.1f);
        }

        public void AnimateDeath() {
            m_animator.CrossFade("death", 0.1f);
        }
        
        private void Awake() {
            m_animator = GetComponent<Animator>();
        }

        private void Start() {
            DissolveMaterial.DOFloat(0, "_DissolveRange", 0f);
            GameManager.Instance.Dispatcher.Subscribe<OnBossStartAttack>(OnBossStartAttack);
        }

        private void OnBossStartAttack(OnBossStartAttack arg0) {
            DissolveMaterial.DOFloat(1f, "_DissolveRange", 2f);
        }
    }

}