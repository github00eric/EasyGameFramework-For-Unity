using System;
using UnityEngine;

public class HandleOnDestroy : MonoBehaviour
{
    public void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    public event Action OnDestroyEvent;
}
