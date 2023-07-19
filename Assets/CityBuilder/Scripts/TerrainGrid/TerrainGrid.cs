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

        private const string CellName = "Cell";
        private const string PointName = "Point";
        private const string GridContainer = "[GRID CONTAINER]";
        
        public void DrawGrid()
        {
            Clear();
            
            TerrainData terrainData = _terrain.terrainData;
            GameObject gridContainer = new GameObject(GridContainer);

            for (int x = 0; x < terrainData.size.x; x += _sizeCell)
            {
                for (int z = 0; z < terrainData.size.z; z += _sizeCell)
                {
                    Vector3 center = new Vector3(x, 0, z);

                    CreateMesh(center, gridContainer.transform);
                }
            }
        }

        public void CreateMesh(Vector3 startPoint, Transform container)
        {
            GameObject gm = new GameObject(CellName);
            gm.transform.SetParent(container);
            gm.transform.position = startPoint;

            //Create vertices
            List<Vector3> vertices = new List<Vector3>();
            
            float perStep = (float)_sizeCell / _resolution;
            
            int size = _resolution * _sizeCell;
            
            for (int z = 0; z < _resolution + 1; z ++)
            {
                for (int x = 0; x < _resolution + 1; x ++)
                {
                    float xValue = startPoint.x + x * perStep;
                    float zValue = startPoint.z + z * perStep;
                    float height = _terrain.SampleHeight(new Vector3(xValue, 0, zValue));
                    height += 0.1f;
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
            for (int z = 0; z < size + 1; z ++)
            {
                for (int x = 0; x < _resolution + 1; x++)
                {
                    Vector2 uv = Vector2.zero;
                    uv += new Vector2((float)x / _resolution, (float)z / _resolution);
                    uvs.Add(uv);
                }
            }

            Mesh mesh = new Mesh
            {
                name = CellName,
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
        
        public void Clear()
        {
            DestroyObject(GridContainer);
            DestroyObject(CellName);
            DestroyObject(PointName);
        }
        
        private void DestroyObject(string objName)
        {
            GameObject obj = GameObject.Find(objName);

            if (obj != null)
            {
                DestroyImmediate(obj);
                DestroyObject(objName);
            }
        }
    }
}
