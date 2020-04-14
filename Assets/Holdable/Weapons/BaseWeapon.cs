using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : BaseHoldable
{
    protected bool Attacking;
    protected float AttackSpeed;
    protected float AttackSpeedTimer;
    protected ushort AttackDamage;

    protected void BaseWeaponInitialize()
    {
        base.BaseHoldableInitialize();
    }

    protected void BaseWeaponUpdate()
    {
        if (CanAttack)
            AttackSpeedTimer += Time.deltaTime;
        
        if (AttackSpeedTimer > AttackSpeed)
        {            
            AttackSpeedTimer = 0f;
            base.BaseActor.DealDamageToSingleTarget(AttackDamage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
