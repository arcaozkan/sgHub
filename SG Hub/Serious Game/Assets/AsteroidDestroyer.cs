using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDestroyer : MonoBehaviour
{
    public GameObject player;
    public GameObject spaceship;
    private Vector3 temppos = new Vector3();
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Alien" && firstPartData.OnSecondPart ==false )
        {
            
            temppos = collision.gameObject.transform.position;
            //Debug.Log(collision.gameObject.transform.position.ToString());
            Destroy(collision.gameObject);
            temppos.x = player.transform.position.x;

            if (player.GetComponent<PlayerController>().gotalien == false)
            {
                firstPartData.firstMissedAlienPositions.Add(collision.gameObject.transform.position.ToString());
                player.GetComponent<PlayerController>().motionTimer = 0;
                foreach (var x in HandTracking.allJerks) //CASE 3:Alien missed
                {
                    if (x < firstPartData.smoothness_threshold)
                    {
                        firstPartData.smooth_motions += 1;
                    }
                }
                float ratio = (float)firstPartData.smooth_motions / HandTracking.allJerks.Count;
                float cratio = Vector3.Distance(player.transform.position, temppos) / Vector3.Distance(spaceship.transform.position, temppos);
                if (cratio > 1)
                    cratio = 1f;
                cratio = 0.5f - (cratio/2);
                firstPartData.completeness_ratios.Add(cratio);
                firstPartData.smoothness_ratios.Add(ratio);
                //Debug.Log("Smoothness ratio:");
                //Debug.Log(ratio);
                
               // Debug.Log("Completeness ratio:");
                //Debug.Log(cratio);
                //Reset for the next motion
                firstPartData.smooth_motions = 0;
                HandTracking.allJerks.Clear();
                HandTracking.allAccels.Clear();
                HandTracking.allVelocities.Clear();
                HandTracking.allCoordinates.Clear();

                List<float> tempStats = new List<float>();
                tempStats.Add(ratio);
                tempStats.Add(cratio);
                //tempStats.Add(-1);
                //tempStats.Add(-1);
                firstPartData.firstMissedAlienStats.Add(tempStats);
                //actionstartpoint = Player.transform.position; BUNU DÜÞÜNMEK LAZIM
            }
        }
        if (collision.gameObject.tag == "Asteroid")
        {
            Destroy(collision.gameObject.transform.parent.gameObject);

        }
    }
}
