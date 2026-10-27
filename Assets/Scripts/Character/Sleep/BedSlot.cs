using System;
using UnityEngine;

// One sleeping position on a shared bed, bound to a specific character.
[Serializable]
public class BedSlot
{
    [SerializeField] private SleepController owner;
    [SerializeField] private Transform anchor;

    public SleepController Owner => owner;
    public Transform Anchor => anchor;
}