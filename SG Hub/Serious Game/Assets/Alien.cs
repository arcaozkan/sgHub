using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().gotalien == false)
        {
            //Play sound effect
            //aliencount++
            Destroy(gameObject);



        }


    }

}
