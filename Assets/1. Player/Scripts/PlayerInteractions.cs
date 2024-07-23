using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ScriptableObjectArchitecture;

public class PlayerInteractions : MonoBehaviour
{  
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    private GameObject lastItemClicked;
    [SerializeField] private Vector2GameEvent walkToDestination;
    [SerializeField] private ItemDataGameEvent getItem;

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
                GameObject obj = hitObs.transform.gameObject;
                ItemData_Scene script = obj.GetComponentInParent<ItemData_Scene>();
                lastItemClicked = obj;

                if (script.isClose == false)
                {
                    Collider2D goToCollider = script.interactableArea;
                    Collider2D playerCollider = GetComponentInChildren<Collider2D>();
                    Vector2 playerCenter = playerCollider.bounds.center;
                    Vector2 point = goToCollider.ClosestPoint(playerCenter);
                    walkToDestination.Raise(point);
                }
                else
                {
                    GetItem(obj.transform.parent.gameObject);
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

    private void GetItem(GameObject parent)
    {
        ItemData script = parent.GetComponent<ItemData_Scene>().ItemData;
        getItem.Raise(script);
        lastItemClicked = null;
        //parent.SetActive(false);
        Destroy(parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractableArea"))
        {
            collision.GetComponentInParent<ItemData_Scene>().isClose = true;

            if(lastItemClicked != null)
            {
                GetItem(collision.transform.parent.gameObject);
            }             
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractableArea"))
        {
            collision.GetComponentInParent<ItemData_Scene>().isClose = false;
        }
    }
}
