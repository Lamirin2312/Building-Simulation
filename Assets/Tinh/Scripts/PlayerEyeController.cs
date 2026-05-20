using UnityEngine;

public class PlayerEyeController : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private float _distance = 3f;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private InteractionUI _uiManager; // Kéo Canvas UI vào đây

    private PickableObject _heldObject;
    private GameObject _lastLookedObject;

    private void Awake()
    {
        // Tự động tìm cái UI thực tế đang tồn tại trong Scene
        if (_uiManager == null)
        {
            _uiManager = FindAnyObjectByType<InteractionUI>();

            // Cảnh báo đỏ nếu quên thả UI vào Scene
            if (_uiManager == null)
            {
                Debug.LogError("Player không tìm thấy InteractionUI trong Scene! Hãy kéo Prefab UI Canvas vào Scene nhé.");
            }
        }
    }

    private void OnEnable()
    {
        _input.InteractEvent += OnEKeyPressed;
        _input.CustomizeEvent += OnFKeyPressed;
    }

    private void OnDisable()
    {
        _input.InteractEvent -= OnEKeyPressed;
        _input.CustomizeEvent -= OnFKeyPressed;
    }

    private void Update()
    {
        if (CustomizationUIManager.Instance != null && CustomizationUIManager.Instance.IsOpen)
        {
            _uiManager.HidePrompt();
            return;
        }

        // Nếu đang cầm đồ trên tay thì không cần quét Raycast nữa
        if (_heldObject != null) return;

        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _distance, _layer))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj != _lastLookedObject)
            {
                _lastLookedObject = hitObj;

                // Kiểm tra xem vật thể có những tính năng gì để hiện chữ tương ứng
                bool canPick = hitObj.GetComponent<PickableObject>() != null;
                bool canCustom = hitObj.GetComponent<CustomizableObject>() != null;

                if (canPick && canCustom)
                    _uiManager.ShowPrompt("[E] Nhặt đồ  |  [F] Đổi thiết kế");
                else if (canPick)
                    _uiManager.ShowPrompt("[E] Nhặt đồ");
                else if (canCustom)
                    _uiManager.ShowPrompt("[F] Đổi thiết kế");
            }
        }
        else
        {
            if (_lastLookedObject != null)
            {
                _lastLookedObject = null;
                _uiManager.HidePrompt();
            }
        }
    }

    private void OnEKeyPressed()
    {
        if (CustomizationUIManager.Instance != null && CustomizationUIManager.Instance.IsOpen) return;

        if (_heldObject != null)
        {
            // Thả đồ xuống
            _heldObject.OnDropped();
            _heldObject = null;
            _uiManager.HidePrompt();
        }
        else if (_lastLookedObject != null && _lastLookedObject.TryGetComponent(out PickableObject pickable))
        {
            // Nhặt đồ lên
            _heldObject = pickable;
            _heldObject.OnPickedUp(_holdPoint);
            _uiManager.HidePrompt();
        }
    }

    private void OnFKeyPressed()
    {
        // Chỉ cho phép bấm F khi ĐANG KHÔNG CẦM ĐỒ và đang nhìn vào vật thể tùy biến
        if (_heldObject == null && _lastLookedObject != null && _lastLookedObject.TryGetComponent(out CustomizableObject customizable))
        {
            CustomizationUIManager.Instance.OpenMenu(customizable);
        }
    }
}