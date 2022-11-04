using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using playground;

namespace playground
{
	[CustomEditor(typeof(Planet))]
	public class PlanetEditor : Editor
	{
		Planet planet;
		bool showShapeSettings;
		bool showColorSettings;
		Editor shapeEditor;
		Editor colorEditor;

		public override void OnInspectorGUI()
		{
			using (var check = new EditorGUI.ChangeCheckScope()) {
				base.OnInspectorGUI();
				if (check.changed) {
					planet.GeneratePlanet();
				}
			}

			if (GUILayout.Button("Generate Planet")) {
				planet.GeneratePlanet();
			}
			DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref showShapeSettings, ref shapeEditor);
			DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref showColorSettings, ref colorEditor);
		}

		void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool showSettings, ref Editor editor)
		{
			if (settings != null) {
				showSettings = EditorGUILayout.InspectorTitlebar(showSettings, settings);
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					if (showSettings) {
						CreateCachedEditor(settings, null, ref editor);
						editor.OnInspectorGUI();

						if (check.changed) {
							if (onSettingsUpdated != null) {
								onSettingsUpdated();
							}
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			planet = (Planet)target;
		}
	}
}
