using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform Target;
    public float Smooth_Speed = 0.125f;
    public Vector3 Offset = new(-55, 40, 0);
    public bool Rotate_Around_Player = true;
    public float Rotation_Speed = 5.0f;

    private void FixedUpdate()
    {
        if (Rotate_Around_Player)
        { Quaternion Cam_Turn_Angle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Rotation_Speed, Vector3.up);
            Offset = Cam_Turn_Angle * Offset;
        }
        Vector3 Desired_Postion = Target.position + Offset;
        Vector3 Smooth_Position = Vector3.Lerp(transform.position, Desired_Postion, Smooth_Speed);
        transform.position = Smooth_Position;
        if (Rotate_Around_Player)
        {
            transform.LookAt(Target);
        }
    }
}
