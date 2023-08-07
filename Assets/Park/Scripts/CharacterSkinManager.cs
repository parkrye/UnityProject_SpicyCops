using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.StickyNote;

public class CharacterSkinManager : MonoBehaviour
{
    [SerializeField] List<Color> skinMaterials;
    [SerializeField] Renderer skinRenderer;
    [SerializeField] protected int colorNum;

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
        colorNum = num;
        skinRenderer.materials[0].color = skinMaterials[num];
    }
}
