using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       moveDirection = new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"), 0.0f);
       moveDirection = transform.TransformDirection(moveDirection);
       moveDirection = moveDirection * speed;
       // Move the controller
       controller.Move(moveDirection * Time.deltaTime);
    }
}
