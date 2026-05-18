using UnityEngine;
public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private Transform _cameraTransform; // Kéo Main Camera vào đây
    [SerializeField] private float _mouseSensitivity = 5f;

    private float _xRotation = 0f;

    private void Update()
    {
        // Lấy dữ liệu từ InputReader (Vector2)
        float mouseX = _input.Look.x * _mouseSensitivity;
        float mouseY = _input.Look.y * _mouseSensitivity;

        // 1. Xoay thân nhân vật theo trục Y (Trái/Phải)
        transform.Rotate(Vector3.up * mouseX);

        // 2. Xoay Camera theo trục X (Lên/Xuống)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); // Giới hạn góc nhìn

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}