using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ScriptableObjectArchitecture;

public class PlayerInteractions : MonoBehaviour
{  
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    private GameObject lastItemClicked = null;
    [SerializeField] private Vector2GameEvent walkToDestination;
    [SerializeField] private ItemDataGameEvent addItem;
    [SerializeField] private InventoryManager inventoryManager;

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hitGround = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, groundLayer);
            RaycastHit2D hitObs = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, obstacleLayer);

            if (hitObs.collider != null && hitObs.transform.gameObject.CompareTag("Clickable"))
            {
                GameObject clickableArea = hitObs.transform.gameObject;
                lastItemClicked = clickableArea.transform.parent.gameObject;
                ItemData_Scene script = lastItemClicked.GetComponent<ItemData_Scene>();              

                if (script.isClose)
                {
                    AddItem(lastItemClicked);
                }
                else
                {                    
                    Collider2D goToCollider = script.interactableArea;
                    Collider2D playerCollider = GetComponentInChildren<Collider2D>();
                    Vector2 playerCenter = playerCollider.bounds.center;
                    Vector2 point = goToCollider.ClosestPoint(playerCenter);
                    walkToDestination.Raise(point);
                }
            }
            else if (hitGround.collider != null && hitObs.collider == null)
            {
                lastItemClicked = null;
                walkToDestination.Raise(hitGround.point);
            }
            else { lastItemClicked = null; }
        }
    }

    private void AddItem(GameObject obj)
    {
        if (inventoryManager.CanAddItem())
        {
            print("pode pegar item");
            ItemData script = obj.GetComponent<ItemData_Scene>().ItemData;
            addItem.Raise(script);
            lastItemClicked = null;
            Destroy(obj);
        }
        else
        {
            print("inventario cheio");
            lastItemClicked = null;          
        }     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableArea"))
        {
            collision.GetComponentInParent<ItemData_Scene>().isClose = true;         

            if (lastItemClicked != null && lastItemClicked == collision.transform.parent.gameObject)
            {
                AddItem(lastItemClicked);
            }             
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableArea"))
        {
            collision.GetComponentInParent<ItemData_Scene>().isClose = false;
        }
    }
}
