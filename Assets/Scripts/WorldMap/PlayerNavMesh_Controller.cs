using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh_Controller : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Animator animator;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                navMeshAgent.SetDestination(hitInfo.point);
            }
        }
        animator.SetBool("isRunning", navMeshAgent.hasPath);
    }
}
