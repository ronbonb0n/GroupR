using System.Collections;
using UnityEngine;

// reference: How to Add a Field of View for Your Enemies [Unity Tutorial] https://www.youtube.com/watch?v=j1-OyLo77ss
public class Senses : MonoBehaviour
{
    public float sightRadius;
    [Range(0, 360)] public float sightAngle;

    private GameObject player;
    public bool canSeePlayer;
    public bool canHearPlayer;
    public Vector3 lastSpottedPlayerAt;
    public bool isAttracted = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(SensesRoutine());
    }

    private IEnumerator SensesRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
            HearingCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Transform target = player.transform;
        Vector3 directionToTarget = target.position - transform.position;
        if (Vector3.Angle(transform.forward, directionToTarget) < sightAngle / 2 &&
            Vector3.Distance(target.position, transform.position) <= sightRadius)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, directionToTarget, Color.cyan, 0.1f);
            if (Physics.Raycast(transform.position, directionToTarget, out hit, sightRadius))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("Player"))
                {
                    if (!hitObject.GetComponent<PlayerMovement>().isCloaking)
                    {
                        canSeePlayer = true;
                        lastSpottedPlayerAt = target.position;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }

    private void HearingCheck()
    {
        Transform target = player.transform;

        float detectionDistance;
        PlayerMovement playerComp = player.GetComponent<PlayerMovement>();
        if (playerComp.isCrouching)
        {
            detectionDistance = 3;
        }
        else if (playerComp.isSprinting)
        {
            detectionDistance = 14;
        }
        else
        {
            detectionDistance = 7;
        }

        Vector3 directionToTarget = target.position - transform.position;
        if (Vector3.Distance(target.position, transform.position) <= detectionDistance)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, directionToTarget, Color.magenta, 0.1f);
            if (Physics.Raycast(transform.position, directionToTarget, out hit, detectionDistance))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.CompareTag("Player"))
                {
                    if (!hitObject.GetComponent<PlayerMovement>().isCloaking)
                    {
                        canHearPlayer = true;
                        lastSpottedPlayerAt = target.position;
                    }
                    else
                    {
                        canHearPlayer = false;
                    }
                }
                else
                {
                    canHearPlayer = false;
                }
            }
            else
            {
                canHearPlayer = false;
            }
        }
        else
        {
            canHearPlayer = false;
        }
    }
}