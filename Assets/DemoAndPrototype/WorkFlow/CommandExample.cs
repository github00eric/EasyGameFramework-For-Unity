using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using NaughtyAttributes;
using UnityEngine;

public class CommandExample : MonoBehaviour
{
    [Button]
    void ExitGame()
    {
        ICommand exit = new ExitGameCommand();
        exit.Execute();
    }
}
