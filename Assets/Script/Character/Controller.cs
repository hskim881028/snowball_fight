using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public abstract void OnAwake();
    public abstract void OnStart();
    public abstract void Attack();
    public abstract void Move();

    void Awake() {
        OnAwake();
    }

    void Start() {
        OnStart();
    }
}