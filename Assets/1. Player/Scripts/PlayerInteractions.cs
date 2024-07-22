using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ScriptableObjectArchitecture;

public class PlayerInteractions : MonoBehaviour
{  
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    [SerializeField] private GameObjectVariable lastItemClicked;
    [SerializeField] private Vector2GameEvent walkToDestination;

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
                lastItemClicked.BaseValue = obj;

                if (script.isClose == false)
                {
                    Collider2D goToCollider = script.interactableArea;
                    Collider2D playerCollider = GetComponentInChildren<Collider2D>();
                    Vector2 playerCenter = playerCollider.bounds.center;
                    Vector2 point = goToCollider.ClosestPoint(playerCenter);
                    //destination = point;
                    walkToDestination.Raise(point);
                   // CheckObstacles();
                }
                else
                {
                    //inventory
                }
            }
            else if (hitGround.collider != null && hitObs.collider == null)
            {
               // lastItemClicked.BaseValue = null;
              //  destination = hitGround.point;
                walkToDestination.Raise(hitGround.point);
               // CheckObstacles();
            }
            //else { //lastItemClicked.BaseValue = null; }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("InteractableArea"))
        {
            collision.GetComponentInParent<ItemData_Scene>().isClose = true;
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
