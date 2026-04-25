using System;
using UnityEngine;

namespace Work.JES.Code.BuildSystems
{
    public class PreviewTower : MonoBehaviour
    {
        private Material _material;
        private readonly int _canBuildKey = Shader.PropertyToID("_CanBuild");
        private LineRenderer _rangeLineRenderer; // 공격 범위를 그리는 LineRenderer
        private float _radius = 1f;
        public int segments = 128;
        public bool rotateInXZ = true;
        public void InitMesh(Mesh mesh,float radius)
        {
            _radius = radius;
            GetComponent<MeshFilter>().sharedMesh = mesh;
            _material = GetComponent<MeshRenderer>().sharedMaterial;
            _rangeLineRenderer = GetComponent<LineRenderer>();
            DrawCircle();
        }

        
        public void SetMatColor(bool canBuild)
        {
            DrawCircle();
            _material.SetInt(_canBuildKey, canBuild ? 1 : 0);
        }

        


        private void DrawCircle()
        {
            Vector3[] points = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float angle = 2 * Mathf.PI * i / segments;
                float x = Mathf.Cos(angle) * _radius;
                float y = Mathf.Sin(angle) * _radius;

                points[i] = rotateInXZ ? new Vector3(x, 0, y) : new Vector3(x, y, 0);
            }

            _rangeLineRenderer.positionCount = segments;
            _rangeLineRenderer.SetPositions(points);
        }
    }
}