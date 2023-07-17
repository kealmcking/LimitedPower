using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAllExpInSceneItem : MonoBehaviour
{
    public List<GameObject> ExpDropList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {   
            ExpDropList = new List<GameObject>();
            ExpDropList.AddRange(GameObject.FindGameObjectsWithTag("ExpDrop"));

            foreach (GameObject gameObject in ExpDropList)
            {
                gameObject.GetComponent<ExpDrop>().moveTowardPlayer = true;
            }

            Destroy(gameObject);
        }
    }
}
