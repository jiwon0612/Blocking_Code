using Events;
using EventSystems;
using Unity.Cinemachine;
using UnityEngine;

namespace Level
{
    public class CameraHaptic : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource source;
        [SerializeField] private GameEventChannelSO cameraChannel;

        private void Awake()
        {
            cameraChannel.AddListener<ImpulseEvent>(HandleImpulseEvent);
        }

        private void HandleImpulseEvent(ImpulseEvent @event)
        {
            if (source != null)
            {
                source.GenerateImpulse();
            }
            else
            {
                Debug.LogWarning("CinemachineImpulseSource is not assigned.");
            }
        }
    }
}