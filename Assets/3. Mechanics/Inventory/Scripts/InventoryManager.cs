using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<ItemData> itens = new List<ItemData>();
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject parent;
        private float maxCapacity = 5;
        public StringVariable usingItem;

        private void Start()
        {
            usingItem.Value = "";
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
            button.onClick.AddListener(() => UseItem(item));
        }

        public void UseItem(ItemData item)
        {
            print("está usando o item " + item.itemName);
            //mudar cursor
            //desligar icon
            usingItem.Value = item.itemName;
        }

        public void CancelInteractionWithItem()
        {
            //cursor volta ao normal
            //icon do item liga
            usingItem.Value = "";
            print("cancelou");
        }

        public void CombineItens()
        {
        }

        private void RemoveItem(ItemData item)
        {
            itens.Remove(item);
        }
    }
}