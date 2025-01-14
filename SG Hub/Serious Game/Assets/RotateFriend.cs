using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFriend : MonoBehaviour
{
    public float speed = 70.0f;
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.left, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }
}
