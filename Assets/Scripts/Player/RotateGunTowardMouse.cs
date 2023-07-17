using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGunTowardMouse : MonoBehaviour
{

    [SerializeField] GameObject aimCursor;
    float speed = 1.0f;

    Player player;

    Vector3 targetDirection;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        aimCursor = GameObject.Find("AimPoint");
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
    }

    void RotateGun() {
        if (player.isPaused || player.isDead) return;
        Vector3 mousePos = aimCursor.transform.position;
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0, rotZ);
    }

    public void PlayFireAnim() {
        animator.SetTrigger("hasFired");
    }
}
