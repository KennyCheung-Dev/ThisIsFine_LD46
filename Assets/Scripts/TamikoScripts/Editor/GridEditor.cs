using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
	
	public override void OnInspectorGUI() {
		GridManager myTarget = (GridManager)target;
		DrawDefaultInspector();
		if(GUILayout.Button("Generate Grid")) {

			myTarget.GenerateGrid();


		}

		//myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		//EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
	}
}

