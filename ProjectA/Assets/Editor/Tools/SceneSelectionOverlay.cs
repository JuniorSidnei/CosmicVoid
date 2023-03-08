using System;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.Scene;

namespace ProjectA {
    
    [Overlay(typeof(SceneView),  "Scene Selection")]
    [Icon(k_icon)]
    public class SceneSelectionOverlay : ToolbarOverlay {

        public const string k_icon = "Assets/Editor/Icons/UnityIcon.png";

        SceneSelectionOverlay() : base(SceneDropdownToggle.k_id) { }
        
        [EditorToolbarElement(k_id, typeof(SceneView))]
        class SceneDropdownToggle : EditorToolbarDropdownToggle, IAccessContainerWindow {

            public const string k_id = "SceneSelectionOverlay/SceneDropdpwnToggle";
            
            public EditorWindow containerWindow { get; set; }

            SceneDropdownToggle() {
                text = "Scenes";
                tooltip = "Select a scene to load";
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>(SceneSelectionOverlay.k_icon);

                dropdownClicked += ShowSceneMenu;
            }

            private void ShowSceneMenu() {
                GenericMenu menu = new GenericMenu();

                Scene currentScene = EditorSceneManager.GetActiveScene();
                
                string[] sceneGuids = AssetDatabase.FindAssets("t:scene", null);

                for (int i = 0; i < sceneGuids.Length; i++) {
                    string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);

                    string name = Path.GetFileNameWithoutExtension(path);

                    if (String.CompareOrdinal(currentScene.name, name) == 0) {
                        menu.AddDisabledItem(new GUIContent(name));
                    } else {
                        menu.AddItem(new GUIContent(name), false, () => OpenScene(currentScene, path));    
                    }
                }
                
                menu.ShowAsContext();
            }

            private void OpenScene(Scene currentScene, string path) {
                if (currentScene.isDirty) {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                        EditorSceneManager.OpenScene(path);
                    }
                } else {
                    EditorSceneManager.OpenScene(path);
                }
            }
        }
    }
}
