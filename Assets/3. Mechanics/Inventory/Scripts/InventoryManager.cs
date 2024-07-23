using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> itens = new List<ItemData>();
    //[SerializeField] private GameObject panel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject parent;
    private float maxCapacity = 5;

    private void Start()
    {
       // AddItemUI();
    }
    
    public bool CanAddItem()
    {      
        if (itens.Count < maxCapacity)
            return true;
        else
            return false;
    }

    public void AddItem(ItemData item)
    {
        itens.Add(item);
        AddItemUI(item);
        print("adicionou item " + item.name);
    }

    private void AddItemUI(ItemData item)
    {
        GameObject slot = Instantiate(slotPrefab, parent.transform);        
        slot.transform.Find("Item_Icon").GetComponent<Image>().sprite = item.icon;
        Button button = slot.GetComponent<Button>();
        button.onClick.AddListener(() => GetItem(item));
        //item.inventorySlot = slot;
    }

    public void GetItem(ItemData item)
    {
        print("está usando o item " + item.itemName);
        //mudar cursor
       //testar primeiro como botao
                                        //depois como arrastar
       //bool usingItem
       //botao com sprite invisivel

        //nao tirar ainda do inventario, apenas deixar invisivel pois a açao pode ser cancelada 
       // e o item pode voltar pro inventario
    }

    private void RemoveItem(ItemData item)
    {
        itens.Remove(item);
    }
}