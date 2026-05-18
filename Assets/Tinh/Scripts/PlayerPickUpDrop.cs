using UnityEngine;
using System;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _holdPoint; // Điểm tàng hình trước mặt camera để giữ đồ
    [SerializeField] private float _pickDistance = 3f;
    [SerializeField] private LayerMask _pickableLayer;

    private PickableObject _currentlyHeldObject;
    private bool _isLookingAtPickable;

    public event Action<bool> OnPickableHoverChanged;

    private void OnEnable() => _input.InteractEvent += HandleInteraction;
    private void OnDisable() => _input.InteractEvent -= HandleInteraction;

    private void Update()
    {
        // Nếu đang cầm đồ trên tay thì không cần hiện gợi ý "Nhặt" nữa
        if (_currentlyHeldObject != null)
        {
            if (_isLookingAtPickable)
            {
                _isLookingAtPickable = false;
                OnPickableHoverChanged?.Invoke(false);
            }
            return;
        }

        // Bắn tia Raycast liên tục mỗi khung hình để dò tìm đồ vật cho UI
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, _pickDistance, _pickableLayer);

        if (hitSomething && hit.collider.TryGetComponent(out PickableObject _))
        {
            // Nếu vừa mới nhìn vào vật thể (khung hình trước chưa nhìn)
            if (!_isLookingAtPickable)
            {
                _isLookingAtPickable = true;
                OnPickableHoverChanged?.Invoke(true); // Báo cho UI: "Hiện lên đi!"
            }
        }
        else
        {
            // Nếu vừa mới quay mặt đi chỗ khác
            if (_isLookingAtPickable)
            {
                _isLookingAtPickable = false;
                OnPickableHoverChanged?.Invoke(false); // Báo cho UI: "Ẩn đi!"
            }
        }
    }

    private void HandleInteraction()
    {
        // Nếu đang cầm đồ -> Bấm E để THẢ
        if (_currentlyHeldObject != null)
        {
            DropObject();
        }
        // Nếu đang tay không -> Bấm E để NHẶT
        else
        {
            TryPickUpObject();
        }
    }

    private void TryPickUpObject()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _pickDistance, _pickableLayer))
        {
            if (hit.collider.TryGetComponent(out PickableObject pickable))
            {
                _currentlyHeldObject = pickable;
                _currentlyHeldObject.OnPickedUp(_holdPoint);

                _isLookingAtPickable = false;
                OnPickableHoverChanged?.Invoke(false);
            }
        }
    }

    private void DropObject()
    {
        // Senior Tip: Trước khi thả, có thể cho vật thể dịch xuống sàn một chút
        _currentlyHeldObject.OnDropped();
        _currentlyHeldObject = null;
    }
}