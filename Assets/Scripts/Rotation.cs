using UnityEngine;

public class Rotation : MonoBehaviour
{
    Transform obj;
    public float rotateSpeed = 15;
    public bool reverse = false;

    void Start()
    {
        obj = transform;
    }

    void Update()
    {
        if (reverse) 
            obj.rotation *= Quaternion.Euler(0, -rotateSpeed * Time.deltaTime, 0);
        else
            obj.rotation *= Quaternion.Euler(0, rotateSpeed * Time.deltaTime, 0);
    }
}
