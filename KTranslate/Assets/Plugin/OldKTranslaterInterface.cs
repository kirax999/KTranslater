using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json;

public class OldKTranslaterInterface: EditorWindow {


    /*
    static void Search() {
        foreach (GameObject oneTrans in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
            if (oneTrans != null) {
                if (oneTrans.GetComponent<Text>() != null) {
                    string word = oneTrans.GetComponent<Text>().text;
                    if (word.IndexOf("$") == 0) {
                        foreach (var dico in previousDico.pages) {
                            string key = null;
                            dico.words.TryGetValue(word, out key);
                            if (key == null)
                                dico.words.Add(word, "");
                        }
                    }
                }
            }
        }
    }
*/
    void writeFile(string nameFile, string text) {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(nameFile, false)) {
            file.WriteLine(text);
        }
    }
}
