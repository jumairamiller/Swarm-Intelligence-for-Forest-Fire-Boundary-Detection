using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(FlockAgent))]
public class FieldOfViewEditor : Editor
{
    /**
    private FieldOfView fow;

    private void OnEnable()
    {
        fow = (FieldOfView)target;
    }
    */

    void OnSceneGUI()
    {
        FlockAgent fow = (FlockAgent) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector2.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirectionFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirectionFromAngle(fow.viewAngle / 2, false);
        Handles.DrawLine ((fow.transform.position), (fow.transform.position + viewAngleA * fow.viewRadius));
        Handles.DrawLine((fow.transform.position), (fow.transform.position + viewAngleB * fow.viewRadius));
        //START FROM 14:15 FROM VID AT https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague
    }
}
