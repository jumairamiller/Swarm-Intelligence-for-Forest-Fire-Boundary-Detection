using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject
{
    //NOTE: We dont need a Start() or Update() method because Scriptable Object dont use them
    
    /* Abstract method which any of the other agent behaviours will implement;
    returns a vector
    - agent is the current agent we are calculating the movement for
    - a List of Transforms called 'surroundings'; its the neighbouring agent and objects
        We're saying 'Transforms' instead of other agents because we are generically referring to 
        any obstacle/boundaries
        - IS THE CONTEXT THE LOCAL SPACE???
    - the flock as a whole 
    */
    public bool callStop; 
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> surroundings, Flock flock);
}
