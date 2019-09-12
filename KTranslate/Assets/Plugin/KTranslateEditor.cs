using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace KTranslate {
    [CustomEditor(typeof(KTranslateComponent))]
    public class KTranslateEditor : Editor {
        [MenuItem("Tools/KTranslate")]
        private void OnEnable() {

        }
    }
}
