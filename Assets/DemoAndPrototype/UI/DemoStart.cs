using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using UnityEngine;

public class DemoStart : MonoBehaviour
{
    private IUIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = EgfEntry.GetModule<IUIManager>();
        uiManager.Show("StartPanel");
    }
}
