using System;
using UnityEngine;
using Work.JES.Code.SynergySystems;

namespace Work.CJW.Code.SynergySkills
{
    public abstract class SynergySkill : MonoBehaviour
    {
        [Header("Info")] 
        public Sprite icon;
        public string name;
        public string description;
        
        public float skillCoolTime;
        protected int synergyCount;

        public float skillTimer;

        protected TowerSynergy _synergt;

        public void Init(TowerSynergy synergy)
        {
            _synergt = synergy;
        }
        
        public virtual void UseSkill()
        {
            
        }

        private void Update()
        {
            skillTimer += Time.deltaTime;
            
        }
    }
}