using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{

    [SerializeField] float countdown;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf(countdown));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroySelf(float timeTillDestruction) { 
        yield return new WaitForSeconds(timeTillDestruction);
        Destroy(this.gameObject);
    }
}
