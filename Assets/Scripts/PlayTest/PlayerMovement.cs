using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Vector2 moveValue;
    public float speed = 12f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        var movement = transform.right * moveValue.x + transform.forward * moveValue.y;
        controller.Move(movement * speed * Time.deltaTime);
    }
}
