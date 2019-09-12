using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTranslate {
    public class KTranslate : MonoBehaviour {
        [SerializeField] private TextAsset _file = null;
        public SystemLanguage _LanguageSelected = SystemLanguage.English;

        private List<string> _line = new List<string>();
        private int _targetCol = -1;
        private Dictionary<string, string> _words = new Dictionary<string, string>();
        void Start() {
            LoadFile();
            SearchColumnLanguage();
            MakeDicoKey();
            Debug.Log(_targetCol);
        }

        // Update is called once per frame
        void Update() {

        }

        private void GenerateNewDico() {
            string languageList = "Key";

            for (int i = 0; i < Enum.GetNames(typeof(SystemLanguage)).Length; i++) {
                languageList += "," + Enum.GetNames(typeof(SystemLanguage))[i];
            }
            WriteFile("TrDictionary.csv", languageList);
        }

        void WriteFile(string nameFile, string text) {
            Debug.Log("write file : " + Application.dataPath + "/" + nameFile);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                Application.dataPath + "/" + nameFile, false)) {
                file.WriteLine(text);
            }
        }

        void LoadFile() {
            if (_file != null) {
                _line.Clear();

                foreach (var VARIABLE in _file.text.Split('\n')) {
                    _line.Add(VARIABLE);
                }
            }
        }

        void SearchColumnLanguage() {
            if (_line.Count > 0) {
                string[] words = _line[0].Split(',');

                for (int i = 0; i < words.Length; i++) {
                    if (words[i] == _LanguageSelected.ToString()) {
                        _targetCol = i;
                    }
                }
            }
        }

        void MakeDicoKey() {
            if (_line.Count > 1 && _targetCol > -1) {
                for (int t = 1; t < _line.Count; t++) {
                    string[] words = _line[t].Split(',');
                    if (words.Length >= _targetCol) {
                        var word = words[_targetCol];
                        if (word[0] == '"') {
                            word = word.Remove(0, 1);
                            word = word.Remove(word.Length - 1, 1);
                        }
                        _words.Add(words[0], word);
                    }
                }
            }
        }
    }
}