using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BridgeBuilder))]
public class BridgeBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		BridgeBuilder script = (BridgeBuilder)target;
		if(GUILayout.Button("Build Bridge"))
		{
			script.BuildBridge();
		}
	}
}