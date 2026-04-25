using System;
using UnityEngine;
using UnityEngine.UI;
using Work.JES.Code.SynergySystems;

namespace Work.CJW.Code.SynergySkills
{
    public class SynergyBtnUI : MonoBehaviour
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private SynergySkill redSkill;
        [SerializeField] private SynergySkill greenSkill;
        [SerializeField] private SynergySkill blueSkill;

        private SynergySkill _currentSynergySkill;
        
        public void InitBtn(SunergyType skill, TowerSynergy synergy)
        {
            switch (skill)
            {
                case SunergyType.Red:
                    _currentSynergySkill = redSkill;
                    break;
                case SunergyType.Green:
                    _currentSynergySkill = greenSkill;
                    break;
                case SunergyType.Blue:
                    _currentSynergySkill = blueSkill;
                    break;
            }
            
            _currentSynergySkill.Init(synergy);
        }

        public void UseSkill()
        {
            if (_currentSynergySkill != null)
            {
                _currentSynergySkill.UseSkill();
            }
        }

        private void OnMouseDown()
        {
            if (_currentSynergySkill != null)
            {
                _currentSynergySkill.UseSkill();
            }
        }

        private void Update()
        {
            Camera mainCam = Camera.main;
            transform.rotation = Quaternion.LookRotation(transform.position -mainCam.transform.position);
            
            if (_currentSynergySkill != null)
            {
                skillImage.fillAmount = Mathf.Clamp01(_currentSynergySkill.skillTimer / _currentSynergySkill.skillCoolTime);
            }
        }
    }
}