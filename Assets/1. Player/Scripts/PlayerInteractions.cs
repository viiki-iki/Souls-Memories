using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Inventory;

public class PlayerInteractions : MonoBehaviour
{  
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    private GameObject lastThingClicked = null;
    private bool isUsingItem = false;
    private ItensEnum.Itens itens;

    [Header ("SO")]
    [SerializeField] private Vector2GameEvent walkToDestination;
    [SerializeField] private ItemDataGameEvent addItem;
    [SerializeField] private GameEvent cancelInteractions;
    [SerializeField] private BoolVariable canAddItem;

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {           
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hitGround = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, groundLayer);
            RaycastHit2D hitObs = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, obstacleLayer);

            if (hitObs.collider != null && hitObs.transform.CompareTag("Clickable"))
                OnClickItem(hitObs);
            else if (hitGround.collider != null && hitObs.collider == null)
            {
                ResetInteractions();
                walkToDestination.Raise(hitGround.point);
            }
            else ResetInteractions();
        }
    }

    public void UsingItem(ItemData item)
    {
        isUsingItem = true;
        itens = item.id;
    }

    void ResetInteractions()
    {
        isUsingItem = false;
        cancelInteractions.Raise();
        lastThingClicked = null;
    }

    void OnClickItem(RaycastHit2D hit)
    {
        GameObject clickableArea = hit.transform.gameObject;
        lastThingClicked = clickableArea.transform.parent.gameObject;
        ItemData_Scene itemScene = lastThingClicked.GetComponent<ItemData_Scene>();

        if (itemScene.isClose)
            TryInteraction(lastThingClicked, itemScene.ItemData);
        else
            GoToItem(itemScene);
    }

    void GoToItem(ItemData_Scene script)
    {
        Collider2D goToCollider = script.interactableArea;
        Collider2D playerCollider = GetComponentInChildren<Collider2D>();
        Vector2 playerCenter = playerCollider.bounds.center;
        Vector2 point = goToCollider.ClosestPoint(playerCenter);
        walkToDestination.Raise(point);
    }

    void TryInteraction(GameObject obj, ItemData script)
    {      
        if(isUsingItem)
        {
            if (script.CheckInteractions(itens))
            {
                //inventoryManager.CombineItens();
                print("combinou " + itens + " com " + script.id);
            }
            else
            {
                print("n pode combinar");                
            }
        }
        else
            TryAdd(obj, script);

        ResetInteractions();
    }

    void TryAdd(GameObject obj, ItemData script)
    {
        if (canAddItem.Value)
        {
            addItem.Raise(script);
            Destroy(obj, 0.5f);
        }
        else
            print("inventario cheio");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableArea"))
        {
            ItemData_Scene itemScene = collision.GetComponentInParent<ItemData_Scene>();
            itemScene.isClose = true;         

            if (lastThingClicked != null && lastThingClicked == collision.transform.parent.gameObject)
                TryInteraction(lastThingClicked, itemScene.ItemData);            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableArea"))
        {
            if (collision.transform.parent.TryGetComponent(out ItemData_Scene item))
                item.isClose = false;                                  
        }
    }
}
