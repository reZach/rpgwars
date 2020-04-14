using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingManager : MonoBehaviour
{
    private bool TargetingEnemy;    
    private BaseActor TargetBaseActor;    

    public GameObject TargetedObject;
    public GameObject TargetedObjectHealth;
    public GameObject TargetObjectFill;
    public GameObject TargetObjectName;

    // Start is called before the first frame update
    void Start()
    {
        TargetedObject.SetActive(false);
        TargetedObjectHealth.GetComponent<Slider>().minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetingEnemy)
            TargetedObjectHealth.GetComponent<Slider>().value = TargetBaseActor.GetComponent<MobActor>().Health;
    }

    public void Cancel(int instanceId)
    {
        // Cancel targeting if instance id matches TargetObject
        if (TargetBaseActor != null && TargetBaseActor.GetInstanceID() == instanceId)
        {
            Cancel();
        }
    }

    public void Cancel()
    {
        TargetBaseActor = null;
        TargetingEnemy = false;
        TargetedObject.SetActive(false);
        TargetObjectName.GetComponent<Text>().text = string.Empty;
    }

    public void Target(BaseActor baseActor, bool isEnemy = false)
    {        
        TargetBaseActor = baseActor;
        TargetingEnemy = isEnemy;
        TargetedObject.SetActive(true);
        TargetObjectName.GetComponent<Text>().text = TargetBaseActor.name;
        

        if (isEnemy)
        {
            TargetObjectFill.GetComponent<Image>().color = Color.red;
            TargetedObjectHealth.GetComponent<Slider>().maxValue = TargetBaseActor.GetComponent<MobActor>().MaxHealth;
            TargetedObjectHealth.GetComponent<Slider>().value = TargetBaseActor.GetComponent<MobActor>().Health;
        }            
        else
        {
            TargetObjectFill.GetComponent<Image>().color = Color.green;
            TargetedObjectHealth.GetComponent<Slider>().maxValue = 1;
            TargetedObjectHealth.GetComponent<Slider>().value = 1;
        }            
    }

    public BaseActor MoveTowards()
    {
        return TargetBaseActor;
    }
}
