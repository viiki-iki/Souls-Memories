using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Vector2 destination;
    private Vector2 detourPosition;
    private readonly RaycastHit2D[] hitPoints = new RaycastHit2D[64];

    private bool isMoving;
    private bool isDetouring;

    private bool detour1Clear;
    private bool detour2Clear;
    private bool end1Clear;
    private bool end2Clear;
    private Vector2 detourPos1;
    private Vector2 detourPos2;

    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private void Start()
    {
        isMoving = false;
        isDetouring = false;
    }

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
                if (obj.GetComponentInParent<ItemData_Scene>().isClose == false)
                {
                    Collider2D goToCollider = obj.GetComponentInParent<ItemData_Scene>().goToCollider;
                    Collider2D playerCollider = GetComponent<Collider2D>();
                    Vector2 playerCenter = playerCollider.bounds.center;
                    Vector2 point = goToCollider.ClosestPoint(playerCenter);
                    destination = point;
                    CheckObstacles();
                }             
            }
            else if (hitGround.collider != null && hitObs.collider == null)
            {
                destination = hitGround.point;
                CheckObstacles();            
            } 
        }
    }

    private void CheckObstacles()
    {
        Vector2 origin = transform.position;
        Vector2 direction = (destination - origin).normalized;
        float distance = Vector2.Distance(origin, destination);

        int hits = Physics2D.RaycastNonAlloc(origin, direction, hitPoints, distance, obstacleLayer);
        
        if(hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                if (hitPoints[i].collider != null)
                {
                    Debug.Log("Obstacle in the way: ");

                    Collider2D obsCollider = hitPoints[i].collider;
                    Vector2 obsSize = obsCollider.bounds.size;
                    float detourDistance = Mathf.Max(0.5f, Mathf.Min(obsSize.x, obsSize.y) / 2f);
                    Vector2 perpDirection = Vector2.Perpendicular(direction);

                    StartCoroutine(PathOptionsCheck(hitPoints[i].point, perpDirection, detourDistance));
                    isDetouring = true;
                }
            }
        }
        isMoving = true;
    }

    IEnumerator PathOptionsCheck(Vector2 itemPoint, Vector2 perpDirection, float detourDistance)
    {
        //verificar os 2 lados do obs
        detourPos1 = itemPoint + perpDirection * detourDistance;
        detourPos2 = itemPoint - perpDirection * detourDistance;
        detour1Clear = RaycastVerification(transform.position, detourPos1, Color.red);
        detour2Clear = RaycastVerification(transform.position, detourPos2, Color.blue);
        end1Clear = RaycastVerification(detourPos1, destination, Color.red);
        end2Clear = RaycastVerification(detourPos2, destination, Color.blue);

        //escolher o desvio
        if (detour1Clear && detour2Clear)
        {
            if (end1Clear && end2Clear)
                detourPosition = Vector2.Distance(transform.position, detourPos1) < Vector2.Distance(transform.position, detourPos2) ? detourPos1 : detourPos2;
            else if (end1Clear)
                detourPosition = detourPos1;
            else if (end2Clear)
                detourPosition = detourPos2;
        }
        else if (detour1Clear && end1Clear)
            detourPosition = detourPos1;           
        else if (detour2Clear && end2Clear)               
            detourPosition = detourPos2;            
        else    // quando os 2 lados n dao certo
        {
            print("alterando");
            isMoving = false;
            StartCoroutine(PathOptionsCheck(itemPoint, perpDirection, detourDistance + 0.5f));
        }
        yield break;
    }

    private bool RaycastVerification(Vector2 origin, Vector2 direction, Color color)
    {
        bool pathClear = !Physics2D.Raycast(origin, direction - origin, Vector2.Distance(origin, direction), obstacleLayer);
        Debug.DrawLine(origin, direction, color, 2f);

        if (pathClear)        
            return true;       
        else       
            return false;              
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

    void Update()
    {        
        if (isMoving)
        {
            Vector2 nextPosition = isDetouring ? detourPosition : destination;
            transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);           

            if (isDetouring && !Physics2D.Raycast(transform.position, destination - (Vector2)transform.position, Vector2.Distance(transform.position, destination), obstacleLayer))
            {
                nextPosition = destination;
                isDetouring = false;
            }

            if ((Vector2)transform.position == nextPosition)         
            {
                if (isDetouring)
                    isDetouring = false;                   
                else
                    isMoving = false;
            }
        }
    }
}
