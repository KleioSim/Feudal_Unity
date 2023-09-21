using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class RightMain : UIView
{
    public UnityEvent<Type, UnityAction<object>> showSub;

    public virtual object[] Parameters { get; set; }
}