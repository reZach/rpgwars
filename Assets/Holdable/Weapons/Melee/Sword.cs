using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BaseWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        base.BaseWeaponInitialize();

        AttackSpeed = 2f;
        AttackDamage = 8;
    }

    // Update is called once per frame
    void Update()
    {
        base.BaseWeaponUpdate();
    }
}
