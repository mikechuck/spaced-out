using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetManager : MonoBehaviour
{
	private int _resolution = 100;
	private ColorSettings _colorSettings;
	private ShapeGenerator _shapeGenerator;

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

	public void GeneratePlanet()
	{
		Initialize();
		GenerateMesh();
		GenerateColors();
	}

	void Initialize()
	{
		_shapeGenerator = new ShapeGenerator();
		meshFilters = new MeshFilter[6];
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

			terrainFaces[i] = new TerrainFace(_shapeGenerator, meshFilters[i].sharedMesh, _resolution, directions[i]);
		}
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
