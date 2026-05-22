using UnityEngine;

public class CustomizableObject : MonoBehaviour
{
    [Header("Design Settings")]
    [SerializeField] private GameObject[] _designVariants; // Các mẫu 3D khác nhau

    [Header("Color Settings")]
    [SerializeField] private MeshRenderer _meshRenderer; // Nơi sẽ đổi màu (Material)
    [SerializeField] private Material[] _colorMaterials; // Danh sách các màu

    private int _currentDesignIndex = 0;

    public GameObject[] GetDesigns() => _designVariants;
    public Material[] GetColors() => _colorMaterials;

    public void SetDesign(int index)
    {
        if (index < 0 || index >= _designVariants.Length) return;

        _designVariants[_currentDesignIndex].SetActive(false);
        _currentDesignIndex = index;
        _designVariants[_currentDesignIndex].SetActive(true);
    }

    public void SetColor(int index)
    {
        if (index < 0 || index >= _colorMaterials.Length || _meshRenderer == null) return;

        _meshRenderer.material = _colorMaterials[index];
    }
}