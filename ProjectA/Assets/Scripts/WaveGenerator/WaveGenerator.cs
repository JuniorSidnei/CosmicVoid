using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using UnityEditor;
using UnityEngine;

namespace ProjectA.Generator {
    
    public class WaveGenerator : MonoBehaviour {

        [Serializable]
        public class ColorToType {
            public string name;
            public Color Color;
            public WaveData.EntityType Type;
        }

        // public string WaveName;
        // public string PathToSave;
        // public Texture2D WaveDataInfo;
        public List<ColorToType> ColorToTypes = new List<ColorToType>();
        
        private List<WaveData.EntityInfo> m_entityInfos = new List<WaveData.EntityInfo>();
        
        [ContextMenu("Generate wave by texture")]
        public void GenerateWaveData(string waveName, string pathToSave, Texture2D waveDataInfo, float initialTimeSpawn) {

            m_entityInfos.Clear();
            
            for (int x = 0; x <= waveDataInfo.width - 1; x++) {
                for (int y = waveDataInfo.height - 1; y >= 0; y--) {
                    GenerateWave(x, y, waveDataInfo);
                }
            }

            var newDataWave = ScriptableObject.CreateInstance<WaveData>();
            newDataWave.EntityInfos = m_entityInfos;
            newDataWave.InitialTimeSpawn = initialTimeSpawn;
            
            AssetDatabase.CreateAsset(newDataWave, pathToSave + waveName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void GenerateWave(int x, int y, Texture2D waveDataInfo) {
            var pixelColor = waveDataInfo.GetPixel(x, y);
            
            if(pixelColor.a == 0) return;

            foreach (var colorToType in ColorToTypes)  {
                if (colorToType.Color.Equals(pixelColor)) {
                    WaveData.EntityInfo newEntityInfo = CreateEntityInfo(y, colorToType.Type);
                    m_entityInfos.Add(newEntityInfo);
                }   
            }
        }

        private WaveData.EntityInfo CreateEntityInfo(int position, WaveData.EntityType type) {
            
            var entityPosition = position switch {
                2 => WaveData.EntityPosition.Up,
                1 => WaveData.EntityPosition.Middle,
                0 => WaveData.EntityPosition.Down,
                _ => WaveData.EntityPosition.Up
            };
            
            return new WaveData.EntityInfo() {
                Position = entityPosition,
                Type = type
            };
        }
    }
}