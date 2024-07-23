using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Scene : MonoBehaviour
{
    [SerializeField] private ItemData data;
    public bool isClose;
    public Collider2D interactableArea;
    public ItemData ItemData => data;

    private void Start()
    {
        if (data.collected)
        {
            Destroy(gameObject);
        }
    }
}
