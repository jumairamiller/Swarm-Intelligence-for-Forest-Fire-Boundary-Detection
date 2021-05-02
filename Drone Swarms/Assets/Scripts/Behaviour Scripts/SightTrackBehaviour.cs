using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Because its a scriptable object, we need a way to create it:
[CreateAssetMenu(menuName = "Flock/Behaviour/Sight Track")]
public class SightTrackBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        //maintain current direction if there are no neighbouring objects
        if (surroundings.Count == 0)
        {
            return agent.transform.up;
        }
        
        List<Transform> filteredSurroundings = (filter == null) ? surroundings : filter.Filter(agent, surroundings);
        
        Vector2 sightTrackMove = Vector2.zero; 
        //find flame
        foreach (Transform flame in filteredSurroundings)
        {
            Vector2 dirToFlame = (flame.position - agent.transform.position).normalized;
            float distToFlame = Vector2.Distance(agent.transform.position, flame.position);
        
            if (Mathf.Abs(distToFlame) <= 1f)
            {
                agent.callStop = true;
            }
        
            RaycastHit2D pathToFlame = Physics2D.Raycast(agent.transform.position, dirToFlame);
            Debug.DrawRay(agent.transform.position, dirToFlame * distToFlame, Color.green);
            sightTrackMove += ((300.0f / Mathf.Pow(distToFlame, 2.0f)) * dirToFlame);
        }
        return sightTrackMove;
    }
}
