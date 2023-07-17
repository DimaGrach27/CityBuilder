using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.TerrainGrid
{
    public class TerrainGrid : MonoBehaviour
    {
        [SerializeField] private Terrain _terrain;
        
        [Header("[CELL]")]
        [SerializeField] private Material _material;
        [SerializeField] private int _resolution;
        [SerializeField] private int _sizeCell = 1;
        
        void Start()
        {
            DrawGrid();
        }

        public void Clear()
        {
            
        }

        public void DrawGrid()
        {
            TerrainData terrainData = _terrain.terrainData;
            GameObject gridContainer = new GameObject("[GRID CONTAINER]");

            for (int x = 0; x < terrainData.size.x; x += _sizeCell)
            {
                for (int z = 0; z < terrainData.size.z; z += _sizeCell)
                {
                    float y = terrainData.GetHeight(x, z);
                    Vector3 center = new Vector3(x, y, z);

                    CreateMesh(center, gridContainer.transform);
                }
            }
        }

        public void CreateMesh(Vector3 startPoint, Transform container)
        {
            GameObject gm = new GameObject("Cell");
            gm.transform.SetParent(container);

            //Create vertices
            List<Vector3> vertices = new List<Vector3>();
            
            float perStep = (float)_sizeCell / _resolution;
            
            int size = _resolution * _sizeCell;
            int xStart = (int)startPoint.x * _resolution;
            int zStart = (int)startPoint.z * _resolution;
            
            for (int z = zStart; z < size + 1 + zStart; z += _sizeCell)
            {
                for (int x = xStart; x < size + 1 + xStart; x += _sizeCell)
                {
                    float height = _terrain.terrainData.GetHeight(x, z) + 0.1f;
                    Vector3 vertics = new Vector3(x * perStep, height, z * perStep);
                    vertices.Add(vertics);
                }
            }

            //Create triangles
            List<int> triangles = new List<int>();
            for (int row = 0; row < _resolution; row++)
            {
                for (int column = 0; column < _resolution; column++)
                {
                    int i = (row * _resolution) + row + column;
                    
                    //first triangle
                    triangles.Add(i);
                    triangles.Add(i + _resolution + 1);
                    triangles.Add(i + _resolution + 2);
                    
                    //second triangle
                    triangles.Add(i);
                    triangles.Add(i + _resolution + 2);
                    triangles.Add(i + 1);
                }
            }
            
            //Create uv
            List<Vector2> uvs = new List<Vector2>();
            for (int z = 0; z < size + 1; z += _sizeCell)
            {
                for (int x = 0; x < size + 1; x += _sizeCell)
                {
                    Vector2 uv = Vector2.zero;
                    uv += new Vector2((float)x / size, (float)z / size);
                    uvs.Add(uv);
                }
            }

            Mesh mesh = new Mesh
            {
                name = "Cell",
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray(),
                uv = uvs.ToArray()
            };

            mesh.RecalculateNormals();

            MeshRenderer meshRenderer = gm.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = _material;

            MeshFilter meshFilter = gm.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }
    }
}
