using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace KTranslate {  
    public class KTranslateEditor : EditorWindow {
        bool _generator;
        bool _loadLanguage;
        bool _keyMenu;
        int _choiceAlertGenerator = -1;
        UnityEngine.Object _textTranslate;
        TextAsset dictionnary;
        SystemLanguage _selectedLanguage, systemLanguageP;

        [MenuItem("Tools/KTranslater")]
        // Add menu item named "My Window" to the Window menu
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(KTranslateEditor));
        }

        void OnGUI() {
            _textTranslate = EditorGUILayout.ObjectField(_textTranslate, typeof(TextAsset), true);
            _keyMenu = EditorGUILayout.BeginFoldoutHeaderGroup(_keyMenu, "List Key");
            if (_keyMenu) {
                _selectedLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Language", _selectedLanguage);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            _generator = GUILayout.Button("Generate Original File");
        }

        private void Update() {
            if (_generator) {
                _choiceAlertGenerator = EditorUtility.DisplayDialogComplex("KTranslate new dictionary", 
                    "Are you sure you want to generate a new file,\n\nIf there is already one it will be erased"
                    , "Create / Replace", "Do nothing", "");
                TaptTap();
            }
            if (_loadLanguage) {

            }
            if (_choiceAlertGenerator > -1) {
                switch (_choiceAlertGenerator) {
                    case 0:
                        TaptTap();
                        break;
                    default:
                        break;
                }
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