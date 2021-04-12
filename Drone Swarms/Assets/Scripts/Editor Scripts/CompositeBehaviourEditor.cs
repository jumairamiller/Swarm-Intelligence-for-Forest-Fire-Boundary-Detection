using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor;

[CustomEditor((typeof(CompositeBehaviour)))]
public class CompositeBehaviourEditor : Editor
{
    // Method to specify how the Composite Behaviour's editor will render like on the Inspector tab in Unity
    public override void OnInspectorGUI()
    {
        /*reference to the Composite Behaviour object (in the BehaviourObjects folder):
         * target is the object which the inspector is looking at but it is in the most generic sense so we need to
         * cast it as a Composite behaviour so we can access composite behaviour class' variables etc.
         */
        CompositeBehaviour cb = (CompositeBehaviour) target; // We are casting 'target'; 
        
        // Rect object to manage/keep track of positioning of different fields within the editor
        Rect r = EditorGUILayout.BeginHorizontal(); //this will return the width and initial position of the cursor; but it doesnt give the height
        r.height = EditorGUIUtility.singleLineHeight; // gives the height of all the text in the Inspector window
        
        // if we have no behaviours in the composite behaviour, return warning because the composite behaviour will do nothing in the flock
        if (cb.behaviours == null || cb.behaviours.Length == 0)
        {
            EditorGUILayout.HelpBox("No behaviours in array", MessageType.Warning);
            //Reset cursor for when we DO add behaviours
            EditorGUILayout.EndHorizontal();
            r = EditorGUILayout.BeginHorizontal();
            r.height = EditorGUIUtility.singleLineHeight;
        }
        // otherwise, if we do have behaviours, list them out in the Inspector window
        else
        {
            //insert horizontal padding/margin from the edge of the inspector window from left:
            r.x = 30f;
            //behaviour and corresponding weight fields will be on the same row:
            r.width = EditorGUIUtility.currentViewWidth - 95f;
            EditorGUI.LabelField(r, "Behaviours");
            r.x = EditorGUIUtility.currentViewWidth - 65f;
            r.width = 60f; // this means we have 5 pixels of horizontal padding from the right
            EditorGUI.LabelField(r, "Weights");
            //Padding between field labels (i.e. headers) and inserted behaviours and weights:
            r.y += EditorGUIUtility.singleLineHeight * 1.2f; // vertical of a little bit more than a single text line


            // Initialise flag for changes; initially set to false:
            EditorGUI.BeginChangeCheck();
            //insert each behaviour and its weight on its own line, using a for loop as we'll need the index number to match up:
            for (int i = 0; i < cb.behaviours.Length; i++)
            {
                // set margin from left side of window
                r.x = 5f;
                r.width = 20f;
                EditorGUI.LabelField(r, i.ToString()); // Label in front of each behaviour field

                // Behavior fields
                r.x = 30f;
                r.width = EditorGUIUtility.currentViewWidth - 95f;
                cb.behaviours[i] =
                    (FlockBehaviour) EditorGUI.ObjectField(r, cb.behaviours[i], typeof(FlockBehaviour), false);

                // Weight fields
                r.x = EditorGUIUtility.currentViewWidth - 65f;
                r.width = 60f;
                cb.weights[i] = EditorGUI.FloatField(r, cb.weights[i]);

                // insert line break
                r.y += EditorGUIUtility.singleLineHeight * 1.1f;
            }
            // check for changes to inform Unity to save 
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb); // Inform Unity to save changes in scriptable object
            }
        }

        // reset horizontal 
        EditorGUILayout.EndHorizontal();
            
        // space for button which spans the width of window except 5 pixels margin from sides of window
        r.x = 5f;
        r.width = EditorGUIUtility.currentViewWidth - 10f;
        r.y += EditorGUIUtility.singleLineHeight * 0.5f; 
            
        // Button for adding behaviours 
        if (GUI.Button(r, "Add Behaviour"))
        {
            AddBehaviour(cb);
            EditorUtility.SetDirty(cb);// Inform Unity to save changes in scriptable object
        }

        r.y += EditorGUIUtility.singleLineHeight * 1.5f; 
        // If there are behaviours, then insert button to allow the option of removing behaviours
        if (cb.behaviours != null && cb.behaviours.Length > 0)
        {
            if (GUI.Button(r, "Remove Behaviour"))
            {
                RemoveBehaviour(cb);
                EditorUtility.SetDirty(cb);// Inform Unity to save changes in scriptable object
            }
        }
    }

    // Method to add behaviour to composite behaviour
    void AddBehaviour(CompositeBehaviour cb)
    {
        // get original size of arrays
        int oldCount = (cb.behaviours != null) ? cb.behaviours.Length : 0; // if list isn't empty, use length of list; otherwise set counter to 0
        // create new arrays with additional index:
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        //populate new arrays with original values
        for (int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        // set new weight to a default value of 1 so user can see effect of new behaviour when it's been added
        newWeights[oldCount] = 1f; 
        //assign newWeights and newBehvaiours to composite behaviours variables
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }
    
    //Method to remove behaviour from existing composite behaviour
    void RemoveBehaviour(CompositeBehaviour cb)
    {
        // get original size of arrays
        int oldCount = cb.behaviours.Length;
        // empty arrays if it only contained one element
        if (oldCount == 1)
        {
            cb.behaviours = null;
            cb.weights = null;
            return; 
        }
        // otherwise, create new arrays with one less index and populate them with original values
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }
        //assign newWeights and newBehaviours to composite behaviours variables
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }
    
}
