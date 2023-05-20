using UnityEngine;

namespace ProjectA {

    public class GameSettingsData {
        public bool HasTutorialFinished;
        public GameDifficulty GameDifficulty;
        public int PlayerMaxLife;
        public bool HasUnlockedStage2;
        public bool HasUnlockedStage3;
        public bool HasExtraTutorialStepOneShowed;
        public bool HasExtraTutorialStepTwoShowed;
        public bool HasExtraTutorialStepThreeShowed;
        public bool HasExtraTutorialStepFourShowed;   
        public float MusicVolume;
        public float SfxVolume;
        public bool BossOneDefeated;
        public bool BossTwoDefeated;
        public bool BossThreeDefeated; 
    }
    
     [CreateAssetMenu(menuName = "ProjectA/Settings", fileName = "GameSettings")]   
     public class GameSettings : ScriptableObject {
        public bool HasTutorialFinished;
        public GameDifficulty GameDifficulty;
        public int PlayerMaxLife;
        public bool HasUnlockedStage2;
        public bool HasUnlockedStage3;
        public bool HasExtraTutorialStepOneShowed;
        public bool HasExtratutorialStepTwoShowed;
        public bool HasExtraTutorialStepThreeShowed;
        public bool HasExtraTutorialStepFourShowed;   
        public float MusicVolume;
        public float SfxVolume;
        public bool BossOneDefeated;
        public bool BossTwoDefeated;
        public bool BossThreeDefeated;

        [Header("audios")]
        public AudioClip GameTheme;
        public AudioClip BossTheme;
        public AudioClip VictoryTheme;
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

        public void UpgradePlayerLife(int sceneIndex) {
            switch (sceneIndex) {
                case 1:
                    HasUnlockedStage2 = true;
                    if (!BossOneDefeated) {
                        PlayerMaxLife += 1;                
                    }
                    break;
                case 2:
                    HasUnlockedStage3 = true;
                    if (!BossTwoDefeated) {
                        PlayerMaxLife += 1;                
                    }
                    break;
                case 3:
                    if (!BossThreeDefeated) {
                        PlayerMaxLife += 1;                
                    }
                    break;
            }
        }

        public void Load(GameSettingsData saveDataLoaded) {
            HasTutorialFinished = saveDataLoaded.HasTutorialFinished;
            PlayerMaxLife = saveDataLoaded.PlayerMaxLife;
            HasUnlockedStage2 = saveDataLoaded.HasUnlockedStage2;
            HasUnlockedStage3 = saveDataLoaded.HasUnlockedStage3;
            MusicVolume = saveDataLoaded.MusicVolume;
            SfxVolume = saveDataLoaded.SfxVolume;
            HasExtraTutorialStepOneShowed = saveDataLoaded.HasExtraTutorialStepOneShowed;
            HasExtratutorialStepTwoShowed = saveDataLoaded.HasExtraTutorialStepTwoShowed;
            HasExtraTutorialStepThreeShowed = saveDataLoaded.HasExtraTutorialStepThreeShowed;
            HasExtraTutorialStepFourShowed = saveDataLoaded.HasExtraTutorialStepFourShowed;
            BossOneDefeated = saveDataLoaded.BossOneDefeated;
            BossTwoDefeated = saveDataLoaded.BossTwoDefeated;
            BossThreeDefeated = saveDataLoaded.BossThreeDefeated; 
        }

        public void SetInitialValues() {
            HasTutorialFinished = false;
            GameDifficulty = GameDifficulty.EASY;
            PlayerMaxLife = 4;
            HasUnlockedStage2 = false;
            HasUnlockedStage3 = false;
            HasExtraTutorialStepOneShowed = false;
            HasExtratutorialStepTwoShowed = false;
            HasExtraTutorialStepThreeShowed = false;
            HasExtraTutorialStepFourShowed = false;
            MusicVolume = 3f;
            SfxVolume = 2f;
            BossOneDefeated = false;
            BossTwoDefeated = false;
            BossThreeDefeated = false; 
        }
     }
}
