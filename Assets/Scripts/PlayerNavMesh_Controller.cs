using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh_Controller : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                navMeshAgent.SetDestination(hitInfo.point);
            }
        }
    }
}
