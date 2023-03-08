using UnityEditor;

namespace ProjectA {
    
    public static class CreateScriptTemplateMenu {

        [MenuItem("Assets/Create/Code/MonoBehavior", priority = 40)]
        public static void CreateMonoBehavior() {
            string templatePath = "Assets/Editor/Templates/MonoBehaviour.cs.txt";
            
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, " .cs");
        }
        
        [MenuItem("Assets/Create/Code/Scriptable", priority = 41)]
        public static void CreateScriptable() {
            string templatePath = "Assets/Editor/Templates/Scriptable.cs.txt";
            
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, " .cs");
        }
    }
}
