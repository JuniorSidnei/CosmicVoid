using GameToBeNamed.Utils.Sound;
using ProjectA.Modals;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class PlayerHealth : MonoBehaviour {

        public GameObject OnHitPrefab;
        public GameObject OnDeathPrefab;
        private int m_currentHealth;

        private void Awake() {
            m_currentHealth = GameManager.Instance.GameSettings.GameDifficulty == GameDifficulty.HARD ? 1 : GameManager.Instance.GameSettings.PlayerMaxLife;
            GameManager.Instance.Dispatcher.Subscribe<OnDamagePlayer>(OnDamagePlayer);
            GameManager.Instance.Dispatcher.Subscribe<OnSpawnBoss>(OnSpawnBoss);
        }

        private void OnDamagePlayer(OnDamagePlayer ev) {
            if(!GameManager.Instance.GameSettings.HasTutorialFinished) return;
            
            AudioController.Instance.Play(GameManager.Instance.GameSettings.PlayerHit, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale());
            m_currentHealth -= ev.Damage;
            Instantiate(OnHitPrefab, transform.position, Quaternion.identity, transform);
            
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ev.ShakeForce));

            if (m_currentHealth <= 0) {
                AudioController.Instance.Play(GameManager.Instance.GameSettings.BigExplosion, AudioController.SoundType.SoundEffect2D, GameManager.Instance.GameSettings.GetSfxVolumeReduceScale());
                m_currentHealth = 0;
                Instantiate(OnDeathPrefab, transform.position, Quaternion.identity, transform);
                PauseModal.Instance.PauseGame(true);
            }
            
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(PlayerMovement.PlayerStates.HIT));
            GameManager.Instance.Dispatcher.Emit(new OnPlayerLifeUpdate(m_currentHealth));
        }

        private void OnSpawnBoss(OnSpawnBoss ev) {
            if(GameManager.Instance.GameSettings.GameDifficulty == GameDifficulty.EASY) {
                m_currentHealth = GameManager.Instance.GameSettings.PlayerMaxLife;
                GameManager.Instance.Dispatcher.Emit(new OnPlayerRechargeLife());
            }
        }
    }
}