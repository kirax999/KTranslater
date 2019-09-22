using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KTranslate {
    public class KTranslate : MonoBehaviour {
        [SerializeField] TextAsset _textTranslate = null;
        [SerializeField] private Dropdown _LanguageChoice = null;
        [SerializeField] private SystemLanguage _language = SystemLanguage.English;

        static Dictionary<string, string> _dicTranslate = new Dictionary<string, string>();
        
        private void Awake() {
            if (_textTranslate != null) {
                _dicTranslate.Clear();
                if (PlayerPrefs.HasKey("KtranslaterLanguage")) {
                    _language = (SystemLanguage)PlayerPrefs.GetInt("KtranslaterLanguage");
                }
                int numberColumn = GetColumnNumber(_language.ToString());
                if (numberColumn != -1) {
                    string[] fileLine = _textTranslate.text.Split('\n');

                    for (int i = 1; i < fileLine.Length; i++) {
                        string[] line = fileLine[i].Split(',');
                        if (line.Length > 1) {
                            _dicTranslate.Add(KTranslateUtils.ClearString(line[0]),
                                KTranslateUtils.ClearString(line[numberColumn]));
                        }
                    }
                }
                if (_LanguageChoice != null) {
                    SetDropDown();
                }
            } else {
                Debug.LogError("Please define dictionary file");
            }
        }
        void ChangeManualLanguage(UnityEngine.SystemLanguage language) {
            PlayerPrefs.SetInt("KtranslaterLanguage", (int)language);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        int GetColumnNumber(string lang) {
            int target = -1;
            
            string[] fileLine = _textTranslate.text.Split('\n');
            string[] ListLanguage = fileLine[0].Split(',');
            for (int i = 0; i < ListLanguage.Length; i++) {
                if (KTranslateUtils.ClearString(ListLanguage[i]) == lang) {
                    target = i;
                }
            }
            return target;
        }
        public static string GetString(string key) {
            string result = "Please define value in dictionary";
            try {
                result = _dicTranslate[key];
            } catch {
                return result;
            }
            return result;
        }

        private void SetDropDown() {
            if (_LanguageChoice != null) {
                List<string> listLanguage = new List<string>();
                
                string[] fileLineL = _textTranslate.text.Split('\n');
                string[] arrayLangL = fileLineL[0].Split(',');
                
                for (int i = 1; i < arrayLangL.Length; i++) {
                    listLanguage.Add(KTranslateUtils.ClearString(arrayLangL[i]));
                }

                foreach (string VARIABLE in listLanguage) {
                    _LanguageChoice.options.Add(new Dropdown.OptionData(VARIABLE));
                }
                _LanguageChoice.value = GetColumnNumber((_language - 1).ToString());
                _LanguageChoice.onValueChanged.AddListener(delegate { DropdownValueChanged(_LanguageChoice); });
            }
        }
        private void DropdownValueChanged(Dropdown change) {
            PlayerPrefs.SetInt("KtranslaterLanguage", change.value);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
