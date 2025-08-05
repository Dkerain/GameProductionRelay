using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Leaf,
    Alice,
}

public class Character : MonoBehaviour, IHittable
{
    public bool HasBeenHit { get; }

    public virtual void OnHit()
    {
        Debug.Log("Character has been hit!");
    }
}