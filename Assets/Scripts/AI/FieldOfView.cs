using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)] public float angle;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private GameObject player;
    public bool canSeePlayer;

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
        if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, radius))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    canSeePlayer = true;
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
