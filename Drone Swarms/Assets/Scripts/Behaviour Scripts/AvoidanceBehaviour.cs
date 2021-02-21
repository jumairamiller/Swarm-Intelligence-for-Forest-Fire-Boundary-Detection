using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Because its a scriptable object, we need a way to create it:
[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    /*
     * find the new position of an agent based on avoidance behaviour
     * (move away from agents in the personal/avoidance radius of the given agent)
     */ 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        //No need to make any adjustments if there are no neighbouring objects
        if (surroundings.Count == 0)
        {
            return Vector2.zero; // return a vector with no magnitude = no adjustments made
        }
        
        //otherwise get the average of the sum of all the positions of each neighbouring object
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0; // counter for number of objects to avoid
        
        List<Transform> filteredSurroundings = (filter == null) ? surroundings : filter.Filter(agent, surroundings); 
        foreach (Transform item in filteredSurroundings)
        {
            // update counter and move if item is within personal radius
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquarePersonalRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position); // sum of offset positions
            }
        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid; // normalise sum of offsets
        }
        return avoidanceMove;
    }
}
