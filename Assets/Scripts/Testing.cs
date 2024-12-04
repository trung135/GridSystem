using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace GridSystem
{
    public class Testing : MonoBehaviour
    {
        private Grid _grid;
        private float mouseMoveTimer;
        private float mouseMoveTimerMax = .01f;

        public void Start()
        {
            _grid = new Grid(66, 40, 5f, new Vector3(-165, -100));

            HeatMapVisual heatMapVisual = new HeatMapVisual(_grid, GetComponent<MeshFilter>());
        }

        public void Update()
        {
            HandleHeatMapMouseMove();

            if (Input.GetMouseButtonDown(1)) {
                Debug.Log(_grid.GetValue(UtilsClass.GetMouseWorldPosition()));
            }
        }
        
        private void HandleHeatMapMouseMove() {
            mouseMoveTimer -= Time.deltaTime;
            if (mouseMoveTimer < 0f) {
                mouseMoveTimer += mouseMoveTimerMax;
                int gridValue = _grid.GetValue(UtilsClass.GetMouseWorldPosition());
                _grid.SetValue(UtilsClass.GetMouseWorldPosition(), gridValue + 1);
            }
        }
    }

    public class HeatMapVisual
    {
        private Grid _grid;
        private Mesh _mesh;

        public HeatMapVisual(Grid grid, MeshFilter meshFilter)
        {
            this._grid = grid;
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;

            UpdateHeatMapVisual();
            _grid.OnGridValueChanged += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(object sender, EventArgs e)
        {
            UpdateHeatMapVisual();
        }

        public void UpdateHeatMapVisual()
        {
            Vector3[] vertices;
            Vector2[] uv;
            int[] triangles;

            MeshUtils.CreateEmptyMeshArrays(_grid.Width * _grid.Height, out vertices, out uv, out triangles);
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    int index = x * _grid.Height + y;
                    Vector3 baseSize = new Vector3(1, 1) * _grid.CellSize;
                    int gridValue = _grid.GetValue(x, y);
                    int maxGridValue = 100;
                    float gridValueNormalized = Mathf.Clamp01((float)gridValue / maxGridValue);
                    Vector2 gridCellUv = new Vector2(gridValueNormalized, 0f);
                    MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, _grid.GetWorldPosition(x, y) + baseSize * 0.5f, 0f, baseSize, gridCellUv, gridCellUv);
                }
            }
            
            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
        }
    }
}