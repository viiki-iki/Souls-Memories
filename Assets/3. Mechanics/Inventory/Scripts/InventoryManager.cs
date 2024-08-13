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

        [Header ("SO")]
        [SerializeField] private BoolVariable canAddItem;
        [SerializeField] private ItemDataGameEvent usingItem;

        private float maxCapacity = 5;
        
        void Start()
        {
            UpdateCanAddItem();
        }

        void UpdateCanAddItem() => canAddItem.Value = itens.Count < maxCapacity;

        public void AddItem(ItemData item)
        {
            itens.Add(item);
            AddItemUI(item);
            UpdateCanAddItem();
        }

        void AddItemUI(ItemData item)
        {
            GameObject slot = Instantiate(slotPrefab, parent.transform);
            slot.transform.Find("Item_Icon").GetComponent<Image>().sprite = item.icon;
            Button button = slot.GetComponent<Button>();
            button.onClick.AddListener(() => UseItem(item));
        }

        void UseItem(ItemData item)
        {
            usingItem.Raise(item);
            //mudar cursor
            //desligar icon           
            print("está usando o item " + item.id);
        }

        public void CancelInteractionWithItem()
        {
            //cursor volta ao normal
            //icon do item liga
            print("cancelou ação");
        }

        void CombineItens()
        {
        }

        void RemoveItem(ItemData item)
        {
            itens.Remove(item);
            //ui slot destroy
            UpdateCanAddItem();
        }
    }
}