using UnityEngine;

namespace ProjectA {
    
     [CreateAssetMenu(menuName = "ProjectA/Settings", fileName = "GameSettings")]   
     public class GameSettings : ScriptableObject {
          
          public bool HasInitialCutSceneShow;
          public bool HasExplosionActivated;
          public bool HasTutorialFinished;
          
          public void SetTutorialStatus(int hasFinished) {
              PlayerPrefs.SetInt("tutorial_finished", hasFinished);
              PlayerPrefs.Save();
          }

          public void SetStatus() {
              HasTutorialFinished = PlayerPrefs.GetInt("tutorial_finished") != 0;
              HasExplosionActivated = PlayerPrefs.GetInt("explosion_activated") != 0;
              HasInitialCutSceneShow = PlayerPrefs.GetInt("intro_cutscene_showed") != 0;
          }

          public void SaveExplosionStatus() {
              HasExplosionActivated = true;
              PlayerPrefs.SetInt("explosion_activated", 1);
              PlayerPrefs.Save();
          }
          
          public void SaveInitialCutsceneStatus() {
              HasInitialCutSceneShow = true;
              PlayerPrefs.SetInt("intro_cutscene_showed", 1);
              PlayerPrefs.Save();
          }
     }
}
