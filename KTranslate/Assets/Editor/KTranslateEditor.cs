using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace KTranslate {  
    public class KTranslateEditor : EditorWindow {
        bool _generator;
        bool _loadLanguage;
        SystemLanguage _selectedLanguage, systemLanguageP;

        [MenuItem("Tools/KTranslater")]
        // Add menu item named "My Window" to the Window menu
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(KTranslateEditor));
        }

        void OnGUI() {
            _generator = GUILayout.Button("Generate Original File");
            _selectedLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Language", _selectedLanguage);
        }

        private void Update() {
            if (_generator) {
                TaptTap();
            }
            if (_loadLanguage) {

            }
        }

        #region methode
        static void TaptTap() {
            string languageList = "Key";

            for (int i = 0; i < Enum.GetNames(typeof(SystemLanguage)).Length; i++) {
                languageList += "," + Enum.GetNames(typeof(SystemLanguage))[i];
            }
            WriteFile("TrDictionary.csv", languageList);
        }

        static void WriteFile(string nameFile, string text) {
            Debug.Log("write file : " + Application.dataPath + "/" + nameFile);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                Application.dataPath + "/" + nameFile, false)) {
                file.WriteLine(text);
            }
        }
        #endregion
    }
}