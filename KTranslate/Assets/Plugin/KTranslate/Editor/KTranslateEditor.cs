using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace KTranslate {  
    public class KTranslateEditor : EditorWindow {
        bool _generator;
        bool _keyMenu;
        bool _keyLanguage;
        bool _confirmSave;
        int _choiceAlertGenerator = -1;
        string _newKey;
        TextAsset _textTranslate;
        SystemLanguage _selectedLanguage = SystemLanguage.English;

        List<string> Keys = new List<string>();
        List<string> Language = new List<string>();
        Dictionary<string, List<string>> _dicTranslate = new Dictionary<string, List<string>>();

        [MenuItem("Tools/KTranslater")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(KTranslateEditor), false, "KTranslate");
        }

        void OnGUI() {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Translation File");
            _textTranslate = (TextAsset)EditorGUILayout.ObjectField(_textTranslate, typeof(TextAsset), true);
            if (_textTranslate == null)
                _generator = GUILayout.Button("Generate Original File");
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Load File")) {
                LoadFile();
                LoadKeys();
                Repaint();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _keyMenu = EditorGUILayout.BeginFoldoutHeaderGroup(_keyMenu, "List keys");
            if (_keyMenu) {
                for (int i = 0; i < Keys.Count; i++) {
                    EditorGUILayout.BeginHorizontal();
                    Keys[i] = EditorGUILayout.TextField(Keys[i]);
                    if (GUILayout.Button("Remove")) {
                        RemoveKey(Keys[i]);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.BeginHorizontal();
                _newKey = EditorGUILayout.TextField(_newKey);
                if (GUILayout.Button("Add")) {
                    AddKeys();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            _keyLanguage = EditorGUILayout.BeginFoldoutHeaderGroup(_keyLanguage, "Language");
            if (_keyLanguage) {
                EditorGUI.BeginChangeCheck();
                _selectedLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Language", _selectedLanguage);

                foreach (var item in _dicTranslate) {
                    if (item.Key.Length > 0) {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(item.Key);
                        item.Value[(int) _selectedLanguage] =
                            EditorGUILayout.TextField(item.Value[(int) _selectedLanguage]);
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.BeginHorizontal();
            _confirmSave = EditorGUILayout.Toggle("Confirm Save", _confirmSave);
            if (GUILayout.Button("Save")) {
                if (_confirmSave) {
                    _confirmSave = false;
                    Save();
                } else {
                    EditorUtility.DisplayDialog("Error", "Please check toggle for confirm data save", "ok");
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void Update() {
            if (_generator) {
                _choiceAlertGenerator = EditorUtility.DisplayDialogComplex("KTranslate new dictionary", 
                    "Are you sure you want to generate a new file,\n\nIf there is already one it will be erased"
                    , "Create / Replace", "Do nothing", "");
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
        }

        #region methode
        void LoadFile() {
            _dicTranslate.Clear();

            if (_textTranslate != null) {
                var fileLine = _textTranslate.text.Split('\n');
                Language.Clear();
                foreach (var VARIABLE in fileLine[0].Split(',')) {
                    Language.Add(KTranslaterUtils.ClearString(VARIABLE));
                }

                for (int target = 1; target < fileLine.Length; target++) {
                    if (fileLine[target].Length > 1) {
                        var item = new List<string>();
                        string[] words = fileLine[target].Split(',');
                        if (words[0] != null && words[0] != "") {
                            for (int i = 1; i < words.Length; i++) {
                                string word = KTranslaterUtils.ClearString(words[i]);
                                item.Add(word);
                            }
                        }
                        _dicTranslate.Add(KTranslaterUtils.ClearString(words[0]), item);
                    }
                }
            }
        }
        void LoadKeys() {
            if (_textTranslate != null) {
                Keys.Clear();
                foreach (string name in _dicTranslate.Keys) {
                    Keys.Add(name);
                }
                Repaint();
            }
        }
        static void GenerateFile() {
            string languageList = "Key";

            for (int i = 0; i < Enum.GetNames(typeof(SystemLanguage)).Length; i++) {
                languageList += "," + Enum.GetNames(typeof(SystemLanguage))[i];
            }
            WriteFile(languageList);
        }
        static void WriteFile(string text, string nameFile = null) {
            if (nameFile == null) {
                nameFile = Application.dataPath + "/" + "TrDictionary.csv";
            }
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                nameFile, false)) {
                file.WriteLine(text);
            }
        }
        void RemoveKey(string key) {
            _dicTranslate.Remove(key);
            LoadKeys();
            Repaint();
        }
        void AddKeys() {
            if (_newKey.Length > 0 && _textTranslate != null) {
                string keyWrite = "$(" + _newKey + ")";
                List<string> keyList = new List<string>();
                for (int i = 0; i < Enum.GetNames(typeof(SystemLanguage)).Length; i++) {
                    keyList.Add("");
                }
                _dicTranslate.Add(keyWrite, keyList);
                _newKey = "";
                LoadKeys();
            }
        }
        void Save() {
            var textFile = "";
            for (int i = 0; i < Language.Count; i++) {
                textFile += "\"" + Language[i] + "\"";
                if (i < Language.Count - 1) 
                    textFile += (",");
            }
            textFile += '\n';
            int t = 0;
            foreach (var VARIABLE in _dicTranslate) {
                textFile += "\"" + VARIABLE.Key + "\"";
                foreach (var VARIABLET in VARIABLE.Value) {
                    if (VARIABLET.Length > 0)
                        textFile += "," + "\"" + VARIABLET + "\"";
                    else
                        textFile += "," + VARIABLET;
                }
                if (t < _dicTranslate.Count)
                    textFile += '\n';
                t++;
            }
            WriteFile(textFile, AssetDatabase.GetAssetPath(_textTranslate));
        }
        #endregion
    }
}