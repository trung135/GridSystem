using System;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int X;
        public int Y;
    }
    
    private readonly int _width;
    private readonly int _height;
    private readonly float _cellSize;
    private readonly Vector3 _originPosition;
    private readonly int[,] _gridArray;
    private readonly TextMesh[,] _textMeshArray;

    public int Width { get { return _width; } }
    public int Height { get { return _height; } }
    public float CellSize { get { return _cellSize; } }
    
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPosition = originPosition;
        
        this._gridArray = new int[width, height];
        this._textMeshArray = new TextMesh[width, height];

        for (int x = 0; x < this._gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < this._gridArray.GetLength(1); y++)
            {
                _textMeshArray[x, y] = UtilsClass.CreateWorldText("", null,
                    GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white,
                    TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    private void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
            //_textMeshArray[x, y].text = _gridArray[x, y].ToString();
            if (OnGridValueChanged != null)
                OnGridValueChanged(this, new OnGridValueChangedEventArgs { X = x, Y = y });
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out var x, out var y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return _gridArray[x, y];
        else
            return 0;
    }

    public int GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out var x, out var y);
        return GetValue(x, y);
    }
}