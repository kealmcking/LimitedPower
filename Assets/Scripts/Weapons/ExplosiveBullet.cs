using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    public GameObject circle;

    public float maxRadius, radiusExpansionRate;

    void Update() {
        Vector3 localScale = circle.transform.localScale;
        if (circle.transform.localScale.x < maxRadius && circle.transform.localScale.y < maxRadius) {
            localScale.x += radiusExpansionRate * Time.deltaTime;
            localScale.y += radiusExpansionRate * Time.deltaTime;
            circle.transform.localScale = localScale;
        } else if (circle.transform.localScale.x > maxRadius && circle.transform.localScale.y > maxRadius) {
            localScale.x = maxRadius;
            localScale.y = maxRadius;
            circle.transform.localScale = localScale;
        }
    }



}
