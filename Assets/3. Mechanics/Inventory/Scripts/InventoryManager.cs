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
        public bool isUsingItem;
        public ItemDataVariable usingItem;

        private void Start()
        {
            isUsingItem = false;
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
            usingItem.Value = item;
            isUsingItem = true;
            print("está usando o item " + item.id);
            //mudar cursor
            //desligar icon           
        }

        public void CancelInteractionWithItem()
        {
            //cursor volta ao normal
            //icon do item liga
            isUsingItem = false;
            print("cancelou ação");
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