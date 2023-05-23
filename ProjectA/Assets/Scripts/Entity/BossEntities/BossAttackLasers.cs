using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils.Sound;
using ProjectA.Data.Wave;
using ProjectA.Singletons.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectA.Controllers {
    
    public class BossAttackLasers : BossAttackSpawn {

        [Header("laser settings")]
        public WaveData ShieldBreakerPattern;
        public List<GameObject> LaserAnticipationViewList = new List<GameObject>();
        public List<GameObject> LaserList = new List<GameObject>();

        private bool m_isShieldActive = true;
        private int m_currentLaserIndex;
        private GameObject m_currentLaser;
        
        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnBossStartAttack>(OnBossStartAttack);
            GameManager.Instance.Dispatcher.Subscribe<OnShootLaser>(OnShootLaser);
            GameManager.Instance.Dispatcher.Subscribe<OnShieldBreak>(OnShieldBreak);
            m_currentPatternData = IsTestWave ? TestWave : ShieldBreakerPattern;
        }

        private void OnShieldBreak(OnShieldBreak arg0) {
            m_isShieldActive = false;
            
            CurrentPatternIndex = Random.Range(0, BossPatternsDatas.Count);
            m_currentPatternData = BossPatternsDatas[CurrentPatternIndex];
            EnqueueWave(m_currentPatternData);
        }

        private void OnShootLaser(OnShootLaser ev) {
            LaserAnticipationViewList[m_currentLaserIndex].SetActive(false);
            m_currentLaserIndex = (int)ev.Type;
            var currentAnticipation = LaserAnticipationViewList[m_currentLaserIndex];
            currentAnticipation.SetActive(true);
            //currentAnticipation.GetComponent<LineRenderer>().widthMultiplier = 3f;
            Invoke(nameof(HideAnticipation), 1f);
        }

        protected override void OnBossStartAttack(OnBossStartAttack ev) {
            m_waveFinishedSpawn = false;
            StartCoroutine(nameof(WaitToEnqueuePattern));
        }
        
        protected override IEnumerator WaitToEnqueuePattern() {
            yield return new WaitForSeconds(3);
            EnqueueWave(m_currentPatternData);
        }
        
        protected override void SelectRandomPattern() {
            
            switch (m_entityQueue.Count) {
                case <= 0 when m_isShieldActive: {
                    m_currentPatternData = IsTestWave ? TestWave : ShieldBreakerPattern;
                    EnqueueWave(m_currentPatternData);
                    break;
                }
                case <= 0 when !m_isRageActivated: {
                    CurrentPatternIndex = Random.Range(0, BossPatternsDatas.Count);
                    m_currentPatternData = IsTestWave ? TestWave : BossPatternsDatas[CurrentPatternIndex];
                    EnqueueWave(m_currentPatternData);
                    break;
                }
                case <= 0 when m_isRageActivated: {
                    CurrentPatternIndex = BossPatternsDatas.Count + 1;
                    m_currentPatternData = BossPatternRageData;
                    EnqueueWave(m_currentPatternData);
                    break;                    
                }
            }
        }

        private void HideAnticipation() {
            LaserAnticipationViewList[m_currentLaserIndex].SetActive(false);
            
            var currentLaser = LaserList[m_currentLaserIndex];
            currentLaser.SetActive(true);
            m_currentLaser = currentLaser;
            AudioController.Instance.Play(GameManager.Instance.GameSettings.Lightning, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale());
            
            //var line = currentLaser.GetComponent<LineRenderer>();
            //line.widthMultiplier = 0f;

            // DOTween.To(()=> line.widthMultiplier, width => line.widthMultiplier = width, 6f, .25f).OnComplete(()=> {
            //     HideLaser(line, currentLaser);
            // });
            
            Invoke(nameof(HideLaser), 0.35f);
        }
        
        private void HideLaser() {
            m_currentLaser.SetActive(false);
        }
    }
}