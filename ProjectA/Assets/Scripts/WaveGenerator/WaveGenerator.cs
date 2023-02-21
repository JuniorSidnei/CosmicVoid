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
        public struct ColorToType {
            public Color Color;
            public WaveData.EntityType Type;
        }

        public string WaveName;
        public Texture2D WaveDataInfo;
        public List<ColorToType> ColorToTypes = new List<ColorToType>();
        
        private List<WaveData.EntityInfo> m_entityInfos = new List<WaveData.EntityInfo>();
        
        [ContextMenu("Generate wave by texture")]
        public void GenerateWaveData() {

            m_entityInfos.Clear();
            
            for (int x = 0; x < WaveDataInfo.width; x++) {
                for (int y = 0; y < WaveDataInfo.height; y++) {
                    GenerateWave(x, y);
                }
            }

            var newDataWave = ScriptableObject.CreateInstance<WaveData>();
            newDataWave.EntityInfos = m_entityInfos;
            
            AssetDatabase.CreateAsset(newDataWave, "Assets/Scriptables/WaveData/" + WaveName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void GenerateWave(int x, int y) {
            var pixelColor = WaveDataInfo.GetPixel(x, y);
            
            if(pixelColor.a == 0) return;

            foreach (var colorToType in ColorToTypes) {
                if (colorToType.Color.Equals(pixelColor)) {
                    WaveData.EntityInfo newEntityInfo = CreateEntityInfo(y, colorToType.Type);
                    m_entityInfos.Add(newEntityInfo);
                }   
            }
        }

        private WaveData.EntityInfo CreateEntityInfo(int position, WaveData.EntityType type) {
            
            var entityPosition = position switch {
                0 => Data.Wave.WaveData.EntityPosition.Up,
                1 => Data.Wave.WaveData.EntityPosition.Middle,
                2 => Data.Wave.WaveData.EntityPosition.Down,
                _ => Data.Wave.WaveData.EntityPosition.Up
            };
            
            return new WaveData.EntityInfo() {
                Position = entityPosition,
                Type = type
            };
        }
    }
}