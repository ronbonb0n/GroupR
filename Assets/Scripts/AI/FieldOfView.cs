using System.Collections;
using UnityEngine;

// reference: How to Add a Field of View for Your Enemies [Unity Tutorial] https://www.youtube.com/watch?v=j1-OyLo77ss
public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)] public float angle;

    private GameObject player;
    public bool canSeePlayer;
    public Vector3 lastSeenPlayerAt;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Transform target = player.transform;
        Vector3 directionToTarget = target.position - transform.position;
        if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2 && Vector3.Distance(target.position, transform.position) <= radius)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, directionToTarget, Color.cyan, 0.1f);
            if (Physics.Raycast(transform.position, directionToTarget, out hit, radius))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    canSeePlayer = true;
                    lastSeenPlayerAt = target.position;
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
}