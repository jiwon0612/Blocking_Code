using Projectiles;
using UnityEngine;

namespace Towers
{
    public class ShotgunTower : Tower
    {
        [SerializeField] private int bulletCnt = 3;
        [SerializeField] private float distance = 2.5f;
        [SerializeField] private float intervalAngle = 15f;
        
        public override void Shooting(Transform target)
        {
            if(target == null) return;
            
            Vector3 dir = target.position - firePosTrm.position;
            float startAngle = -(intervalAngle * (bulletCnt - 1)) / 2f;
            
            for (int i = 0; i < bulletCnt; ++i)
            {
                float angle = startAngle + i * intervalAngle;
                Projectile bullet = poolManager.Pop(bulletPoolType) as Projectile;
                
                Vector3 bulletPos = Quaternion.Euler(0, angle, 0) * (dir.normalized * distance);
                bulletPos.y = firePosTrm.position.y;

                bullet.transform.position = bulletPos;
                
                bullet.FireAndInit(target);
            }
        }
    }
}