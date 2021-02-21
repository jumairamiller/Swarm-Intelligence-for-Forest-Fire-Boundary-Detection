using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Because its a scriptable object, we need a way to create it:
[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesionBehaviour : FilteredFlockBehaviour // only differs from CohesionBehaviour by 3 lines (lines 9,10, and 35)
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f; // how long it should take for an agent to get from its current state to its next state; currently set to half a second
    
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
        
        // smooth the flickering motion of;
        // parameters are: current velocity, target velocity, current velocity (passed in as a reference), time taken to transition between current and target velocity
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime); 
        return cohesionMove;
    }
    
}
