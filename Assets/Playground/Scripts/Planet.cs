using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using playground;

namespace playground
{
	public class Planet : MonoBehaviour
	{
		[Range(2,256)]
		public int resolution = 10;

		public ShapeSettings shapeSettings;
		public ColorSettings colorSettings;
		ShapeGenerator shapeGenerator;

		[SerializeField, HideInInspector]
		MeshFilter[] meshFilters;
		TerrainFace[] terrainFaces;

		private void OnValidate()
		{
			GeneratePlanet();
		}

		public void OnShapeSettingsUpdated()
		{
			Initialize();
			GenerateMesh();
		}

		public void OnColorSettingsUpdated()
		{
			Initialize();
			GenerateColors();
		}

		void Initialize()
		{
			shapeGenerator = new ShapeGenerator(shapeSettings);

			if (meshFilters == null || meshFilters.Length == 0)
			{
				meshFilters = new MeshFilter[6];
			}
			terrainFaces = new TerrainFace[6];

			// save all 6 base directions of the cube to an array
			Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

			for (int i = 0; i < 6; i++)
			{
				if (meshFilters[i] == null) {
					// create a new gameobject to hold meshfilter
					// add a shared material to new gameobject
					// add a new mesh filter component to the new gameobject
					// save to array at this index
					GameObject meshObj = new GameObject("mesh");
					meshObj.transform.parent = transform;
					meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
					meshFilters[i] = meshObj.AddComponent<MeshFilter>();
					meshFilters[i].sharedMesh = new Mesh();
				}

				terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
			}
		}

		public void GeneratePlanet()
		{
			Initialize();
			GenerateMesh();
			GenerateColors();
		}

		void GenerateMesh()
		{
			foreach(TerrainFace face in terrainFaces)
			{
				face.ConstructMesh();
			}
		}

		void GenerateColors()
		{
			foreach (MeshFilter mesh in meshFilters)
			{
				mesh.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
			}
		}
	}
}