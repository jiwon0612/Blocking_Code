using UnityEngine;
using Work.JES.Code.Sounds;

namespace Work.Feedbacks
{
    public class SoundFeedback : Feedback
    {
        [SerializeField] private SoundDataSO soundSO;
        
        public override void CreateFeedback()
        {
            SoundManager.Instance.PlaySound(soundSO);
        }

        public override void StopFeedback()
        {
        }
    }
}