using System.IO;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Singletons.Managers {

    public class SaveLoadManager : Singleton<SaveLoadManager> {
        
        private string m_savePath;

        public GameSettings GameSettings;

        private void Awake() {
            m_savePath = Application.persistentDataPath + "/sav.json";
            LoadGame();
        }

        [ContextMenu("Save game")]
        public void SaveGame() {
            if (string.IsNullOrEmpty(m_savePath)) {
                m_savePath = Application.persistentDataPath + "/sav.json";
            }
            
            string json = JsonUtility.ToJson(GameSettings);
            
            using StreamWriter writer = new StreamWriter(m_savePath);
            writer.Write(json);
            writer.Close();
        } 

        [ContextMenu("Load game")]
        public void LoadGame() {
            if (string.IsNullOrEmpty(m_savePath)) {
                m_savePath = Application.persistentDataPath + "/sav.json";
            }
            
            if (!File.Exists(m_savePath)) {
                GameSettings.SetInitialValues();
                SaveGame();
                return;
            }

            using StreamReader reader = new StreamReader(m_savePath);
            string json = reader.ReadToEnd();
                
            var saveDataLoaded = JsonUtility.FromJson<GameSettingsData>(json);
            GameSettings.Load(saveDataLoaded);
        }
    }

}