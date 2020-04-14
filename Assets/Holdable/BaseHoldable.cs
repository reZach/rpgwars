using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHoldable : MonoBehaviour
{
    protected BaseActor BaseActor;

    public bool CanAttack;
    public HoldableType Type = HoldableType.Sword;
    public enum HoldableType
    {
        Sword = 0,
        Axe
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

    public BaseHoldable Cast()
    {
        switch (Type)
        {
            case HoldableType.Sword:
                return (Sword)this;
            case HoldableType.Axe:
                return (Axe)this;
            default:
                return null;
        }
    }
}
