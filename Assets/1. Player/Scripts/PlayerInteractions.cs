using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{  
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    private GameObject lastThingClicked = null;
    //private bool isUsingItem = false;
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

            if (hitObs.collider != null && hitObs.transform.CompareTag("Clickable"))
            {
                GameObject clickableArea = hitObs.transform.gameObject;
                lastThingClicked = clickableArea.transform.parent.gameObject;
                ItemData_Scene script = lastThingClicked.GetComponent<ItemData_Scene>();

                if (script.isClose)
                {
                    TryInteraction(lastThingClicked);
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
                inventoryManager.CancelInteractionWithItem();
                lastThingClicked = null;
                walkToDestination.Raise(hitGround.point);
            }
            else { lastThingClicked = null; inventoryManager.CancelInteractionWithItem(); }
        }
    }

    private void TryInteraction(GameObject obj)
    {
        if(inventoryManager.usingItem.Value != null)
        {
            ItemData script = obj.GetComponent<ItemData_Scene>().ItemData;
            script.CheckInteractions(inventoryManager.usingItem.Value);
          //  if(script.)
        }
        if (inventoryManager.CanAddItem())
        {
            print("pode pegar item");
            
            //addItem.Raise(script);       
            Destroy(obj, 0.5f);
        }
        else
        {
            print("inventario cheio");
        }
        lastThingClicked = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableArea"))
        {
            collision.GetComponentInParent<ItemData_Scene>().isClose = true;         

            if (lastThingClicked != null && lastThingClicked == collision.transform.parent.gameObject)
            {
                TryInteraction(lastThingClicked);
            }             
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableArea"))
        {
            Transform parent = collision.transform.parent;
            if (parent.TryGetComponent(out ItemData_Scene item))
            {
                item.isClose = false;
            }                                  
        }
    }
}
