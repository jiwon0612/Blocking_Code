using Core;
using Core.Dependencies;
using Core.ObjectPool.Runtime;
using UnityEngine;

namespace Work.JES.Code.Sounds
{
    [Provide]
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [Inject] private PoolManagerMono poolManager;
        [SerializeField] private PoolingItemSO playerSO;
        
        public SoundPlayer PlaySound(SoundDataSO sound)
        {
            var soundPlayer = poolManager.Pop<SoundPlayer>(playerSO);
                soundPlayer.PlaySound(sound, false);
                    return soundPlayer;
                
        }
    }
}