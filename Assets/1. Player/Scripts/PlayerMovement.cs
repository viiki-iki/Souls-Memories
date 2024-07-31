using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInteractions playerInteractions;
    private Vector2 destination;
    private Vector2 detourPosition;
    private readonly RaycastHit2D[] hitPoints = new RaycastHit2D[64];
    private Animator animator;

    private bool isMoving;
    private bool isDetouring;

    private bool detour1Clear;
    private bool detour2Clear;
    private bool end1Clear;
    private bool end2Clear;
    private Vector2 detourPos1;
    private Vector2 detourPos2;

    [SerializeField] private float speed;

    private void Start()
    {
        isMoving = false;
        isDetouring = false;
        playerInteractions = GetComponent<PlayerInteractions>();
        animator = GetComponent<Animator>();
    }

    public void CheckObstacles(Vector2 target)
    {
        destination = target;
        Vector2 origin = transform.position;
        Vector2 direction = destination - origin;
        float distance = Vector2.Distance(origin, destination);
        int hits = Physics2D.RaycastNonAlloc(origin, direction, hitPoints, distance, playerInteractions.obstacleLayer);      
        if(hits > 0)
        {
            for (int i = 0; i < hits; i++)
            {
                if (hitPoints[i].collider != null)
                {
                    Debug.Log("Obstacle in the way: ");

                    Collider2D obsCollider = hitPoints[i].collider;
                    Vector2 obsSize = obsCollider.bounds.size;
                    float detourDistance = Mathf.Max(0.3f, Mathf.Min(obsSize.x, obsSize.y) / 2f);
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

    bool RaycastVerification(Vector2 origin, Vector2 direction, Color color)
    {
        bool pathClear = !Physics2D.Raycast(origin, direction - origin, Vector2.Distance(origin, direction), playerInteractions.obstacleLayer);
        Debug.DrawLine(origin, direction, color, 2f);

        if (pathClear)        
            return true;       
        else       
            return false;              
    }

    void UpdateAnimation()
    {
        float distance = Vector2.Distance(transform.position, destination);
        animator.SetFloat("distance", distance);
        if(distance > .01)
        {
            Vector2 direction = (Vector2)transform.position - destination;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            animator.SetFloat("angle", angle);
        }     
    }

    void Update()
    {        
        if (isMoving)
        {
            Vector2 nextPosition = isDetouring ? detourPosition : destination;
            transform.position = Vector2.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
            UpdateAnimation();

            if (isDetouring && !Physics2D.Raycast(transform.position, destination - (Vector2)transform.position, Vector2.Distance(transform.position, destination), playerInteractions.obstacleLayer))
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