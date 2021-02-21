using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
public class PhysicsLayerFilter : SurroundingsFilter
{
    public LayerMask mask;
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original)
        {
            // if the value of the layer mask is on the same layer as the Transform item, add it to the filterest list
            if (mask == (mask | (1 << item.gameObject.layer))) // left side of the or-statement converts the item's layer into a layermask
            {
                filtered.Add(item);
            }
        }
        return filtered;
    }
}
