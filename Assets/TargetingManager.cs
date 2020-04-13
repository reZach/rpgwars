using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingManager : MonoBehaviour
{
    private bool TargetingEnemy;    
    private GameObject TargetObject;    

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
            TargetedObjectHealth.GetComponent<Slider>().value = TargetObject.GetComponent<Mob1>().Health;
    }

    public void Cancel()
    {
        TargetObject = null;
        TargetingEnemy = false;
        TargetedObject.SetActive(false);
        TargetObjectName.GetComponent<Text>().text = string.Empty;
    }

    public void Target(GameObject gameObject, bool isEnemy = false)
    {        
        TargetObject = gameObject;
        TargetingEnemy = isEnemy;
        TargetedObject.SetActive(true);
        TargetObjectName.GetComponent<Text>().text = TargetObject.name;
        

        if (isEnemy)
        {
            TargetObjectFill.GetComponent<Image>().color = Color.red;
            TargetedObjectHealth.GetComponent<Slider>().maxValue = TargetObject.GetComponent<Mob1>().Health;
            TargetedObjectHealth.GetComponent<Slider>().value = TargetObject.GetComponent<Mob1>().Health;
        }            
        else
        {
            TargetObjectFill.GetComponent<Image>().color = Color.green;
            TargetedObjectHealth.GetComponent<Slider>().maxValue = 1;
            TargetedObjectHealth.GetComponent<Slider>().value = 1;
        }            
    }

    public GameObject MoveTowards()
    {
        return TargetObject;
    }
}
