using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/* Each agent will find neighbours using the physics system to check for
 other objects within its local radius; 
 Keeping the collider type generic instead of specifying 'CircleCollider2D' because, 
 although flock agents will use a circle collider, we could also use box colliders if needed (maybe for drones visual)
 */
[RequireComponent(typeof(Collider2D))] 
public class FlockAgent : MonoBehaviour
{
    Collider2D _agentCollider;
    
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
   
    // Access the collider without ever being able to assign to it (after starting it up in Start())
    public Collider2D AgentCollider
    {
        get { return _agentCollider; }
        set => _agentCollider = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        AgentCollider = GetComponent<Collider2D>();
    }

    /*
     * Method to indicate each agents new position to relocate to once
     * we've calculated the movement through the flock behaviours;
     * 'newPosition' is the offset position the agent will be moving to 
     */
    public void Move(Vector2 newPosition)
    {
        //turn agent to direction it will move towards
        //Use transform.forward if code is changed to 3D
        transform.up = newPosition; 
        
        /*
         * move to new position;
         * Time.deltaTime ensures we have constant movement regardless of Framerate;
         * Cast newPosition to Vector3 to avoid errors
         * because transform.position expects Vector 3 and newPosition is a Vector2 object
         */
        transform.position += (Vector3)newPosition * Time.deltaTime; 
    }
    
    
    // method to take in an angle (in degrees) and return direction of the angleInDegrees
    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        angleInDegrees += 180;
        if (!angleIsGlobal)
        {
            //use z rotation because we are in 2D
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), -Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    } 

}
