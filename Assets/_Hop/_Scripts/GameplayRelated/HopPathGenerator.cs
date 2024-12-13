using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

namespace _Hop._Scripts.GameplayRelated
{
    public class HopPathGenerator : MonoBehaviour
    {
        [SerializeField] private SplineComputer _computer;

        private void Start()
        {
            
        }

        int index = 1;
        float zAdder = 3.5f;
        public void GenerateHopPath(int tileNum, bool isInitialTile)
        {
            int totalPoints = 0;
            if (isInitialTile) totalPoints = tileNum * 2;
            else totalPoints = tileNum;
            for (int i = 1; i <= 250; i++)
            {
                Vector3 point = Vector3.zero;
                if(i % 2 != 0) 
                    point = new Vector3(0, 4, zAdder);
                else point = new Vector3(0, 0, zAdder);
                SplinePoint pt = new SplinePoint(point);
                _computer.SetPoint(index, pt);

                GameObject node = new GameObject("Node_" + index);
                node.transform.position = point;
                node.transform.parent = _computer.transform;

                Node currNode = node.AddComponent<Node>();
                _computer.ConnectNode(currNode, index);

                index++;
                zAdder += 3.5f;
            }
        }
    }
}
