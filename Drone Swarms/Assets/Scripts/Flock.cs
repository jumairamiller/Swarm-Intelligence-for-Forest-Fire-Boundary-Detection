﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    private float timer;
    private bool continueTimer = true; // flag to ensure we only print at the end of the search
    
    // a single scriptableObject behaviour (composite behaviour) which will have multiple differently weighted behaviours 
    public FlockBehaviour behaviour;

    [Range(1,300)] //range for slider of number of objects to be put in the scene
    public int agentCount = 150;
    
    //Flock is populated randomly within a circle;
    //Here we are choosing the size of circle based on the number of agents in the flock
    private const float FlockDensity = 0.08f; //Circle space agents will populate within upon start

    //Agents movement variables:
    [Range(1f, 100f)] public float forceFactor = 25f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighbourRadius = 5f;
    [Range(0f, 1f)] public float personalRadius = 0.8f;
    [Range(5f, 15f)]public float heatSensorRadius = 5f;
    
    //Formation, True = Circle and False = Line
    public bool CircleFormation = true;
    
    /*
     * Uutility variable based on above movement variables;
     * - square values avoid more computationally expensive operation of square rooting
     *   (to getting a vector's Magnitude) for comparison between vectors
     */
    float squareMaxSpeed;
    float squareNeighbourRadius;
    float squarePersonalRadius;
    public float SquarePersonalRadius { get { return squarePersonalRadius; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //compute and define Utility variables
        timer = 0;
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squarePersonalRadius = personalRadius * personalRadius;

        //initialise and instantiate flock
        float offset = -(agentCount / 2); //centre offset
        for (int i = 0; i < agentCount; i++)
        {
            if (CircleFormation == true)
            {
                FlockAgent newAgent = Instantiate(
                    agentPrefab, //the object we are instantiating
                    Random.insideUnitCircle * FlockDensity, //position at a random points within a circle
                    Quaternion.Euler(Vector3.forward * Random.Range(0f,360f)), //rotation on z axis 
                    transform //the parent of each agent is going to be this flock instance's transform
                );
                newAgent.name = "UAV/drone " + i;
                flock.Add(newAgent);
            }
            else
            {
                Vector3 position = new Vector3(offset + i, 0, 0);
                FlockAgent newAgent = Instantiate(agentPrefab, position, Quaternion.Euler(Vector3.up), transform);
                newAgent.name = "UAV/drone " + i;
                flock.Add(newAgent);
            }
        }
    }

    /*
     * Update is called once per frame; this is where we iterate through each agent to
     * apply behaviours based on the context of its surrounding
     */
    void Update()
    {
        timer += 1 * Time.deltaTime; // increment timer
        if (SystemStoppingCondition())
        {
            if (continueTimer)
            {
                Debug.Log("Search Time: " + timer);
                continueTimer = false;
            }
        }
        
        foreach (FlockAgent agent in flock)
        {
            // List of objects surrounding a given agent in the flock
            List<Transform> surroundings = GetNearbyObjects(agent);
            
            // calculate move based on surrounding objects 
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
     * Method to compute and return objects which are within a given agents neighbouring radius;
     * Unity's Physics System is used to get the neighbours
     */
    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> surroundings = new List<Transform>();
        Collider2D[] surroundingsCollider = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        
        foreach (Collider2D colliderObject in surroundingsCollider)
        {
            if (colliderObject != agent.AgentCollider)
            {
                surroundings.Add(colliderObject.transform);
            }
        }

        return surroundings;
    }

    /**
     * Method to check whether all agents in the flock have found the boundary
     */
    private bool SystemStoppingCondition()
    {
        foreach (FlockAgent agent in flock)
        {
            // return false if any of the agents are still moving
            if (!(agent.callStop))
            {
                return false;
            }
        }
        // returns true only if all agents have callStop == true 
        return true;
    }
    
}
