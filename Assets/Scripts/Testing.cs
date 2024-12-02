using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace GridSystem
{
    public class Testing : MonoBehaviour
    {
        private Grid grid;
        public void Start()
        {
            grid = new Grid(4, 2, 10f, new Vector3(0, -20));
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                grid.SetValue(UtilsClass.GetMouseWorldPosition(), 50);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
            }
        }
    }
}