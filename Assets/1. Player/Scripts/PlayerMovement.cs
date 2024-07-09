using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Vector2 destination;
    private Vector2 detourPosition;

    private bool isMoving;
    private bool isDetouring;
    
    // [SerializeField] private float perspectiveScale;
    // [SerializeField] private float scaleRatio;

    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private void Start()
    {
        isMoving = false;
        isDetouring = false;
        // AdjustPerspective();
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {        
        if (ctx.performed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);             
            RaycastHit2D hitGround = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, groundLayer);
            if (hitGround.collider != null) //futuramente add movimentaçao p/ o objeto interactable
            {
                destination = hitGround.point;
               // destination = worldPosition;
                //isMoving = true;

                //raycast para obstaculos
                RaycastHit2D[] obstaclesHit = Physics2D.RaycastAll(transform.position, destination - (Vector2)transform.position, Vector2.Distance(transform.position, destination), obstacleLayer);
                foreach (var item in obstaclesHit)
                {
                    if (item.collider != null)
                    {
                        //collider
                        Collider2D obsCollider = item.collider;
                        Vector2 obsSize = obsCollider.bounds.size;
                        float detourDistance = Mathf.Max(obsSize.x, obsSize.y) + 0.5f;

                        //ponto de desvio
                        Vector2 direction = (destination - (Vector2)transform.position).normalized;
                        Vector2 perpDirection = Vector2.Perpendicular(direction);

                        //verificar os 2 lados do obs
                        Vector2 detourPos1 = item.point + perpDirection * detourDistance;
                        Vector2 detourPos2 = item.point - perpDirection * detourDistance;
                        bool detour1Clear = !Physics2D.Raycast(transform.position, detourPos1 - (Vector2)transform.position, Vector2.Distance(transform.position, detourPos1), obstacleLayer);
                        bool detour2Clear = !Physics2D.Raycast(transform.position, detourPos2 - (Vector2)transform.position, Vector2.Distance(transform.position, detourPos2), obstacleLayer);

                        //escolher o desvio mais curto
                        if (detour1Clear && detour2Clear)
                            detourPosition = Vector2.Distance(transform.position, detourPos1) < Vector2.Distance(transform.position, detourPos2) ? detourPos1 : detourPos2;
                        else if (detour1Clear)
                            detourPosition = detourPos1;
                        else if (detour2Clear)
                            detourPosition = detourPos2;
                        else
                        {
                            print("sem caminho livre");
                            isMoving = false;
                            return;
                        }                  
                        isDetouring = true;
                    }
                }
                isMoving = true;
                if (!isDetouring)
                    Debug.Log("No obstacles");
            }     
        }
    }

   // private void OnCollisionStay2D(Collision2D collision)
   // {
   //     if (collision.gameObject.CompareTag("SceneEdge"))
   //     {
   //        // isMoving = false;
   //     }
   // }

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
            Vector2 nextPosition = isDetouring ? detourPosition : destination;
            transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);           

            if ((Vector2)transform.position == nextPosition)         
            {
                if (isDetouring)
                    isDetouring = false;                   
                else
                    isMoving = false;
            }
        }
      //  AdjustPerspective();
    }
}
