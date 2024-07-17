using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WaterBuilder))]
public class WaterBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		WaterBuilder script = (WaterBuilder)target;
		if(GUILayout.Button("Build Water"))
		{
			script.BuildWater();
		}
	}
}