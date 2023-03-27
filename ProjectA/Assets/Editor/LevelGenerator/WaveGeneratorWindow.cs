using System.Collections.Generic;
using System.Linq;
using ProjectA.Data.Wave;
using ProjectA.Generator;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ProjectA.Editor {

    public class WaveGeneratorWindow : EditorWindow {

        private string m_waveName = "";
        private string m_pathToSave = "";
        private Texture2D m_waveDataInfo;
        private float m_waveDataInitialSpawn;
        private WaveGenerator m_waveGenerator;
        private SerializedObject m_waveGeneratorSO;
        private SerializedProperty CurrentProperty;
        private Vector2 m_scrollPos = Vector2.zero;
        private string m_selectedPropertyPath;
        protected SerializedProperty SelectedProperty;
        private static WaveGeneratorWindow window;


        [MenuItem("Wave Generator/Open Editor")]
        public static void Open() {
            window = GetWindow<WaveGeneratorWindow>("Wave Generator Window");
            window.maxSize = new Vector2(600, 400);
            window.minSize = window.maxSize;
        }

        private void OnGUI() {

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Height(400));
            m_waveGenerator = FindObjectOfType<WaveGenerator>();
            m_waveGeneratorSO = new SerializedObject(m_waveGenerator);
            
            CurrentProperty = m_waveGeneratorSO.FindProperty("ColorToTypes");
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            DrawSideBar(CurrentProperty);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            
            if (SelectedProperty != null) {
                DrawProperties(SelectedProperty, true);
            }
            else {
                EditorGUILayout.LabelField("Select an item from the list!"); 
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            Apply();
            
            EditorGUILayout.Space(20);
            
            EditorGUILayout.BeginHorizontal();
            var titleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            titleStyle.fontStyle = FontStyle.BoldAndItalic;
            titleStyle.fontSize = 25;
            titleStyle.normal.textColor = Color.yellow;
            GUILayout.Label("WAVE GENERATOR EDITOR!", titleStyle);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(15);

            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginHorizontal();
            m_waveName = EditorGUILayout.TextField("Wave name", m_waveName);
            var waveNameStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Normal,
                fontSize = 15,
                normal = {
                    textColor = string.IsNullOrWhiteSpace(m_waveName) ? Color.red : Color.green
                }
            };

            var waveNameTextLavel = string.IsNullOrWhiteSpace(m_waveName) ? "Set wave name" : "Ok!";
            GUILayout.Label(waveNameTextLavel, waveNameStyle);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            m_pathToSave = EditorGUILayout.TextField("Path to generate wave", m_pathToSave);
            var pathStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Normal,
                fontSize = 15,
                normal = {
                    textColor = string.IsNullOrWhiteSpace(m_pathToSave) ? Color.red : Color.green
                }
            };

            var textLabel = string.IsNullOrWhiteSpace(m_pathToSave) ? "Set path to save" : "Ok!";
            GUILayout.Label(textLabel, pathStyle);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(5);
            
            m_waveDataInfo = (Texture2D) EditorGUILayout.ObjectField("Texture to generate wave", m_waveDataInfo, typeof (Texture2D), false);
            var textureStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight,
                fontStyle = FontStyle.Normal,
                fontSize = 15,
                normal = {
                    textColor = m_waveDataInfo == null ? Color.red : Color.green
                }
            };

            var textureLabel = m_waveDataInfo == null ? "Set texture to read as data" : "Ok!";
            GUILayout.Label(textureLabel, textureStyle);
            
            EditorGUILayout.Space(15);
            
            m_waveDataInitialSpawn = EditorGUILayout.FloatField("Time to start wave", m_waveDataInitialSpawn);
            var startWaveTime = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Normal,
                fontSize = 15,
                normal = {
                    textColor = m_waveDataInitialSpawn <= 0 ? Color.red : Color.green
                }
            };

            var startWaveTimeLabel = m_waveDataInitialSpawn <= 0 ? "Set initial time to start wave" : "Ok!";
            GUILayout.Label(startWaveTimeLabel, startWaveTime);

            EditorGUILayout.Space(15);

            bool isSetupOk = !string.IsNullOrWhiteSpace(m_waveName) && !string.IsNullOrWhiteSpace(m_pathToSave) && m_waveDataInfo != null && m_waveDataInitialSpawn > 0;

            if (isSetupOk) {
                if (GUILayout.Button("Generate Wave")) {
                    m_waveGenerator.GenerateWaveData(m_waveName, m_pathToSave, m_waveDataInfo, m_waveDataInitialSpawn);
                }    
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        
        protected void DrawProperties(SerializedProperty prop, bool drawChildren) {
            
            var lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop) {
                
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic) {

                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded) {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else {
                    if(!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath)) { continue; }

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }
        
        protected void DrawSideBar(SerializedProperty prop) {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Edit List");
            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Add New Item")) {
                prop.arraySize++;
            }

            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Remove Item")) {
                prop.arraySize--;
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Items List");
            EditorGUILayout.EndHorizontal();
            
            foreach (SerializedProperty p in prop) {
                
                var path = p.propertyPath;
                var pos = int.Parse(path.Split('[').LastOrDefault().TrimEnd(']'));
                
                if (GUILayout.Button(ObjectNames.NicifyVariableName(p.displayName)) ) {
                    m_selectedPropertyPath = p.propertyPath;
                }
            }

            if (!string.IsNullOrEmpty(m_selectedPropertyPath)) {
                SelectedProperty = m_waveGeneratorSO.FindProperty(m_selectedPropertyPath);
            }
        }

        protected void Apply() {
            m_waveGeneratorSO.ApplyModifiedProperties();
        }
    }
}
