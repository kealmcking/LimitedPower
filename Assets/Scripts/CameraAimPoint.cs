using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAimPoint : MonoBehaviour
{
    public GameObject player, aimPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<Player>().isPaused)
        {
            transform.position = (player.transform.position + aimPoint.transform.position) / 2;
        }
        
    }
}
