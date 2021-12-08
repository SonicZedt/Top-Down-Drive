using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Input input;
    public Joystick joystick;

    [Header("Car Configuration")]
    public float rotationSpeed;
    public float movementSpeed;

    void Update()
    {
        if(!input.Detected()) return;

        if(input.InJoystickArea()) {
            Steer();
            Move();
        }
    }

    private void Steer()
    {
        float angle = Mathf.Atan2(joystick.Direction.y, joystick.Direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(angle * Vector3.forward);

        transform.rotation = rotation;
    }

    private void Move()
    {
        float speed = movementSpeed * Time.deltaTime;
        
        transform.Translate(0, speed, 0);
    }
}
