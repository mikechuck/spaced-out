using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
add mesh as prefab
add mesh to network list
spawn mesh instead of creating mesh
*/


public class PlanetManager : MonoBehaviour
{
	private int _resolution = 10;
	private ShapeGenerator _shapeGenerator;
	[SerializeField] GameObject meshPrefab;

	[SerializeField, HideInInspector]
	MeshFilter[] meshFilters;
	TerrainFace[] terrainFaces;

	public NetworkVariable<Color> PlanetColor = new NetworkVariable<Color>();


	public void GeneratePlanet()
	{
		Debug.Log("generating planet");
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
				GameObject meshObj = Instantiate(meshPrefab);
				Mesh meshToCollide = new Mesh();
				meshObj.tag = "PlanetMesh";
				meshObj.transform.parent = transform;
				meshObj.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
				MeshCollider meshCollider = meshObj.GetComponent<MeshCollider>();
				meshCollider.sharedMesh = meshToCollide;
				meshFilters[i] = meshObj.GetComponent<MeshFilter>();
				meshFilters[i].sharedMesh = meshToCollide;
			}
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
		if (NetworkManager.Singleton.IsServer)
		{
			float randR = Random.Range(0f, 1f);
			float randG = Random.Range(0f, 1f);
			float randB = Random.Range(0f, 1f);
			PlanetColor.Value = new Color(randR, randG, randB);
		}

		foreach (MeshFilter mesh in meshFilters)
		{
			mesh.GetComponent<MeshRenderer>().sharedMaterial.color = PlanetColor.Value;
		}
	}
}
