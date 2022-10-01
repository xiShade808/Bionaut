using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private float distance = 1f;
    void Update()
    {
        Vector3 back = player.transform.forward;
        back.y = 0.5f;
        transform.position = player.transform.position - back * distance;

        transform.forward = player.transform.position - transform.position;
    }
}
