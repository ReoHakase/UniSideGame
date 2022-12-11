using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public GameObject subScreen;

    public bool shouldForceScroll = false;
    public float forceScrollSpeedX = 0.4f; // per second
    public float forceScrollSpeedY = 0.0f; // per second

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 cameraPosition = transform.position;
            
            if(shouldForceScroll){ // Force the camera to scroll not following the player
                cameraPosition.x += forceScrollSpeedX * Time.deltaTime;
                cameraPosition.y += forceScrollSpeedY * Time.deltaTime;
            } else { // Follow the player
                cameraPosition.x = Mathf.Clamp(playerPosition.x, leftLimit, rightLimit);
                cameraPosition.y = Mathf.Clamp(playerPosition.y, bottomLimit, topLimit);
            }

            transform.position = cameraPosition;

            if(subScreen != null){
                subScreen.transform.position = new Vector3(cameraPosition.x / 2.0f, subScreen.transform.position.y , subScreen.transform.position.z);
            }
        }

        
    }
}
