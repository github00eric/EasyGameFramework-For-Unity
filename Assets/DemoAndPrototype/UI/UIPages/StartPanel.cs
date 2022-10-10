using System;
using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using UnityEngine;

public class StartPanel : UiView
{
    public void SwitchScene()
    {
        EgfEntry.GetModule<IAssetLoader>()?.Switch2Scene("UI_2",null,true);
    }
}
