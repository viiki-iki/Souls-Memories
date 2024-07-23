using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    public bool collected = false;
  //  public bool canCombine;
}
