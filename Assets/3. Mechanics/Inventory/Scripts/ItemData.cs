using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
   // public GameObject inventorySlot;
    public Sprite icon;
   // public bool collected = false;
  //  public bool canCombine;
}
