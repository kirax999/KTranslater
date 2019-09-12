using System;
using System.Collections.Generic;
using UnityEngine;

public class pageLanguage {
    public pageLanguage() {
        
    }

    public pageLanguage(UnityEngine.SystemLanguage language, string nameLanguage) {
        this.language = language;
        this.nameLanguage = nameLanguage;
    }

    public UnityEngine.SystemLanguage language;
    public string nameLanguage;
    public Dictionary<String, String> words = new Dictionary<string, string>();
}

public class dictionaryLanguage {
    public List<pageLanguage> pages = new List<pageLanguage>();
}