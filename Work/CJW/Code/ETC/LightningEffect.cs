using System;
using DG.Tweening;
using UnityEngine;

namespace Projectiles
{
    public class LightningEffect : MonoBehaviour
    {
        [SerializeField] private float duration;
        public LineRenderer lineRenderer;

        private Transform _point1;
        private Transform _point2;

        public void Init()
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }

        private void Update()
        {
            if (lineRenderer.enabled)
            {
                lineRenderer.SetPosition(0, _point1.position);
                lineRenderer.SetPosition(1, _point2.position);
            }
        }

        public void Connection(Transform point1, Transform point2)
        {
            lineRenderer.enabled = true;
            
            _point1 = point1;
            _point2 = point2;
            
            DOVirtual.DelayedCall(duration, () => lineRenderer.enabled = false);
        }
    }
}