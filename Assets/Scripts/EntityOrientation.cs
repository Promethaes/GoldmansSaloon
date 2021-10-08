using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityOrientation : MonoBehaviour
{
    public enum Orientation
    {
        Left,
        Forward,
        Right,
    }

    [HideInInspector] public Orientation orientation { get; protected set; } = Orientation.Forward;
    public UnityEvent OnDetermineOrientation;

    public virtual void DetermineOrientation()
    {
        //call this after the orientation is actually determined
        OnDetermineOrientation.Invoke();
    }
}
