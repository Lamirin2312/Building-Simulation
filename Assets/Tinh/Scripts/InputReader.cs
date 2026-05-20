using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "InputReader", menuName = "BDS/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    public event Action InteractEvent;

    public event Action CustomizeEvent;

    private PlayerInputActions _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new PlayerInputActions();
            _controls.Player.SetCallbacks(this);
        }
        EnableAllInput();
    }
    private void OnDisable() => DisableAllInput();

    public void EnableAllInput() => _controls?.Player.Enable();
    public void DisableAllInput() => _controls?.Player.Disable();

    public void OnMove(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
    public void OnLook(InputAction.CallbackContext context) => Look = context.ReadValue<Vector2>();

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Kiểm tra nếu người dùng vừa mới nhấn nút xuống (Performed)
        if (context.phase == InputActionPhase.Performed)
        {
            // Bắn tín hiệu ra ngoài: "Có người vừa nhấn nút tương tác kìa!"
            InteractEvent?.Invoke();
        }
    }

    public void OnCustomize(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CustomizeEvent?.Invoke();
        }
    }
}