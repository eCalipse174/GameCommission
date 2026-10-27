using System.Collections.Generic;
using UnityEngine;

// A bed shared by several characters. Each character has its own slot.
// Needs a Collider2D on a layer included in SleepController.bedLayerMask.
[RequireComponent(typeof(Collider2D))]
public class Bed : MonoBehaviour
{
    [SerializeField] private List<BedSlot> slots = new List<BedSlot>();

    public bool TryGetAnchor(SleepController requester, out Transform anchor)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Owner == requester)
            {
                anchor = slots[i].Anchor;
                return anchor != null;
            }
        }

        anchor = null;
        return false;
    }
}