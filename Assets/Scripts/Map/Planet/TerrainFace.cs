using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainFace
{
	ShapeGenerator shapeGenerator;
	Mesh mesh;
	int resolution;
	Vector3 localUp;
	Vector3 axisA;
	Vector3 axisB;

	public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
	{
		this.shapeGenerator = shapeGenerator;
		this.mesh = mesh;
		this.resolution = resolution;
		this.localUp = localUp;

		axisA = new Vector3(localUp.y, localUp.z, localUp.x);
		axisB = Vector3.Cross(localUp, axisA);
	}

	public void ConstructMesh()
	{
		Vector3[] vertices = new Vector3[resolution * resolution];
		int squaresPerSide = resolution - 1;
		int squaresPerMesh = squaresPerSide * squaresPerSide;
		int trianglesPerSquare = 2;
		int verticesPerTriangle = 3;
		int[] triangles = new int[squaresPerMesh * trianglesPerSquare * verticesPerTriangle];
		int triIndex = 0;

		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{
				// just another way of incrementing i
				int i = x + y * resolution;

				Vector2 percent = new Vector2(x, y) / (resolution - 1);
				Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
				// normalize the vectors. this will cause the cube to turn into a sphere
				// i.e. all vector have magnitude 1, keeping their direction
				Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
				vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

				// Add vertices to triangles array for all but right/bottom edge vertices
				if (x != resolution - 1 && y != resolution - 1)
				{
					triangles[triIndex] = i;
					triangles[triIndex + 1] = i + resolution + 1;
					triangles[triIndex + 2] = i + resolution;

					triangles[triIndex + 3] = i;
					triangles[triIndex + 4] = i + 1;
					triangles[triIndex + 5] = i + resolution + 1;

					// increase by 6 because we added 6 vertices to array
					triIndex += 6;
				}
			}
		}
		
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}