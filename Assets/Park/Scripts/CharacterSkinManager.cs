using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    [SerializeField] List<Color> skinMaterials;
    [SerializeField] Renderer skinRenderer;

    void Awake()
    {
        skinMaterials = new List<Color>();
        skinRenderer = GetComponentInChildren<Renderer>();
    }

    void Start()
    {
        for(int i = 1; i < 9; i++)
        {
            skinMaterials.Add(Resources.Load<Material>($"Skin/Skin{i}").color);
        }
    }

    public void SettingColor(int num)
    {
        skinRenderer.materials[0].color = skinMaterials[num];
    }
}
