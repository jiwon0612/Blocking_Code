using R3;
using UnityEngine;

namespace UI
{
    public class BossHPPresenter : MonoBehaviour
    {
        [SerializeField] private BossHPView view;
        [SerializeField] private BossHPModelSO model;

        private float maxHP;
        private bool hpBarOn = false;

        private void Start()
        {
            model.BossHP
                .Subscribe(hp => OnBossHPChanged(hp))
                .AddTo(this);

            view.Fade(0);
            hpBarOn = false;
        }

        private void OnBossHPChanged(float hp)
        {
            if (hpBarOn == false)
            {
                maxHP = model.BossHP.Value;
                view.SetBossName(model.BossName);
                hpBarOn = true;
                view.Show();
            }

            view.SetBossHP(hp, maxHP);

            if (model.BossHP.Value <= 0)
            {
                view.Fade(0.3f);
                hpBarOn = false;
            }
        }
    }
}