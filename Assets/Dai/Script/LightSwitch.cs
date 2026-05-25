using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [Header("Gắn 4 đèn của bạn vào đây")]
    public GameObject[] corridorLights;

    // Hàm này chạy khi người chơi click chuột trái vào vật thể (cần có Collider)
    void OnMouseDown()
    {
        // Kiểm tra xem mảng đèn có trống không để tránh lỗi
        if (corridorLights.Length == 0)
        {
            Debug.LogWarning("Chưa có đèn nào được gắn vào script!");
            return;
        }

        // Duyệt qua từng đèn trong mảng
        foreach (GameObject lightObj in corridorLights)
        {
            if (lightObj != null)
            {
                // Đảo ngược trạng thái: đang bật thì tắt, đang tắt thì bật
                lightObj.SetActive(!lightObj.activeSelf);
            }
        }
    }
}
