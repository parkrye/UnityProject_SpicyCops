using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkinManager : MonoBehaviour
{
    [SerializeField] List<Color> skinMaterials;
    [SerializeField] Renderer skinRenderer;
    [SerializeField] protected int colorNum;

    void Start()
    {
        skinMaterials = new List<Color>();
        for (int i = 1; i < 9; i++)
        {
            skinMaterials.Add(Resources.Load<Material>($"Skin/Skin{i}").color);
        }
    }

    public void SettingColor(int num)
    {
        StartCoroutine(WaitRoutine(num));
    }

    IEnumerator WaitRoutine(int num)
    {
        while(skinMaterials.Count == 0)
            yield return null;
        skinRenderer = GetComponentInChildren<Renderer>();
        colorNum = num;
        Debug.Log($"{num}, {skinMaterials.Count}");
        skinRenderer.materials[0].color = skinMaterials[num];
    }
}
