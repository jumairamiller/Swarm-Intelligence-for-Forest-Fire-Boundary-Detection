using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    
    public FlockBehaviour[] behaviours; // flock behaviours to be composited together
    public float[] weights; // weights of each behaviour
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock)
    {
        /*
         * Handle data mismatch;
         * if the two arrays are not of the same length, highlight the object and keep flock in a stationary position
         */
        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Data mismatch in " + name, this); // name of scriptable object, and highlight the object
            return Vector2.zero;
        }
        
        Vector2 move = Vector2.zero;
        
        //iterate through behaviours using for instead of foreach because we need to refer to the same index of data
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (agent.callStop)
            {
                return Vector2.zero;
            }
            
            Vector2 partialMove = behaviours[i].CalculateMove(agent, surroundings, flock) * weights[i];
            
            //confirm the partial move is limited to extent of weight
            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i]) 
                {   
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        
        return move;
    }
}
