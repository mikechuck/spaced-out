using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlanetManager))]
public class PlanetManagerEditor : Editor
{
	public PlanetManager planetManager;

	public override void OnInspectorGUI()
	{
		using (var check = new EditorGUI.ChangeCheckScope()) {
			base.OnInspectorGUI();
			if (check.changed) {
				planetManager.GeneratePlanet();
			}
		}

		if (GUILayout.Button("Generate Planet")) {
			planetManager.GeneratePlanet();
		}
	}

	private void OnEnable()
	{
		planetManager = (PlanetManager)target;
	}
}
