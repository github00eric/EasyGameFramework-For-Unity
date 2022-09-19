using System;
using System.Collections;
using System.Collections.Generic;
using EGF.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BindDataTest : MonoBehaviour
{
    [ShowInInspector]
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

    [Button]
    private void AddNumber()
    {
        number.Value++;
    }
}
