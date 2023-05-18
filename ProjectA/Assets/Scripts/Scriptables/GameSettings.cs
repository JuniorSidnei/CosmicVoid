using UnityEngine;

namespace ProjectA {
    
     [CreateAssetMenu(menuName = "ProjectA/Settings", fileName = "GameSettings")]   
     public class GameSettings : ScriptableObject {
        public bool HasTutorialFinished;
        public GameDifficulty GameDifficulty;
        public int PlayerMaxLife;
        public bool HasUnlockedStage2;
        public bool HasUnlockedStage3;
        public bool HasExtratutorialStepOneShowed;
        public bool HasExtratutorialStepTwoShowed;
        public bool HasExtratutorialStepThreeShowed;
        public bool HasExtratutorialStepFourShowed;   
        public float MusicVolume;
        public float SfxVolume;

        [Header("audios")]
        public AudioClip GameTheme;
        public AudioClip BossTheme;
        public AudioClip ReflectSound;
        public AudioClip PlayerHit;
        public AudioClip EnemyDie;
        public AudioClip BigExplosion;
        public AudioClip BossShoot;
        public AudioClip EnemyShoot;
        public AudioClip BossTeleport;
        public AudioClip Lightning;
        public AudioClip PlayerMove;
        public AudioClip PlayerAttack;
        public AudioClip PlayerAttackCharged;
        public AudioClip PlayerCharged;
        public AudioClip ConfirmUI;

        public float GetSfxVolumeReduceScale() {
            return SfxVolume / 10;
        }
        
        public float GetMusicVolumeReduceScale() {
            return MusicVolume / 10;
        }

        public void UpgradePlayerLife() {
            PlayerMaxLife += 1;
        }

        public void Load(GameSettings saveDataLoaded) {
            HasTutorialFinished = saveDataLoaded.HasTutorialFinished;
            PlayerMaxLife = saveDataLoaded.PlayerMaxLife;
            HasUnlockedStage2 = saveDataLoaded.HasUnlockedStage2;
            HasUnlockedStage3 = saveDataLoaded.HasUnlockedStage3;
            MusicVolume = saveDataLoaded.MusicVolume;
            SfxVolume = saveDataLoaded.SfxVolume;
        }
    }
}
