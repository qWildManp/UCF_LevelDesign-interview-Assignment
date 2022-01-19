using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WalkToObject))]
public class KillerPathfindInspector : Editor
{
    // Start is called before the first frame update    public override void OnInspectorGUI()
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WalkToObject myScript = (WalkToObject)target;
        if(GUILayout.Button("Move To Position"))
        {
            myScript.MoveToTarget();
        }
    }
}
