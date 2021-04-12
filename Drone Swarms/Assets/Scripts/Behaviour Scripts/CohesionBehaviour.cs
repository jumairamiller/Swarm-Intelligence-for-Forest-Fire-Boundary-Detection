using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Because its a scriptable object, we need a way to create it:
[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    /*
     * find the new position of an agent based on cohesion behaviour
     * (move to the mid-point between all neighbouring agents)
     */ 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        //No need to make any adjustments if there are no neighbouring objects
        if (surroundings.Count == 0)
        {
            return Vector2.zero; // return a vector with no magnitude = no adjustments made
        }
        
        //otherwise get the average of the sum of all the positions of each neighbouring object
        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredSurroundings = (filter == null) ? surroundings : filter.Filter(agent, surroundings); 
        foreach (Transform item in filteredSurroundings)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= surroundings.Count; // this is the global position
        cohesionMove -= (Vector2)agent.transform.position; // offset from agents position
        return cohesionMove;
    }
    
}
