using System;
using UnityEngine;

namespace ProjectA {
    
     [CreateAssetMenu(menuName = "ProjectA/Settings", fileName = "GameSettings")]   
     public class GameSettings : ScriptableObject {
          public bool HasTutorialFinished;
          public GameDifficulty GameDifficulty;
          public int PlayerMaxLife;
          public bool HasUnlockedStage2;
          public bool HasUnlockedStage3;

        public float MusicVolume;
        public float SfxVolume;

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
