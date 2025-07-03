using UnityEngine;

namespace Heavenage.Scripts.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;
        public Transform cameraTransform;
        
        private CharacterController _controller;
        private Vector3 _velocity;
        private float _gravity = -9.81f;
        private float _yVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (inputDirection.magnitude >= 0.1f)
            {
                // Направление относительно камеры
                var targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

                // Поворот игрока
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Направление движения
                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDir.normalized * (moveSpeed * Time.deltaTime));
            }

            // Гравитация
            if (_controller.isGrounded && _yVelocity < 0)
                _yVelocity = -2f;
            else
                _yVelocity += _gravity * Time.deltaTime;

            _controller.Move(new Vector3(0, _yVelocity, 0) * Time.deltaTime);
        }
    }
}
