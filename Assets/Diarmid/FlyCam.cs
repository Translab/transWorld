/*
using UnityEngine;
using System.Collections;

public class FlyCam : MonoBehaviour
{

    public float speed = 50.0f;
    public float sensitivity = 0.25f;
    public bool inverted = false;

    Vector3 lastMouse = new Vector3(255, 255, 255);

    public bool smooth = true;
    public float acceleration = 0.1f;
    private float actualSpeed = 0.0f;
    private Vector3 lastDirection = new Vector3();

    void Start()
    {

    }

    void Update()
    {

        //mouse look
        lastMouse = Input.mousePosition - lastMouse;
        if (!inverted) lastMouse.y = -lastMouse.y;
        lastMouse *= sensitivity;
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;



        //movement

        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W)) direction.z += 1.0f;
        if (Input.GetKey(KeyCode.S)) direction.z -= 1.0f;
        if (Input.GetKey(KeyCode.A)) direction.x -= 1.0f;
        if (Input.GetKey(KeyCode.D)) direction.x += 1.0f;

        direction.Normalize();

        if (direction != Vector3.zero)
        {
            if (actualSpeed < 1)
                actualSpeed += acceleration * Time.deltaTime * 40;
            else 
                actualSpeed = 1.0f;
            lastDirection = direction;
        }
        else
        {
            if (actualSpeed > 0)
                actualSpeed -= acceleration * Time.deltaTime * 20;
            else
                actualSpeed = 0.0f;
        }
        if (smooth)
            transform.Translate(lastDirection * actualSpeed * speed * Time.deltaTime);
        else
            transform.Translate(direction * speed * Time.deltaTime);

    }

}
*/
using UnityEngine;
using System.Collections;
 
public class FlyCam : MonoBehaviour
{

    /*
    EXTENDED FLYCAM
        Desi Quintans (CowfaceGames.com), 17 August 2012.
        Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
    LICENSE
        Free as in speech, and free as in beer.
 
    FEATURES
        WASD/Arrows:    Movement
                  Q:    Climb
                  E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
    */

    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Screen.lockCursor = true;
    }

    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

        if (Input.GetKeyDown(KeyCode.End))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}