using DG.Tweening;
using Enemies;
using UnityEngine;

namespace Work.CJW.Code.SynergySkills
{
    public class GreenSynergySkill : SynergySkill
    {
        [SerializeField] private Transform visual;
        [SerializeField] private float defaultRadius;
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private float debuffValue;
        [SerializeField] private float debuffTime;
        [SerializeField] private int maxHitCount;
        [SerializeField] private float animationTime;

        private Collider[] _result;
        
        private void Awake()
        {
            _result = new Collider[maxHitCount];
            visual.gameObject.SetActive(false);
        }

        public override void UseSkill()
        {
            if (skillTimer < skillCoolTime)
            {
                return;
            }

            skillTimer = 0;

            synergyCount = _synergt.tileCount;
            
            int cnt = Physics.OverlapSphereNonAlloc(transform.position, defaultRadius + synergyCount, _result,
                whatIsTarget.value);
            
            Animation(defaultRadius + synergyCount);

            for (int i = 0; i < cnt; i++)
            {
                if (_result[i].TryGetComponent(out Enemy enemy))
                {
                    enemy.AddBuff(EnemyBuffReason.SlowerTower,-debuffValue,debuffValue);
                }
            }
        }

        public void Animation(float radius)
        {
            visual.gameObject.SetActive(true);
            visual.transform.localScale = new Vector3(0, 0, 1);
            
            visual.transform.DOScale(new Vector3(radius * 2, radius * 2,1), animationTime).OnComplete(() => visual.gameObject.SetActive(false));
        }
    }
}