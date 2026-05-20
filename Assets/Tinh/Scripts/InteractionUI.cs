using UnityEngine;
using TMPro; // Bắt buộc phải có để dùng TextMeshPro

public class InteractionUI : MonoBehaviour
{
    // Dùng CanvasGroup để chống giật lag Camera
    [SerializeField] private CanvasGroup _uiPromptCanvasGroup;

    // Kéo component TextMeshPro (chứa chữ [E] hoặc [F]) vào đây
    [SerializeField] private TextMeshProUGUI _promptText;

    private void Start()
    {
        // Vào game là tàng hình ngay lập tức (không dùng SetActive)
        if (_uiPromptCanvasGroup != null)
        {
            _uiPromptCanvasGroup.alpha = 0f;
        }
    }

    // Hàm này sẽ được gọi từ PlayerEyeController để bơm chữ vào
    public void ShowPrompt(string message)
    {
        if (_promptText != null && _uiPromptCanvasGroup != null)
        {
            _promptText.text = message;
            _uiPromptCanvasGroup.alpha = 1f; // Hiện lên
        }
    }

    // Hàm này gọi khi quay mặt đi chỗ khác
    public void HidePrompt()
    {
        if (_uiPromptCanvasGroup != null)
        {
            _uiPromptCanvasGroup.alpha = 0f; // Ẩn đi
        }
    }
}