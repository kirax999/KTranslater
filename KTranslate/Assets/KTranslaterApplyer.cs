using System.Collections.Generic;
using UnityEngine;

namespace KTranslate {
    public class KTranslaterApplyer : MonoBehaviour {
        [SerializeField] TextAsset _textTranslate;
        [SerializeField] private SystemLanguage _language = SystemLanguage.English;

        Dictionary<string, string> _dicTranslate = new Dictionary<string, string>();

        private void Awake() {
            if (PlayerPrefs.HasKey("KtranslaterLanguage")) {
                _language = (SystemLanguage) PlayerPrefs.GetInt("KtranslaterLanguage");
            }
            int numberColumn = GetColumnNumber();
            if (numberColumn != -1) {
                string[] fileLine = _textTranslate.text.Split('\n');

                for (int i = 1; i < fileLine.Length; i++) {
                    string[] line = fileLine[i].Split(',');
                    if (line.Length > 1) {
                        _dicTranslate.Add(KTranslaterUtils.ClearString(line[0]),
                            KTranslaterUtils.ClearString(line[numberColumn]));
                    }
                }
            }
        }
        void ChangeManualLanguage(UnityEngine.SystemLanguage language) {
            PlayerPrefs.SetInt("KtranslaterLanguage", (int)language);
            PlayerPrefs.Save();
        }
        int GetColumnNumber() {
            int target = -1;
            
            string[] fileLine = _textTranslate.text.Split('\n');
            string[] ListLanguage = fileLine[0].Split(',');
            for (int i = 0; i < ListLanguage.Length; i++) {
                if (KTranslaterUtils.ClearString(ListLanguage[i]) == _language.ToString()) {
                    target = i;
                }
            }

            return target;
        }
        string GetString(string key) {
            string result = null;
            
            result = _dicTranslate[key];
            return result;
        }
    }
}
