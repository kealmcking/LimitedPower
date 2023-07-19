using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int health;


    bool hasMultiplePhases;

    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !hasMultiplePhases) {
            enemy.Die();
        }
    }
}
