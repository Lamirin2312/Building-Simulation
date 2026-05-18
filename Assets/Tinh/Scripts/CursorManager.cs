using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Khóa chuột vào giữa màn hình và ẩn nó đi
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}