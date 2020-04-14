using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHoldable : MonoBehaviour
{
    protected BaseActor BaseActor;

    public bool CanAttack;
    public HoldableType Type = HoldableType.Weapon;
    public enum HoldableType
    {
        Weapon = 0
    };

    protected void BaseHoldableInitialize()
    {
        BaseActor = GetComponentInParent<BaseActor>();
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
