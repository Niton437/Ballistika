using UnityEngine;
using UnityEngine.InputSystem;

namespace Cannon
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float rotationSpeed = 5f;

        private InputSystem inputActions;
        private BallisticController ballisticController;

        private void Awake()
        {
            inputActions = new InputSystem();
            inputActions.Enable();

            ballisticController = GetComponentInChildren<BallisticController>();
            if (ballisticController == null)
            {
                Debug.LogError("BallisticController not found in children.");
            }

            inputActions.Control.Shoot.started += context => ballisticController.Fire();
        }

        private void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Control.Shoot.started -= context => ballisticController.Fire();
                inputActions.Disable();
                inputActions.Dispose();
            }
        }

        private void Update()
        {
            ProcessMovement();
            ProcessRotation();
        }

        private void ProcessMovement()
        {
            Vector2 moveInput = inputActions.Control.WASD.ReadValue<Vector2>();
            Vector3 moveDirection = (transform.right * moveInput.x) + (transform.forward * moveInput.y);
            transform.position += moveDirection * (movementSpeed * Time.deltaTime);
        }

        private void ProcessRotation()
        {
            float rotateInput = inputActions.Control.Rotate.ReadValue<float>();
            transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
        }
    }
}