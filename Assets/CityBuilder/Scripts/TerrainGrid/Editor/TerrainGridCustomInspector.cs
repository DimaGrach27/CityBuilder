using UnityEditor;
using UnityEngine;

namespace CityBuilder.TerrainGrid.Editor
{
  [CustomEditor(typeof(TerrainGrid))]
  public class TerrainGridCustomInspector : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      TerrainGrid terrainGrid = (TerrainGrid)target;

      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Draw grid"))
      {
        terrainGrid.DrawGrid();
      }
      
      if (GUILayout.Button("Draw cell"))
      {
        terrainGrid.CreateMesh(Vector3.zero, null);
      }
      
      if (GUILayout.Button("CLEAR"))
      {
        terrainGrid.Clear();
      }
      
      GUILayout.EndHorizontal();
    }
  }
}