using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Apply script to GUI TEXT ONLY, setup the texts in script component
public class UITextHandler : MonoBehaviour
{
    [SerializeField]
    string[] textDataSet;

    // whether to initialize on start; otherwise it must be initialized by another script
    [SerializeField]
    bool initOnStart = true;

    void OnEnable()
    {
        if (initOnStart)
        {
            InitLanguage();
        }
    }

    // Gets the text for the textID in the current language and assigns to GUI text
    public void InitLanguage()
    {
        // Draw string text from the data set, using language defined in GlobalData
        GetComponent<UnityEngine.UI.Text>().text = textDataSet[GlobalData.language];
    }
}
