using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldActorStats : MonoBehaviour
{    
    public Guid Id;
    public Vector3 InitialPosition;
    public string Name;
    public string Title;
    public ushort Health;
    public ushort MaxHealth;
    public byte Energy;
    public byte MaxEnergy;

    // Start is called before the first frame update
    void Start()
    {
        Id = Guid.NewGuid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
