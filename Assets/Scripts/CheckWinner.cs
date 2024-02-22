using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CheckWinner : MonoBehaviour
{
    public static CheckWinner instance;

    public Camera defaultCamera;
    public Camera winnerCamera;
    public bool isWinner = false;

    public Transform target;
    public float smoothSpeed = 1.0f;

    public Transform playerRotation;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        defaultCamera.enabled = true;
        winnerCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWinner)
        {
            defaultCamera.enabled = false;
            winnerCamera.enabled = true;
        }
    }

    private void LateUpdate()
    {
        if(target != null && isWinner)
        {
            Vector3 desirePosition = new Vector3(target.position.x - 2.2f, target.position.y,target.position.z);

            Vector3 smoothedPosition = Vector3.Lerp(winnerCamera.transform.position, desirePosition, smoothSpeed*Time.deltaTime);

            winnerCamera.transform.position = smoothedPosition;

            playerRotation.LookAt(new Vector3(winnerCamera.transform.position.x, playerRotation.position.y, playerRotation.position.z));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.instance.groundedPlayer) {
            isWinner = true;
        }
    }
}
