using System.IO;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Singletons.Managers {

    public class SaveLoadManager : Singleton<SaveLoadManager> {
        
        private string m_savePath;

        public GameSettings GameSettings;

        private void awake() {
            m_savePath = Application.persistentDataPath + "/.jsonSav";
            LoadGame();
        }

        public void SaveGame() {
            if(!File.Exists(m_savePath)) return;

            string json = JsonUtility.ToJson(GameSettings);
                
            using StreamWriter writer = new StreamWriter(m_savePath);
            writer.Write(json);
        } 

        public void LoadGame() {
            if(!File.Exists(m_savePath)) return;

            using StreamReader reader = new StreamReader(m_savePath);
            string json = reader.ReadToEnd();
                
            var saveDataLoaded = JsonUtility.FromJson<GameSettings>(json);
            GameSettings.Load(saveDataLoaded);
        }
    }

}