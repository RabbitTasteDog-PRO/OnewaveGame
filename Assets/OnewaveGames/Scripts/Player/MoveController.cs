using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class MoveController : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask groundMask;

    private MyInputActions inputActions;
    private InputAction rightClickAction;

    public float moveSpeed = 5f;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
        inputActions = new MyInputActions();

        rightClickAction = inputActions.Player.RightClick;
        rightClickAction.Enable();
        rightClickAction.performed += OnRightClick;
    }

    private void OnDestroy()
    {
        rightClickAction.performed -= OnRightClick;
        rightClickAction.Disable();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            targetPosition = hit.point;
            if (!isMoving)
            {
                isMoving = true;
                MoveToTargetAsync().Forget();
            }
        }
    }

    private async UniTaskVoid MoveToTargetAsync()
    {
        while (true)
        {
            await UniTask.Yield(); // 매 프레임 대기

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            if (distance < 0.05f)
            {
                transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
                isMoving = false;
                return;
            }

            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
    }
}
