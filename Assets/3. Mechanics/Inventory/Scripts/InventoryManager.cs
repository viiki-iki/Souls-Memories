using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> itens = new List<ItemData>();

    public void GetItem(ItemData dataItem)
    {
        itens.Add(dataItem);
        print("adicionou item" + dataItem.name);
    }
}
