using Unity.Cinemachine;
using UnityEngine;

namespace Heavenage.Scripts.CameraControls
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private CinemachineVirtualCameraBase virtualCamera;
        
        public Transform MainCameraTransform => mainCameraTransform;

        public void SetTarget(Transform target)
        {
            virtualCamera.Follow = target;
        }
    }
}