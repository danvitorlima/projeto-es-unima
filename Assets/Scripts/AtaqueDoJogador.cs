using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueDoJogador : MonoBehaviour
{
    [SerializeField]
    private GameObject projetil;
    [SerializeField]
    private float cooldown;
    private float lastAttackTime;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttackTime >= cooldown)
            {
                GetComponent<AudioSource>().Play();
                var bala = Instantiate(projetil, transform.position + transform.up/1.6f, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 90));
                bala.GetComponent<Rigidbody2D>().AddForce(transform.up * 400f);
                lastAttackTime = Time.time;
            }
        }
    }
}
