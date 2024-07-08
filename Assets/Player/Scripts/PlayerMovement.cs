using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Vector2 destination;
    private bool isMoving;
   // [SerializeField] private float perspectiveScale;
   // [SerializeField] private float scaleRatio;

    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
       // destination = transform.position;     
        isMoving = false;
       // AdjustPerspective();
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {        
        if (ctx.performed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // RaycastHit2D hitGround = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, groundLayer);
            // if(hitGround.collider != null)
            //  {
            //destination = hitGround.point;
            destination = worldPosition;
                isMoving = true;
          //  }
            //print(mousePosition);
            //print(destination);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SceneEdge"))
        {
            isMoving = false;
        }
    }

    // private void AdjustPerspective()
    // {
    //     Vector3 scale = transform.localScale;
    //     scale.x = perspectiveScale / (scaleRatio - transform.position.y);
    //     scale.y = perspectiveScale / (scaleRatio - transform.position.y);
    //     transform.localScale = scale;
    // }

    void Update()
    {        
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);          
            if ((Vector2)transform.position == destination)         
            {               
                isMoving = false;         
            }
        }
      //  AdjustPerspective();
    }
}
