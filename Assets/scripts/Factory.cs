using System;
using UnityEngine;

public class Factory {
  public static GameObject CreateCube(){
    GameObject cube = new GameObject();
    MeshRenderer mr = cube.AddComponent<MeshRenderer>();
    MeshFilter mf = cube.AddComponent<MeshFilter>();
    MeshCollider mc = cube.AddComponent<MeshCollider>();
    mf.mesh = GenerateCubeMesh();
    mc.sharedMesh = mf.mesh;
    return cube;    
  }

  // Method to generate a cube mesh
  static Mesh GenerateCubeMesh() {
      Mesh mesh = new Mesh();

      Vector3[] vertices = new Vector3[]
      {
          new Vector3(0, 0, 0),
          new Vector3(1, 0, 0),
          new Vector3(1, 1, 0),
          new Vector3(0, 1, 0),
          new Vector3(0, 1, 1),
          new Vector3(1, 1, 1),
          new Vector3(1, 0, 1),
          new Vector3(0, 0, 1)
      };

      int[] triangles = new int[]
      {
          // Front face
          0, 2, 1,
          0, 3, 2,
          // Top face
          2, 3, 4,
          2, 4, 5,
          // Right face
          1, 2, 5,
          1, 5, 6,
          // Left face
          0, 7, 4,
          0, 4, 3,
          // Bottom face
          6, 5, 4,
          6, 4, 7,
          // Back face
          0, 1, 6,
          0, 6, 7
      };

      mesh.vertices = vertices;
      mesh.triangles = triangles;
      mesh.RecalculateNormals();

      return mesh;
  }
}