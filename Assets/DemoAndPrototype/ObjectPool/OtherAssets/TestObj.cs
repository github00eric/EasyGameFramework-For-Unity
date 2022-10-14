using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour
{
    private bool landed;
    private float timer;
    private Action destroyAction;
    private Rigidbody myRigidbody;
    
    public float destroyDelay;

    public Rigidbody MyRigidbody => myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void ResetState()
    {
        landed = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!landed) return;

        if (timer > destroyDelay)
        {
            destroyAction?.Invoke();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        landed = true;
    }

    public void SetDestroyAction(Action destroy)
    {
        this.destroyAction = destroy;
    }
}
