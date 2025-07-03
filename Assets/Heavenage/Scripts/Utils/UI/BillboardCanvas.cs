using UnityEngine;

namespace Heavenage.Scripts.Utils.UI
{
    public class BillboardCanvas : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;

        private void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }
        }

        private void LateUpdate()
        {
            if (!targetCamera) return;

            var cameraForward = targetCamera.transform.forward;
            if (cameraForward.sqrMagnitude < 0.001f)
                return; // zero division avoidance
            
            cameraForward.Normalize();
            
            var rotation = Quaternion.LookRotation(-cameraForward, Vector3.up);
        
            // Fix axis reflection
            rotation *= Quaternion.Euler(0f, 180f, 0f);
        
            transform.rotation = rotation;
        }
    }
}