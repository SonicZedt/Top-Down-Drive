using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public Joystick joystick;

    [HideInInspector] public Vector2 position, positionScreen, positionViewport;
    [HideInInspector] public Vector2 origin, originViewport;
    
    private bool originSet;

    void Start()
    {
        
    }

    void Update()
    {
        positionScreen = Touchscreen.current.primaryTouch.position.ReadValue();
        positionViewport = Camera.main.ScreenToViewportPoint(positionScreen);
        position = Camera.main.ScreenToWorldPoint(positionScreen);

        SetOrigin();
    }

    public bool Detected()
    {
        return Touchscreen.current.primaryTouch.press.isPressed;
    }

    private void SetOrigin()
    {
        if(!Detected()) {
            origin = Vector2.zero;
            originViewport = Vector2.zero;
            originSet = false;
        }

        else {
            if(originSet) return;

            origin = position;
            originViewport = Camera.main.WorldToViewportPoint(origin);
            originSet = true;
        }
    }

    public bool InJoystickArea()
    {
        RectTransform joystickRect = joystick.GetComponent<RectTransform>();
        Vector3 joystickArea = new Vector3(joystickRect.rect.width, joystickRect.rect.height, 0);
        Vector3 joystickScreenArea = Camera.main.ScreenToViewportPoint(joystickArea);
        
        return((originViewport.x <= joystickScreenArea.x) && ((originViewport.y <= joystickScreenArea.y)));
    }
}
