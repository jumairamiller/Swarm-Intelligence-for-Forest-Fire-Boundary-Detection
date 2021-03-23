using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    /**
    private FieldOfView fow;

    private void OnEnable()
    {
        fow = (FieldOfView)target;
    }
    */

    // Draw handles into scene window for field of view lines
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView) target;
        
        // Draw view radius 
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.viewRadius);
        Vector3 viewAngleA = fov.DirectionFromAngle((fov.viewAngle+180) / 2, false);
        Vector3 viewAngleB = fov.DirectionFromAngle(-(fov.viewAngle+180) / 2, false);
        
        Vector3 viewAngleAtoDraw = fov.DirectionFromAngle((fov.viewAngle + 180) / 2, false);
        Vector3 viewAngleBtoDraw = fov.DirectionFromAngle(-(fov.viewAngle+180) / 2, false);
        
        Handles.DrawLine ((fov.transform.position), (fov.transform.position + viewAngleAtoDraw * fov.viewRadius));
        Handles.DrawLine((fov.transform.position), (fov.transform.position + viewAngleBtoDraw * fov.viewRadius));

        Handles.color = Color.red;
        foreach (Transform visibleFlame in fov.flamesInSight)
        {
         Handles.DrawLine(fov.transform.position, visibleFlame.position);   
        }
        //START FROM 14:15 FROM VID AT https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague
    }
    
}
