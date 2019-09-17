using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KTranslate {
    public class KTranslaterApplyer : MonoBehaviour {
        [SerializeField] TextAsset _textTranslate = null;
        [SerializeField] private SystemLanguage _language = SystemLanguage.English;

        static Dictionary<string, string> _dicTranslate = new Dictionary<string, string>();

        private void Awake() {
            if (_textTranslate != null) {
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
            } else {
                Debug.LogError("Please define dictionary file");
            }
        }
        void ChangeManualLanguage(UnityEngine.SystemLanguage language) {
            PlayerPrefs.SetInt("KtranslaterLanguage", (int)language);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        public static string GetString(string key) {
            string result = "Please define value in dictionary";
            
            result = _dicTranslate[key];
            return result;
        }
    }
}
