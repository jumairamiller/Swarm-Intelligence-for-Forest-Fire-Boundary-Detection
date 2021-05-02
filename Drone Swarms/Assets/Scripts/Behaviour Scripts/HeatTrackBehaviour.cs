using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Because its a scriptable object, we need a way to create it:
[CreateAssetMenu(menuName = "Flock/Behaviour/Heat Track")]
public class HeatTrackBehaviour : FilteredFlockBehaviour
{
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        List<Transform> surroundingFlames = new List<Transform>();
        //identify objects which are within a given agents heat sensor's radius
        Collider2D[] heatSensorColliders = Physics2D.OverlapCircleAll(agent.transform.position, flock.heatSensorRadius);
        
        foreach (Collider2D colliderObject in heatSensorColliders)
        {
            if (colliderObject != agent.AgentCollider)
            {
                surroundingFlames.Add(colliderObject.transform);
            }
        }
        
        //maintain current direction if there are no neighbouring objects
        if (surroundingFlames.Count == 0)
        {
            return agent.transform.up; 
        }
        
        //Otherwise get the sum of distances to each flame object within the heat sensor's range distance
        Vector2 heatTrackMove = Vector2.zero;
        float totalAgentHeat = 0;
        
        //Debug.Log("Code has been reached");
        List<Transform> filteredSurroundings = (filter == null) ? surroundingFlames : filter.Filter(agent, surroundingFlames);
        //Debug.Log(filteredSurroundings.ToString());
        
        foreach (Transform flame in filteredSurroundings)
        {   
            Vector2 dirToFlame = (flame.position - agent.transform.position).normalized;
            float distToFlame = Vector2.Distance(agent.transform.position, flame.position);
            
            // if agent is too close to flame, set flag for stopping condition to 'true'
            if (Mathf.Abs(distToFlame) <= 1f)
            {
                agent.callStop = true;
            }

            // otherwise compute move to seek flame 
            RaycastHit2D pathToFlame = Physics2D.Raycast(agent.transform.position, dirToFlame);
            Debug.DrawRay(agent.transform.position, dirToFlame * distToFlame, Color.red);
            // heat at drone is 300/d^2 where 300 is the average temperature of tree to ignite
            totalAgentHeat += 300.0f/ Mathf.Pow(distToFlame, 2.0f);
            heatTrackMove += ((300.0f / Mathf.Pow(distToFlame, 2.0f)) * dirToFlame);
        }
        
        return heatTrackMove;

    }
    
}
