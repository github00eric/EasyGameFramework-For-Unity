using System;
using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BindDataTest : MonoBehaviour
{
    // [ShowInInspector]
    public BindingData<int> number;
    
    private void Start()
    {
        number = new BindingData<int>();

        Bind();
    }

    private void Bind()
    {
        number.AddValueChangedEventListener(i =>
        {
            Debug.Log(i);
        });
    }

    [ContextMenu("AddNumber")]
    private void AddNumber()
    {
        number.Value++;
    }
}
