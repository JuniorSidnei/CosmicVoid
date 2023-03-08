using System;
using Cinemachine;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Camera {
    
    public class CameraShakeController : MonoBehaviour {
        
        private CinemachineVirtualCamera m_virtualCamera;
        private CinemachineBasicMultiChannelPerlin m_basicPerlin;

        public ShakeForceInfo BasicShake;
        public ShakeForceInfo MediumShake;
        public ShakeForceInfo StrongShake;
        
        private float m_initialIntensity;
        private float m_shakeTimer;
        private float m_shakeTotalTimer;
        
        private void Awake() {
            m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
            m_basicPerlin = m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            GameManager.Instance.Dispatcher.Subscribe<OnCameraScreenShake>(OnCameraScreenShake);
            GameManager.Instance.Dispatcher.Subscribe<OnCameraScreenShakeWithValues>(OnCameraScreenShakeWithValues);
        }

        private void OnCameraScreenShakeWithValues(OnCameraScreenShakeWithValues ev) {
            ShakeCamera(ev.Force, ev.Time);
        }

        private void OnCameraScreenShake(OnCameraScreenShake ev) {
            var force = GetForce(ev.Force);
            ShakeCamera(force.ShakeForce, force.ShakeTime);
        }

        private ShakeForceInfo GetForce(ShakeForce force) {
            return force switch {
                ShakeForce.BASIC => BasicShake,
                ShakeForce.MEDIUM => MediumShake,
                ShakeForce.STRONG => StrongShake,
                _ => throw new ArgumentOutOfRangeException(nameof(force), force, null)
            };
        }

        private void ShakeCamera(float intensity, float shakeTimer) {
            m_basicPerlin.m_AmplitudeGain = intensity;
            m_shakeTimer = shakeTimer;
            m_shakeTotalTimer = shakeTimer;
            m_initialIntensity = intensity;
        }

        private void Update() {
            if (!(m_shakeTimer > 0)) return;
            
            m_shakeTimer -= Time.deltaTime;
            if (m_shakeTimer <= 0) {
                m_basicPerlin.m_AmplitudeGain = Mathf.Lerp(m_initialIntensity, 0, 1 - (m_shakeTimer / m_shakeTotalTimer));
            }
        }
    }
}