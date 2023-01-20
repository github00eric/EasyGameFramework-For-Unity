using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using UnityEngine;

public class CommandExample : MonoBehaviour
{
    [ContextMenu("ExitGame")]
    void ExitGame()
    {
        ICommand exit = new ExitGameCommand();
        exit.Execute();
    }
}
