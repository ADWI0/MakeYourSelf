using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    GameObject cam;
    Vector3 playerPos;

    [Header("PlayerMove")]
    public float accelSpeed = 5;
    public float decclSpeed = 10;
    public float impulsePow = 10;
    public KeyCode brake = KeyCode.Space;
    public KeyCode reset = KeyCode.R;

    [Header("Camera")]
    public float distance = 5;
    public float sensitivity = 3;

    [Space(10)]
    public float squeezTime = 1;
    bool hit = false;
    float timer = 0;

    float yaw;
    float pitch;

    float speed;
    float curSpeed;
    float prevSpeed;

    bool ch = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        speed = accelSpeed;
        curSpeed = rb.linearVelocity.magnitude;
        prevSpeed = curSpeed;

        playerPos = transform.position;

        PauseMenu.Instance.Sens = sensitivity;

        ch = false;
    }

    void Update()
    {
        sensitivity = PauseMenu.Instance.Sens;

        if (PauseMenu.Instance.Pause) return;

        //Brake
        curSpeed = rb.linearVelocity.magnitude;
        if (curSpeed < prevSpeed) speed = decclSpeed;
        else speed = accelSpeed;
        prevSpeed = curSpeed;

        if (Input.GetKeyDown(brake)) Goal.Instance.AddBrakeCnt();
        if (Input.GetKey(brake))
        {
            if (rb.linearVelocity.magnitude > 1)
            {
                rb.AddForce(new Vector3(-rb.linearVelocity.x, 0, -rb.linearVelocity.z).normalized * decclSpeed * 2);
                rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.zero, decclSpeed * 2 * Time.fixedDeltaTime);
            }
            else
            {
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                rb.angularVelocity = Vector3.zero;
            }
        }

        // player move
        if (!Goal.Instance.Fin)
        {
            float speedZ = Input.GetAxis("Vertical") * speed;
            float speedX = Input.GetAxis("Horizontal") * speed;

            Vector3 dirZ = cam.transform.forward; dirZ.y = 0;
            Vector3 dirX = cam.transform.right; dirX.y = 0;

            rb.AddForce(dirZ * speedZ);
            rb.AddForce(dirX * speedX);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            if (!ch)
            {
                rb.AddForce(Vector3.up * impulsePow, ForceMode.VelocityChange);
                ch = true;
            }
        }

        // camera move
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        cam.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        cam.transform.position = transform.position - cam.transform.forward * distance;

        // camera distance
        float scroll = Input.mouseScrollDelta.y;
        if (scroll < 0) distance++;
        if (scroll > 0) distance--;
        distance = Mathf.Clamp(distance, 1, 30);

        // camera clamp
        Vector3 pos = cam.transform.position;
        pos.y = Mathf.Max(pos.y, 0.2f);
        cam.transform.position = pos;

        pitch = Mathf.Clamp(pitch, -35, 80);

        // reset
        if (Input.GetKeyDown(reset) || transform.position.y < -1)
        {
            Reset();
        }
    }

    private void Reset()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.position = playerPos;
        Goal.Instance.Restart();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Respawn")
        {
            if (!hit) hit = true;
            if (hit)
            {
                timer += Time.deltaTime;
                if (timer >= squeezTime)
                {
                    hit = false;
                    timer = 0;  
                    Reset();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            //rb.linearVelocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
        }
    }
}