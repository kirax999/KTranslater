using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KTranslate {
    public class KTranslateSearch : MonoBehaviour
    {
        private void Awake() { }

        void Start() {
            var targetText = this.GetComponent<Text>();

            targetText.text = KTranslate.GetString(targetText.text);
        }

        // Update is called once per frame
        void Update() { }
    }
}