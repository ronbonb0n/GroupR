using UnityEngine;
using UnityEngine.AI;

public class Drone_Anim_Script : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    float current_speed = 0f;
    float initial_rotation = 0f;
    float change_rotation = 0f;

    float remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private void Start()
    {
        initial_rotation = agent.transform.rotation.y;
    }

    void Update()
    {
        current_speed = agent.velocity.magnitude;
        current_speed = remap(current_speed, 0, agent.speed, 0, 1);
        animator.SetFloat("movementSpeed", current_speed);

        change_rotation = agent.transform.rotation.y - initial_rotation;
        change_rotation = remap(change_rotation, -0.0075f, 0.0075f, -1f, 1f);
        animator.SetFloat("leftRight", change_rotation);
        initial_rotation = agent.transform.rotation.y;
    }
}
