using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behaviour/Keep Within Radius Boundary")]
public class KeepWithinRadiusBehaviour : FlockBehaviour
{
    public Vector2 centre; //defaulted to (0,0)
    public float searchRadius = 15f; // this can be adjusted in the Inspector Window
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        // see how far away the agent is from the centre
        Vector2 centreOffset = centre - (Vector2)agent.transform.position;
        float offsetRatio = centreOffset.magnitude / searchRadius;
        // if within 90% of radius boundary, continue as normal
        if (offsetRatio < 0.9f)
        {
            return Vector2.zero;
        }
        //Otherwise, if we're less that 10% to or beyond the boundary, push agent within bounds
        return centreOffset * offsetRatio * offsetRatio;
    }
}
