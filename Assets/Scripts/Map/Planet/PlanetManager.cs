using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlanetManager : MonoBehaviour
{
	public int _resolution = 10;
	private Color _planetColor;
	private ShapeGenerator _shapeGenerator;

	[SerializeField, HideInInspector]
	MeshFilter[] meshFilters;
	TerrainFace[] terrainFaces;

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

	private void Initialize()
	{
		_shapeGenerator = new ShapeGenerator();
		terrainFaces = new TerrainFace[6];
		if (meshFilters.Length == 0) {
			meshFilters = new MeshFilter[6];
		}
		
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
				meshObj.tag = "PlanetMesh";
				meshObj.transform.parent = transform;
				meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
				meshFilters[i] = meshObj.AddComponent<MeshFilter>();
				meshFilters[i].sharedMesh = new Mesh();
			}
			Debug.Log(string.Format("Setting new terrainface with resolution: {0}", _resolution));
			terrainFaces[i] = new TerrainFace(_shapeGenerator, meshFilters[i].sharedMesh, _resolution, directions[i]);
		}
	}

	private void GenerateMesh()
	{
		foreach(TerrainFace face in terrainFaces)
		{
			face.ConstructMesh();
		}
	}

	private void GenerateColors()
	{
		float randR = Random.Range(0f, 1f);
		float randG = Random.Range(0f, 1f);
		float randB = Random.Range(0f, 1f);
		foreach (MeshFilter mesh in meshFilters)
		{
			_planetColor = new Color(randR, randG, randB);
			mesh.GetComponent<MeshRenderer>().sharedMaterial.color = _planetColor;
		}
	}
}
