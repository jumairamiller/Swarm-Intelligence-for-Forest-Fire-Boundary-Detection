using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Because its a scriptable object, we need a way to create it:
[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    /*
     * find the new direction of an agent based on alignment behaviour
     * (move in the average direction of all neighbouring agents)
     */ 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        //maintain current direction if there are no neighbouring objects
        if (surroundings.Count == 0)
        {
            return agent.transform.up; 
        }
        
        //otherwise get the average of the sum of all the directions of each neighbouring object
        Vector2 alignmentMove = Vector2.zero;
        List<Transform> filteredSurroundings = (filter == null) ? surroundings : filter.Filter(agent, surroundings); 
        foreach (Transform item in filteredSurroundings)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= surroundings.Count; // normalised direction of all neighbouring agents
        
        return alignmentMove;
    }
}
