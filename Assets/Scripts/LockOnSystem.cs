using UnityEngine;
using UnityEngine.InputSystem;

public class LockOnSystem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rayDistance = 15f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerController playerController;

    private Transform _lockOnTarget;

    private PlayerControls _controls;
    private InputAction _lockOnAction;

    private void Awake()
    {
        _controls = new PlayerControls();

        _lockOnAction = _controls.Player.LockOn;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        if (_lockOnAction.triggered)
            TryLockOn();
        
        if (!_lockOnTarget) return;
        
        playerController.IsLockedOn = true;
        
        RotatePlayerTowardsTarget();
    }

    private void TryLockOn()
    {
        // Reset the system
        _lockOnTarget = null;
        playerController.IsLockedOn = false;
        
        // Get the center of the camera
        var screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        
        // Shoot a ray from the camera
        var ray = mainCamera.ScreenPointToRay(screenCenter);
        
        // If the ray hit an enemy in range, lock on him
        if (Physics.Raycast(ray, out var hit, rayDistance, enemyLayer))
        {
            _lockOnTarget = hit.collider.CompareTag("Enemy") ? hit.transform : null;
        }
        else
        {
            _lockOnTarget = null;
        }
    }

    private void RotatePlayerTowardsTarget()
    {
        // Calculate the direction from the enemy and ignore the Y-axis
        var direction = _lockOnTarget.position - playerTransform.position;
        direction.y = 0f;
        
        // Rotate toward the target
        if (direction.sqrMagnitude > 0.001f)
        {
            var targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
