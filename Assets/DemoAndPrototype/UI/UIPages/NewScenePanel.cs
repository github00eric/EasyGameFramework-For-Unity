using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using UnityEngine;

public class NewScenePanel : UiView
{
    public void SwitchScene()
    {
        EgfEntry.GetModule<IAssetLoader>()?.Switch2Scene("UI",null,true);
    }
}
