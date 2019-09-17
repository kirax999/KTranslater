using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KTranslaterSearch : MonoBehaviour {
    private void Awake() {
    }
    void Start() {
        var targetText = this.GetComponent<Text>();

        targetText.text = KTranslate.KTranslaterApplyer.GetString(targetText.text);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
