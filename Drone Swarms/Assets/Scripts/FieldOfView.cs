using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    
    public LayerMask flameMask; 
    
    public float viewRadius;
    
    [Range(0,360)]
    public float viewAngle;
    
    [HideInInspector]
    public List<Transform> flamesInSight = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindFlamesWithDelay", 0.2f);
    }

    IEnumerator FindFlamesWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleFlames();
        }
    }
    void FindVisibleFlames()
    {
        print("FindVisibleFlames - has been hit");
        //Initially clear to avoid duplicates
        flamesInSight.Clear();
        print("starting count of sighted flames = " + flamesInSight.Count);
        
        //Get array of all colliders of flames within view Radius
        Collider2D[] flamesInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, flameMask);
        print("Collider array found " + flamesInViewRadius.Length + " flames in radius");
        for (int i = 0; i < flamesInViewRadius.Length; i++)
        {
            Transform flame = flamesInViewRadius[i].transform;
            //Check if flame is within line of sight, if it is check for whether other flames are in the way
            Vector2 dirToFlame = (flame.position - transform.position).normalized;
            
            print(Vector2.Angle(transform.right, dirToFlame));
            
            if (Vector2.Angle(transform.right, dirToFlame) < viewAngle/2)
            {
                float distToFlame = Vector2.Distance(transform.position, flame.position);
                
                //If there are no other flames in the way, add it to list of flames in line of sight:
                //TODO: if distance is not too close then track; otherwise halt if distance is x (i.e. very close to flame)
                //if (!Physics2D.Raycast(transform.position, dirToFlame, distToFlame, flameMask)) - This didnt work; all identified
                //{                                                                                 flames were ignored cuz of this line
                flamesInSight.Add(flame);
                    //TODO: here i want to call the trackBehaviour scripts
                //}
            }
        }
        print("ending count of sighted flames = " + flamesInSight.Count);
    }
    
    
    // method to take in an angle (in degrees) and return direction of the angleInDegrees
    public Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        //if angle is not a global angle, view angle will be relative to agents rotation on z axis
        if (!angleIsGlobal)
        {
            //use z rotation because we are in 2D
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), -Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    } 
    
}
