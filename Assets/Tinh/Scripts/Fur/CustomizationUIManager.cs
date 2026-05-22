using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationUIManager : MonoBehaviour
{
    public static CustomizationUIManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _designTabPanel;
    [SerializeField] private GameObject _colorTabPanel;

    [Header("Dynamic UI Settings")]
    [SerializeField] private GameObject _buttonPrefab; // Kéo file Prefab nút vào đây
    [SerializeField] private Transform _designTabContent; // Nơi chứa nút Thiết kế
    [SerializeField] private Transform _colorTabContent;  // Nơi chứa nút Màu sắc

    [Header("Player Scripts to Disable")]
    [SerializeField] private PlayerMovement _movementScript;
    [SerializeField] private PlayerRotation _rotationScript;

    public bool IsOpen { get; private set; }
    private CustomizableObject _targetObject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (_movementScript == null && _rotationScript == null)
        {
            _movementScript = FindAnyObjectByType<PlayerMovement>();
            _rotationScript = FindAnyObjectByType<PlayerRotation>();
        }
    }

    private void Start()
    {
        _menuPanel.SetActive(false);
    }

    public void OpenMenu(CustomizableObject target)
    {
        IsOpen = true;
        _targetObject = target;
        _menuPanel.SetActive(true);

        // Khóa di chuyển và xoay chuột của người chơi
        if (_movementScript != null) _movementScript.enabled = false;
        if (_rotationScript != null) _rotationScript.enabled = false;

        // Hiện và giải phóng con trỏ chuột để click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Mặc định mở Tab thiết kế trước
        SelectTab(true);

        GenerateDynamicButtons(target);
    }

    public void CloseMenu()
    {
        IsOpen = false;
        _menuPanel.SetActive(false);

        // Bật lại điều khiển cho Player
        if (_movementScript != null) _movementScript.enabled = true;
        if (_rotationScript != null) _rotationScript.enabled = true;

        // Khóa lại con trỏ chuột vào tâm màn hình
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Hàm gắn vào Nút "Thiết Kế" và "Màu Sắc" trên UI
    public void SelectTab(bool isDesignTab)
    {
        _designTabPanel.SetActive(isDesignTab);
        _colorTabPanel.SetActive(!isDesignTab);
    }

    private void GenerateDynamicButtons(CustomizableObject target)
    {
        // 1. Quét dọn sạch sẽ các nút cũ của vật thể trước đó
        foreach (Transform child in _designTabContent) Destroy(child.gameObject);
        foreach (Transform child in _colorTabContent) Destroy(child.gameObject);

        // 2. Tự động sinh Nút Thiết Kế
        GameObject[] designs = target.GetDesigns();
        for (int i = 0; i < designs.Length; i++)
        {
            int index = i; // 💡 GÓC SENIOR: Bắt buộc phải có biến tạm này để tránh lỗi Closure trong vòng lặp

            GameObject newBtn = Instantiate(_buttonPrefab, _designTabContent);
            newBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Mẫu thiết kế {i + 1}";

            // Tự động gán sự kiện khi click
            newBtn.GetComponent<Button>().onClick.AddListener(() => OnDesignButtonClicked(index));
        }

        // 3. Tự động sinh Nút Màu Sắc
        Material[] colors = target.GetColors();
        for (int i = 0; i < colors.Length; i++)
        {
            int index = i; // 💡 GÓC SENIOR

            GameObject newBtn = Instantiate(_buttonPrefab, _colorTabContent);
            newBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Màu sắc {i + 1}";

            newBtn.GetComponent<Button>().onClick.AddListener(() => OnColorButtonClicked(index));
        }
    }

    // Các hàm để Nút con (Button) trong giao diện gọi dữ liệu thay đổi
    public void OnDesignButtonClicked(int index) => _targetObject?.SetDesign(index);
    public void OnColorButtonClicked(int index) => _targetObject?.SetColor(index);
}