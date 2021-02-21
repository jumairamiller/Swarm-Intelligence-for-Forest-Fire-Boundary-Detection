using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurroundingsFilter : ScriptableObject
{
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);
}
