using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private float cameraSpeed;
    public GameObject cameraConstraint;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraSpeed = 7.0f;
    }

    void FixedUpdate()
    {
        follow();
    }

    private void follow()
    {
        transform.position = Vector3.Lerp(transform.position, cameraConstraint.transform.position, Time.deltaTime * cameraSpeed);
        transform.LookAt(player.gameObject.transform.position);
    }
}
