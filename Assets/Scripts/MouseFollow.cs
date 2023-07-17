using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    [SerializeField] float maxDistFromPlayer;
    GameObject player;
    PlayerInput input;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        input = player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(input.lookInput);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

        Vector3 playerToCursor = mousePosition - player.transform.position;
        Vector3 dir = playerToCursor.normalized;
        Vector3 cursorVector = dir * maxDistFromPlayer;
        Vector3 finalPos = player.transform.position + cursorVector;

        transform.position = finalPos;
    }
}
