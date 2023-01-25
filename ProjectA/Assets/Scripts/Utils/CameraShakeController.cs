// using Cinemachine;
// using UnityEngine;
//
// namespace ProjectA.Camera {
//     
//     public class CameraShakeController : MonoBehaviour {
//         
//         private CinemachineVirtualCamera m_virtualCamera;
//         private CinemachineBasicMultiChannelPerlin m_basicPerlin;
//         
//         private float m_initialIntensity;
//         private float m_shakeTimer;
//         private float m_shakeTotalTimer;
//         
//         private void Awake() {
//             m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
//             m_basicPerlin = m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
//             //GameManager.Instance.GlobalDispatcher.Subscribe<OnCameraScreenShake>(OnCameraScreenShake);
//         }
//
//         // private void OnCameraScreenShake(OnCameraScreenShake ev) {
//         //     ShakeCamera(ev.Force, ev.Duration);
//         // }
//
//         private void ShakeCamera(float intensity, float shakeTimer) {
//             m_basicPerlin.m_AmplitudeGain = intensity;
//             m_shakeTimer = shakeTimer;
//             m_shakeTotalTimer = shakeTimer;
//             m_initialIntensity = intensity;
//         }
//
//         private void Update() {
//             if (!(m_shakeTimer > 0)) return;
//             
//             m_shakeTimer -= Time.deltaTime;
//             if (m_shakeTimer <= 0) {
//                 m_basicPerlin.m_AmplitudeGain = Mathf.Lerp(m_initialIntensity, 0, 1 - (m_shakeTimer / m_shakeTotalTimer));
//             }
//         }
//     }
// }