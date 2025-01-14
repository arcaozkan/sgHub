using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Alien")
        {
            other.transform.Find("Halo").gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Alien")
        {
            other.transform.Find("Halo").gameObject.SetActive(false);

        }
    }

}
