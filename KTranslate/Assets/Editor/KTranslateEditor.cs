using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace KTranslate {  
    public class KTranslateEditor : Editor {
        static Button _generator;
        static EnumField _selectedLanguage;

        [MenuItem("Tools/Open KTranslater")]        
        static void Init() {
            // Each editor window contains a root VisualElement object
            VisualElement root = new VisualElement();

            // Import UXML
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/KTranslateEditor.uxml");
            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/KTranslateEditor.uss");
            uxml.CloneTree(root);
            root.styleSheets.Add(uss);

            _generator = root.Q<Button>("generator");
            _selectedLanguage = root.Q<EnumField>("SelectedLanguage");
            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.

            _generator.clickable.clicked += TaptTap;

           // SetLanguageList();
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

        static void SetLanguageList() {
            foreach (var item in (string[])Enum.GetNames(typeof(SystemLanguage))) {
                _selectedLanguage.Add(new Label(item));
            }
        }
        #endregion
    }
}