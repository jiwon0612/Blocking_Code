using Core.ObjectPool.Runtime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Work.JES.Code.Sounds
{
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolingItemSO PoolingType { get; set; }
        public GameObject GameObject => gameObject;

        public void SetUpPool(Pool pool)
        {
        }

        public void ResetItem()
        {
        }

        [SerializeField] private AudioMixerGroup bgm, sfx;
        [SerializeField] private AudioSource audioSource;

        private bool isPlay = false;


        private bool _isPlayer = false;

        public void PlaySound(SoundDataSO sound, bool isPlayer = false)
        {
            _isPlayer = isPlayer;
            audioSource.outputAudioMixerGroup = sound.outputType == OutputType.BGM ? bgm : sfx;

            audioSource.resource = sound.audioResource;

            audioSource.pitch = 1 + sound.GetRandomPitch();
            audioSource.loop = sound.isLoop;
            audioSource.volume = sound.volume;

            audioSource.dopplerLevel = sound.is3D ? 1f : 0f;

            if (sound.isTime)
            {
                DOVirtual.DelayedCall(sound.playTime, () => { DestroySound(); });
            }

            ReplaySound();
        }

        private void Update()
        {
            if (_isPlayer) return;
            if (isPlay && audioSource.isPlaying == false)
                DestroySound();
        }

        public void StopSound()
        {
            isPlay = false;
            audioSource.Pause();
        }

        public void ReplaySound()
        {
            isPlay = true;
            audioSource.Play();
        }

        private void DestroySound()
        {
            StopSound();
            Destroy(gameObject);
        }
    }
}