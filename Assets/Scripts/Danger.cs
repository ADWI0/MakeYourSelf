using UnityEngine;

public class Danger : MonoBehaviour
{
    GameObject player;
    Rigidbody playerRb;
    Vector3 playerPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            player.transform.rotation = Quaternion.identity;
            player.transform.position = playerPos;
            Goal.Instance.AddDeadCnt();
        }
    }
}
