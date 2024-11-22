using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtaqueDoInimigo : AtaqueRanged
{

    protected override void Start()
    {
        base.Start();
        lastAttackTime = cooldown + 1;
    }

    void Update()
    {
        if (Time.time - lastAttackTime >= cooldown && GetComponent<PatrulhaInimigo>().patrulha == false)
        {
            lastAttackTime = Time.time;
            Atirar();
        }
    }
}
