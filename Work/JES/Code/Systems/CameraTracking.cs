using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.JES.Code.Systems
{
    public class CameraTracking : MonoBehaviour
    {
        [SerializeField] private InputSO inputSO;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float scrollSpeed = 5f;
        [SerializeField] private float scrollMin = 1f;
        [SerializeField] private float scrollMax = 30f;
        [SerializeField] private CinemachinePositionComposer composer;
        private Rigidbody _rigidbody;

        [SerializeField] private float minX = -10f;
        [SerializeField] private float maxX = 10f;
        [SerializeField] private float minZ = -10f;
        [SerializeField] private float maxZ = 10f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                Debug.LogError("Rigidbody component is missing on the CameraMovement GameObject.");
            }

            inputSO.OnWheelEvent += HandleWheelEvent;
        }

        private void OnDestroy()
        {
            if (inputSO != null)
            {
                inputSO.OnWheelEvent -= HandleWheelEvent;
            }
        }

        private void HandleWheelEvent(float direction)
        {
            composer.CameraDistance += -direction * scrollSpeed;
            composer.CameraDistance = Mathf.Clamp(composer.CameraDistance, scrollMin, scrollMax); // Adjust min and max distance as needed
        }

        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(inputSO.MoveInput.x, 0, inputSO.MoveInput.y);
            movement = movement.normalized * moveSpeed;
            _rigidbody.linearVelocity = movement;

            // Clamp the camera position within the defined bounds
            Vector3 clampedPosition = _rigidbody.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
            _rigidbody.MovePosition(clampedPosition);
        }
    }
}