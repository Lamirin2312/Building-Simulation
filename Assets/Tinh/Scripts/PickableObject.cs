using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void OnPickedUp(Transform holdPoint)
    {
        // 1. Biến vật thể thành con của điểm giữ đồ
        transform.SetParent(holdPoint);

        // 2. Reset vị trí và góc quay về tâm điểm giữ đồ
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // 3. Tắt vật lý và va chạm để không làm lỗi Player
        if (_rb != null) _rb.isKinematic = true;
        if (_collider != null) _collider.enabled = false;
    }

    public void OnDropped()
    {
        // 1. Hủy bỏ quan hệ cha-con, trả vật thể về lại thế giới tự do
        transform.SetParent(null);

        // 2. Bật lại vật lý và va chạm
        if (_rb != null) _rb.isKinematic = false;
        if (_collider != null) _collider.enabled = true;
    }
}