using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KTranslaterApplyer : MonoBehaviour {
    public TextAsset File;
	// Use this for initialization
	void Start () {
        try {
            int language = PlayerPrefs.GetInt("KtranslaterLanguage", (int)Application.systemLanguage);
            printValue(language);
        } catch { 
            Debug.Log("********************************************************");
            Debug.Log(File.text);
            Debug.Log("********************************************************");
        }
	}
	
	public class pageLanguage {
		public UnityEngine.SystemLanguage language;
		public string nameLanguage;
		public Dictionary<string, string> words = new Dictionary<string, string>();
	}

	public class dictionaryLanguage {
		public List<pageLanguage> pages = new List<pageLanguage>();
	}

    void changeManualLanguage(UnityEngine.SystemLanguage language) {
        PlayerPrefs.SetInt("KtranslaterLanguage", (int)language);
        PlayerPrefs.Save();
    }

    void printValue(int nameLanguage) {
        string dico = File.text;

        dictionaryLanguage data = JsonConvert.DeserializeObject<dictionaryLanguage>(dico);
        foreach (var page in data.pages) {
            if (page.nameLanguage == ((UnityEngine.SystemLanguage)nameLanguage).ToString()) {
                foreach (GameObject oneTrans in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
                    if (oneTrans != null) {
                        if (oneTrans.GetComponent<Text>() != null)
                            if (oneTrans.GetComponent<Text>().text.IndexOf("$") == 0) {
                                string t = page.words[key: oneTrans.GetComponent<Text>().text];
                                oneTrans.GetComponent<Text>().text = t;
                            }
                    }
                }
            }
        }
    }
}
