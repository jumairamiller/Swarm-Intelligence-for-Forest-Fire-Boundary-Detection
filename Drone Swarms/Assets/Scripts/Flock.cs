using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The class is responsible for
 * 1) populating the flock with agent prefabs and
 * 2) handling the iteratively executing the behaviours on each agents in the flock
 */
public class Flock : MonoBehaviour
{
    //Handling the prefabs and initial setup variable
    public FlockAgent agentPrefab;
    private List<FlockAgent> flock = new List<FlockAgent>();
    
    // where we'll put in the scriptable behaviour that we'll put in; 
    // we will only pass in a single object (composite behaviour) which will have multiple behaviours that are weighted differently
    public FlockBehaviour behaviour;

    [Range(1,300)] //range for slider of number of objects to be put in the scene
    public int agentCount = 150;
    //Flock is populated randomly within a circle;
    //Here we are choosing the size of circle based on the number of agents in the flock
    private const float FlockDensity = 0.08f; //Circle space agents will populate upon start

    //Agents movement variables:
    [Range(1f, 100f)] public float forceFactor = 25f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighbourRadius = 5f;
    [Range(0f, 1f)] public float personalRadius = 0.8f; 
    
    
    /*
     * We're not going to set these variables but we'll use them as a utility variable based on the
     * movement variables (from above);
     * For comparisons, using square values voids more computationally expensive operation of (vectors Magnitude) rooting
     */
    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squarePersonalRadius;

    public float SquarePersonalRadius { get { return squarePersonalRadius; }
    }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squarePersonalRadius = personalRadius * personalRadius;
        
        //instantiate flock
        for (int i = 0; i < agentCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * agentCount * FlockDensity, //position at a random points within a circle
                Quaternion.Euler(Vector3.forward * Random.Range(0f,360f)),
                transform 
                );
            newAgent.name = "Boid " + i;
            
            flock.Add(newAgent);
        }
    }

    /*
     * Update is called once per frame; this is where we iterate through each agent to
     * apply behaviours based on the context of its surrounding
     */
    void Update()
    {
        foreach (FlockAgent agent in flock)
        {
            // List of objects surrounding a given agent in the flock
            List<Transform> surroundings = GetNearbyObjects(agent);
            
            // calculate move based on surrounding objects and 
            Vector2 move = behaviour.CalculateMove(agent, surroundings, this);
            move *= forceFactor;
            
            // if move exceeds the maximum speed then cap it (set it to) at maxSpeed
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    /*
    * Method to compute and return objects which are within a given agents neighbouring radius
    */
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> surroundings = new List<Transform>();
        Collider2D[] surroundingColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        foreach (Collider2D colliderObject in surroundingColliders)
        {
            if (colliderObject != agent.AgentCollider)
            {
                surroundings.Add(colliderObject.transform);
            }
        }

        return surroundings;
    }
}
