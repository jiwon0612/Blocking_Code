using System;
using Core;
using UnityEngine;
using Work.JES.Code.Sounds;

namespace Works.Sounds
{
    public class BGMPlayer : MonoSingleton<BGMPlayer>
    {
        [SerializeField] private bool isOn = true;
        [SerializeField] private SoundDataSO soundSo;

        private SoundPlayer _bgmPlayer;
        private void Start()
        {
            if (isOn == false) return;
            _bgmPlayer = SoundManager.Instance.PlaySound(soundSo);
        }

        public void StopBGM()
        {
            _bgmPlayer?.StopSound();
        }

        public void PlayBGM()
        {
            _bgmPlayer?.ReplaySound();
        }
    }
}