using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [SerializeField] private bool doInteraction = false;

    [Header("Can Combine With")]
    [SerializeField] private bool Book = false;
    [SerializeField] private bool Book2 = false;
    [SerializeField] private bool Book3 = false;
    [SerializeField] private bool Hand = false;
    [SerializeField] private bool Hand2 = false;
    [SerializeField] private bool Hand3 = false;
    [SerializeField] private bool Other = false;
    [SerializeField] private bool Other2 = false;
    [SerializeField] private bool Other3 = false;
    private List<string> canCombineItens = new List<string>();

    private void Awake()
    {
        if (Book) { canCombineItens.Add("Book"); }
        if (Book2) { canCombineItens.Add("Book2"); }
        if (Book3) { canCombineItens.Add("Book3"); }
        if (Hand) { canCombineItens.Add("Hand"); }
        if (Hand2) { canCombineItens.Add("Hand2"); }
        if (Hand3) { canCombineItens.Add("Hand3"); }
        if (Other) { canCombineItens.Add("Other"); }
        if (Other2) { canCombineItens.Add("Other2"); }
        if (Other3) { canCombineItens.Add("Other3"); }
    }

    public void CheckInteractions(string lastClickedItemName)
    {
        foreach (var item in canCombineItens)
        {
            if(lastClickedItemName == item)
            {
                Debug.Log("HEHE COMBINOU " + lastClickedItemName + "com " + item );
                //futuramente as interações vao ficar em outro script?
                doInteraction = true;
            }
            else
            {
                doInteraction = false;
            }
        }
    }
}
