using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//global declarations for enums, final vars, etc.
//also contains utility functions
public static class GlobalStuff
{

	//WARNING: TEMPORARY!!!!
	public static int gridSize = 20;

	/* shapes (x is the item origin)
	
    single: x
	
	vert2:  x
		    *
	
	hor2:   x *
	
	l:		x
			* *
	
	r:		x *
			*
	
	rback:  x *
			  *
	
	large:  x *
		    * *
	*/

	public enum Shape { single, vert2, hor2, l, r, rback, large };
	public enum Direction {up, left, down, right};
	
	//finds the first Gameobject with the specified tag within a parent
	public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag) {
		Transform t = parent.transform;
		for (int i = 0; i < t.childCount; i++) {
			if (t.GetChild(i).gameObject.CompareTag(tag)) {
				return t.GetChild(i).gameObject;
			}
		}
		return null;
	}

	//finds ALL Gameobjects with the specified tag within a parent
	public static GameObject[] FindGameObjectsInChildWithTag(GameObject parent, string tag) {
		Transform t = parent.transform;
		List<GameObject> kids = new List<GameObject>();
		for (int i = 0; i < t.childCount; i++) {
			if (t.GetChild(i).gameObject.tag == tag) {
				kids.Add(t.GetChild(i).gameObject);
			}
		}
		return kids.ToArray();
	}

	//get ALL direct child Gameobjects 
	public static GameObject[] FindAllDirectChildren(GameObject parent) {
		Transform t = parent.transform;
		List<GameObject> kids = new List<GameObject>();
		for (int i = 0; i < t.childCount; i++) {
			kids.Add(t.GetChild(i).gameObject);
		}
		return kids.ToArray();
	}

	//simple mapping method
	public static float Map(float s, float a1, float a2, float b1, float b2) {
		return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
	}
	

}
