using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

[CreateAssetMenu(menuName = "SO/ItemData", fileName = "ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [SerializeField] ItensEnum.Itens itensToCombineWith;
    private readonly List<string> canCombineItens = new List<string>();

    private void Awake()
    {
        foreach (var item in GetSelectedFlags(itensToCombineWith))
        {
            canCombineItens.Add(item.ToString());
        }
    }

    public List<E> GetSelectedFlags<E>(E flags) where E : Enum
    {
        List<E> selectedFlags = new List<E>();
        foreach (E value in Enum.GetValues(typeof(E)))
        {
            if (flags.HasFlag(value))
            {
                selectedFlags.Add(value);
            }
        }
        return selectedFlags;
    }

    public bool CheckInteractions(string lastClickedItemName)
    {
        try
        {
            var item = (ItensEnum.Itens)Enum.Parse(typeof(ItensEnum.Itens), lastClickedItemName);
            return itensToCombineWith.HasFlag(item);
        }
        catch
        {
            return false;
        }
    }
}