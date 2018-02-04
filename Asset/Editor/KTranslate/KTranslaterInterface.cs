using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using Newtonsoft.Json;

public class KTranslaterInterface: EditorWindow {
    static Vector2 posScrollView = new Vector2();

    static dictionaryLanguage previousDico = null;
    static int selectedSize = 0;

    static string[] varsName;
    static string[] varsValue;

    [MenuItem("Tools/Open KTranslater")]
    static void Init() {
        try {
            StreamReader reader = new StreamReader("Assets/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "-KTranslaterJson.json");
            string dico = reader.ReadToEnd();
            previousDico = JsonConvert.DeserializeObject<dictionaryLanguage>(dico);
            if (previousDico == null)
                throw new System.InvalidOperationException("dictionaryLanguage Is empty");
        } catch {
            previousDico = new dictionaryLanguage();
            foreach (var language in System.Enum.GetNames(typeof(UnityEngine.SystemLanguage))) {
                previousDico.pages.Add(new pageLanguage((UnityEngine.SystemLanguage)System.Enum.Parse(typeof(UnityEngine.SystemLanguage), language), language));
            }
            Search();
        }
        KTranslaterInterface window = (KTranslaterInterface)EditorWindow.GetWindow(typeof(KTranslaterInterface));
        varsName = new string[previousDico.pages[0].words.Count];
        varsValue = new string[previousDico.pages[0].words.Count];
        previousDico.pages[0].words.Keys.CopyTo(varsName, 0);
        previousDico.pages[0].words.Values.CopyTo(varsValue, 0);
        var obj = Selection.activeObject;
    }

    void OnGUI() {
        EditorGUILayout.BeginVertical();
        posScrollView = EditorGUILayout.BeginScrollView(posScrollView);
        if (GUILayout.Button("Search"))
            Search();
        if (GUILayout.Button("Generate JSON"))
            generateJson();

        EditorGUI.BeginChangeCheck();
        selectedSize = EditorGUILayout.IntPopup(selectedSize, System.Enum.GetNames(typeof(UnityEngine.SystemLanguage)), null);
        if (EditorGUI.EndChangeCheck()) {
            var page = previousDico.pages[selectedSize];
            assingValueInArray();
            Debug.Log("Language : " + page.nameLanguage);
        }
        EditorGUI.BeginChangeCheck();
        if (varsName != null) 
            for (int i = 0; i < varsName.Length; i++) {
                varsValue[i] = EditorGUILayout.TextField(varsName[i], varsValue[i]);
            }
        if (EditorGUI.EndChangeCheck()) {
            saveVarsValue();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

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

    void saveVarsValue() {
        for (int i = 0; i < varsName.Length; i++) {
            var tmp = previousDico.pages[selectedSize];
            tmp.words[varsName[i]] = varsValue[i];
        }
    }

    void assingValueInArray() {
        for (int i = 0; i < varsName.Length; i++) {
            var tmp = previousDico.pages[selectedSize];
            varsValue[i] = tmp.words[varsName[i]];
        }
    }

    void generateJson() {
        try {
            var ligne = JsonConvert.SerializeObject(previousDico);
            writeFile("Assets/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "-KTranslaterJson.json", ligne);
        }
        catch { }
    }

    void writeFile(string nameFile, string text) {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(nameFile, false)) {
            file.WriteLine(text);
        }
    }
}
