using Projectiles;
using UnityEngine;

namespace Towers
{
    public class ShockWaveTower : Tower
    {
        public override void Shooting(Transform target)
        {
            ShockWave shockWave = poolManager.Pop(bulletPoolType) as ShockWave;
            shockWave.InitShockWave(firePosTrm.position);
        }
    }
}