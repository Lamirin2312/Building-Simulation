using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private PlayerPickUpDrop _playerPickUp; // Kéo Player vào đây
    [SerializeField] private GameObject _uiPromptPanel; // Kéo cái "InteractPrompt" ở bước 2 vào đây

    private void Awake()
    {
        // Nếu quên chưa kéo Player vào Inspector, tự động đi tìm Player trong Scene
        if (_playerPickUp == null)
        {
            _playerPickUp = FindAnyObjectByType<PlayerPickUpDrop>();
        }
    }

    private void OnEnable()
    {
        if (_playerPickUp != null)
            _playerPickUp.OnPickableHoverChanged += ToggleUI;
    }

    private void OnDisable()
    {
        if (_playerPickUp != null)
            _playerPickUp.OnPickableHoverChanged -= ToggleUI;
    }

    private void Start()
    {
        // Vào game là phải ẩn cái phím E này đi, khi nào nhìn vào đồ mới hiện
        if (_uiPromptPanel != null) _uiPromptPanel.SetActive(false);
    }

    private void ToggleUI(bool shouldShow)
    {
        if (_uiPromptPanel != null)
        {
            _uiPromptPanel.SetActive(shouldShow);
        }
    }
}