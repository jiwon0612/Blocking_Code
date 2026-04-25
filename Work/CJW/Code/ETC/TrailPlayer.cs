using System;
using UnityEngine;

namespace Work.CJW.Code.ETC
{
    public class TrailPlayer : MonoBehaviour
    {
        private TrailRenderer _trail;

        private void Awake()
        {
            _trail = GetComponent<TrailRenderer>();
        }

        public void SetEnable(bool enable)
        {
            if (enable)
            {
                _trail.enabled = true;
            }
            else
            {
                _trail.Clear();
                _trail.enabled = false;
            }
        }
    }
}