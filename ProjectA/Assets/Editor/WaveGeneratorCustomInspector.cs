using System.Collections;
using System.Collections.Generic;
using ProjectA.Generator;
using UnityEditor;
using UnityEngine;

namespace ProjectA.Editor {
    
    [CustomEditor(typeof(WaveGenerator))]
    public class WaveGeneratorCustomInspector : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            var waveGenerator = (WaveGenerator) target;

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            // if (GUILayout.Button("Generate wave with texture")) {
            //     waveGenerator.GenerateWaveData();
            // }
            EditorGUILayout.EndHorizontal();
        }
    }
}