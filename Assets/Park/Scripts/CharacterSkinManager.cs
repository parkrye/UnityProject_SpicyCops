using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    [SerializeField] List<Material> skinMaterials;
    [SerializeField] Renderer skinRenderer;

    void Awake()
    {
        skinMaterials = new List<Material>();
        skinRenderer = GetComponentInChildren<Renderer>();
    }

    void Start()
    {
        skinMaterials.Add(skinRenderer.materials[0]);
        for(int i = 1; i < 9; i++)
        {
            skinMaterials.Add(Resources.Load<Material>($"Skin/Skin{i}"));
        }
    }

    public void SettingColor(int num)
    {
        skinRenderer.materials[0].color = skinMaterials[num].color;
    }
}
