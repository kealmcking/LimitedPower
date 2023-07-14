using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroySelf");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }
    
}
