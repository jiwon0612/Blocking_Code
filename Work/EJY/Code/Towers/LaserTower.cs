using System;
using Enemies;
using UnityEngine;

namespace Towers
{
    public class LaserTower : Tower
    {
        [SerializeField] private float damage;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private ParticleSystem laserHit;
        [SerializeField] private ParticleSystem laserFire;
        private Enemy _target = null;

        private void Start()
        {
            lineRenderer.SetPosition(0, firePosTrm.position);
            lineRenderer.SetPosition(1, firePosTrm.position);
        }

        protected override void Update()
        {
            if (_target == null)
            {
                _target = FindEnemy()?.GetComponent<Enemy>();
                
                if (laserHit.isPlaying)
                {
                    laserHit.Stop(true);
                    laserHit.Clear(true);
                }

                if (laserFire.isPlaying)
                {
                    laserFire.Stop(true);
                    laserFire.Clear(true);
                }
            }
            else
            {
                Vector3 dir = _target.transform.position - firePosTrm.position;
                RaycastHit hit;

                Physics.Raycast(firePosTrm.position, dir, out hit, attackDistance, whatIsTarget);

                laserHit.transform.position = hit.point;
                laserFire.transform.LookAt(dir);
                
                lineRenderer.SetPosition(1, _target.transform.position);
                
                if (!laserFire.isPlaying)
                {
                    laserFire.Play(true);
                }

                if (!laserHit.isPlaying)
                {
                    laserHit.Stop(true);
                    laserHit.Clear(true);
                    laserHit.Play(true);
                }

                _timer += Time.deltaTime;
                if (_timer >= attackDelay)
                {
                    _timer = 0;
                    
                    if(_target.TakeDamage(damage))
                    {
                        _target = null;
                        lineRenderer.SetPosition(1, firePosTrm.position);
                    }
                }
            }
        }

        public override void Shooting(Transform target)
        {
        }

        public override void ResetItem()
        {
            _target = null;
            lineRenderer.SetPosition(1, firePosTrm.position);
            laserHit.Stop(true);
            laserFire.Stop(true);
            laserHit.Clear(true);
            laserFire.Clear(true);
        }
    }
}