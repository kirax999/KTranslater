using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;

namespace KTranslate {  
    public class KTranslateEditor : EditorWindow {
        bool _generator;
        bool _loadLanguage;
        bool _keyMenu;
        bool _keyLanguage;
        bool _loadKeys;
        bool _removeKey;
        int _choiceAlertGenerator = -1;
        string addKey;
        TextAsset _textTranslate;
        SystemLanguage _selectedLanguage, systemLanguageP;

        List<string> Keys = new List<string>();

        [MenuItem("Tools/KTranslater")]
        // Add menu item named "My Window" to the Window menu
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(KTranslateEditor));
        }

        void OnGUI() {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Translation File");
            _textTranslate = (TextAsset)EditorGUILayout.ObjectField(_textTranslate, typeof(TextAsset), true);
            if (_textTranslate == null)
                _generator = GUILayout.Button("Generate Original File");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _keyMenu = EditorGUILayout.BeginFoldoutHeaderGroup(_keyMenu, "List keys");
            if (_keyMenu) {
                _loadKeys = GUILayout.Button("Load Keys");
                for (int i = 1; i < Keys.Count; i++) {
                    EditorGUILayout.BeginHorizontal();
                    Keys[i] = EditorGUILayout.DelayedTextField(Keys[i]);
                    _generator = GUILayout.Button("Remove");
                    EditorGUILayout.EndHorizontal();
                }
                addKey = EditorGUILayout.DelayedTextField(addKey);
                _generator = GUILayout.Button("Add");
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _keyLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(_keyLanguage, "Language");
            if (_keyLanguage) {
                _selectedLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Language", _selectedLanguage);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void Update() {
            if (_generator) {
                _choiceAlertGenerator = EditorUtility.DisplayDialogComplex("KTranslate new dictionary", 
                    "Are you sure you want to generate a new file,\n\nIf there is already one it will be erased"
                    , "Create / Replace", "Do nothing", "");
            }
            if (_loadLanguage) {

            }
            if (_choiceAlertGenerator > -1) {
                switch (_choiceAlertGenerator) {
                    case 0:
                        GenerateFile();
                        Debug.Log("File Generate");
                        break;
                    default:
                        break;
                }
            }
            if (_loadKeys) {
                loadKeys();
            }
        }

        #region methode
        void loadKeys() {
            if (_textTranslate != null) {
                Keys.Clear();
                List<string> line = new List<string>();
                line.Clear();

                foreach (var VARIABLE in _textTranslate.text.Split('\n')) {
                    line.Add(VARIABLE);
                }
                for (int i = 0; i < line.Count; i++) {
                    Keys.Add(line[i].Split(',')[0]);
                }
                Repaint();
            }
        }
        static void GenerateFile() {
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