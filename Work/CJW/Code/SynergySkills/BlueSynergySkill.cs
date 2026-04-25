using Core.ObjectPool.Runtime;
using DG.Tweening;
using UnityEngine;

namespace Work.CJW.Code.SynergySkills
{
    public class BlueSynergySkill : SynergySkill
    {
        [SerializeField] private float mineDamage;
        [SerializeField] private PoolingItemSO minePoolingItem;
        [SerializeField] private PoolManagerSO poolManager;
        [SerializeField] private float mineUpAnimationValue;
        [SerializeField] private float mineUpAnimationTime;
        [SerializeField] private float mineRadius;
        [SerializeField] private float mineYPoint;

        public override void UseSkill()
        {
            if (skillTimer < skillCoolTime)
            {
                return;
            }

            skillTimer = 0;

            synergyCount = _synergt.tileCount;

            for (int i = 0; i < synergyCount; i++)
            {
                Mine mine = poolManager.Pop(minePoolingItem) as Mine;
                mine.transform.position = transform.position;

                Vector2 point = Random.insideUnitCircle;
                Vector3 minePoint = new Vector3(point.x, mineYPoint,  point.y) * mineRadius;
                mine.transform.DOMoveY(mineUpAnimationValue, mineUpAnimationTime).OnComplete(() =>
                {
                    mine.transform.DOMove(minePoint, mineUpAnimationValue / 2);
                    mine.InitMaie(mineDamage);
                });
            }
        }
    }
}