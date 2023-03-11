using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Managers {
    
    public class TutorialManager : MonoBehaviour {

        [Serializable]
        public struct TutorialInfo {
            public GameObject TutorialText;
            public float TimeDelay;
        }
        
        public float InitialDelay;
        public List<TutorialInfo> TutorialTexts = new List<TutorialInfo>();

        private float m_delayBetweenTexts;
        private int m_currentTextIndex;

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnCutSceneFinished>(OnInitialCutSceneFinished);
        }

        private void OnInitialCutSceneFinished(OnCutSceneFinished ev) {
            if (PlayerPrefs.GetInt("tutorial_finished") == 1) {
                enabled = false;
                return;
            }
            
            m_delayBetweenTexts = InitialDelay;
            StartCoroutine(nameof(ShowTextDelay));
        }

        private IEnumerator ShowTextDelay() {
            if (m_currentTextIndex >= TutorialTexts.Count) {
                yield break;
            }
            
            yield return new WaitForSeconds(m_delayBetweenTexts);
            HideAndShowNext();
        }

        private void HideAndShowNext() {
            TutorialTexts[m_currentTextIndex].TutorialText.SetActive(true);
            m_delayBetweenTexts = TutorialTexts[m_currentTextIndex].TimeDelay;
            StartCoroutine(nameof(HideText));
        }
        
        private IEnumerator HideText() {
            if (m_currentTextIndex >= TutorialTexts.Count) {
                yield break;
            }
            
            yield return new WaitForSeconds(m_delayBetweenTexts);
            TutorialTexts[m_currentTextIndex].TutorialText.SetActive(false);
            m_currentTextIndex += 1;
            StartCoroutine(nameof(ShowTextDelay));
        }
    }
}