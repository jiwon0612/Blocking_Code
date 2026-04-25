using Enemies;
using UnityEngine;
using UnityEngine.Events;
using Work.CJW.Code.ETC;

namespace Projectiles
{
    public class Boomerang : Projectile
    {
        [SerializeField] private AnimationCurve moveDirCurve;
        
        private Vector3 _shotDir;
     
        public UnityEvent OnHit;
        
        protected override void Update()
        {
            if (!_isFire) return;
            
            _currentLifeTime += Time.deltaTime;

            if (_currentLifeTime >= lifeTime)
            {
                BulletDead();
            }

            _rigidCompo.linearVelocity = _shotDir * moveDirCurve.Evaluate(_currentLifeTime / lifeTime);
        }

        public override void FireAndInit(Transform target)
        {
            base.FireAndInit(target);
            _shotDir =(target.position - transform.position.normalized) * moveSpeed;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                if (hitEffect != null)
                {
                    EffectPlayer player = poolManager.Pop(hitEffect) as EffectPlayer;
                    player.PlayEffect(transform.position);
                }
                OnHit?.Invoke();
            }
        }
    }
}