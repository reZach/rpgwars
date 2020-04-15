using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetingManager : MonoBehaviour
{
    public GameObject TargetUIContainer;
    public GameObject HealthSlider;
    public GameObject CancelTargetControl;

    private WorldActorStats _worldActorStats;

    // Inner HealthSlider components
    private GameObject _background;
    private GameObject _fill;
    private GameObject _name;

    void Start()
    {
        for (int i = 0; i < HealthSlider.transform.childCount; i++)
        {
            if (string.Equals(HealthSlider.transform.GetChild(i).name, "Background", StringComparison.OrdinalIgnoreCase))
            {
                _background = HealthSlider.transform.GetChild(i).gameObject;
            }
            else if (string.Equals(HealthSlider.transform.GetChild(i).name, "Fill Area", StringComparison.OrdinalIgnoreCase))
            {
                _fill = HealthSlider.transform.GetChild(i).gameObject;
            }
            else if (string.Equals(HealthSlider.transform.GetChild(i).name, "ObjectName", StringComparison.OrdinalIgnoreCase))
            {
                _name = HealthSlider.transform.GetChild(i).gameObject;
            }
        }
    }

    void Update()
    {

    }

    /// <summary>
    /// Target a given actor.
    /// </summary>
    /// <param name="gameObject"></param>
    public void Target(GameObject gameObject)
    {
        _worldActorStats = gameObject.GetComponent<WorldActorStats>();

        // Re-activate
        TargetUIContainer.SetActive(false);
        _name.SetActive(true);

        // Update values
        TargetedObjectHealth.GetComponent<Slider>().maxValue = _worldActorStats.MaxHealth;
        TargetedObjectHealth.GetComponent<Slider>().value = _worldActorStats.Health;

        _name.GetComponent<Text>().text = _worldActorStats.Name;
        _fill.GetComponent<Image>().color = Color.red;        
    }

    public void Cancel(Guid? id)
    {
        // If passing in no target or
        // a target that matches the currently targeted WorldActor,
        // then cancel
        if (!id.HasValue || (_worldActorStats != null &&
            id.HasValue &&
            _worldActorStats.Id == id))
        {
            _worldActorStats = null;

            // De-activate
            TargetUIContainer.SetActive(false);
            _name.SetActive(false);
        }
    }

    private bool TargetingEnemy;
    private BaseActor TargetBaseActor;

    public GameObject TargetedObject;
    public GameObject TargetedObjectHealth;
    public GameObject TargetObjectFill;
    public GameObject TargetObjectName;

    // Start is called before the first frame update
    //void Start()
    //{
    //    TargetedObject.SetActive(false);
    //    TargetedObjectHealth.GetComponent<Slider>().minValue = 0;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (TargetingEnemy)
    //        TargetedObjectHealth.GetComponent<Slider>().value = TargetBaseActor.GetComponent<MobActor>().Health;
    //}

    public void Cancel(Guid guid)
    {
        // Cancel targeting if instance id matches TargetObject
        if (TargetBaseActor != null && TargetBaseActor.Guid == guid)
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

    //public void Target(BaseActor baseActor, bool isEnemy = false)
    //{
    //    TargetBaseActor = baseActor;
    //    TargetingEnemy = isEnemy;
    //    TargetedObject.SetActive(true);
    //    TargetObjectName.GetComponent<Text>().text = TargetBaseActor.name;


    //    if (isEnemy)
    //    {
    //        TargetObjectFill.GetComponent<Image>().color = Color.red;
    //        TargetedObjectHealth.GetComponent<Slider>().maxValue = TargetBaseActor.GetComponent<MobActor>().MaxHealth;
    //        TargetedObjectHealth.GetComponent<Slider>().value = TargetBaseActor.GetComponent<MobActor>().Health;
    //    }
    //    else
    //    {
    //        TargetObjectFill.GetComponent<Image>().color = Color.green;
    //        TargetedObjectHealth.GetComponent<Slider>().maxValue = 1;
    //        TargetedObjectHealth.GetComponent<Slider>().value = 1;
    //    }
    //}

    public void Target2(GameObject gameObject)
    {
        //CurrentlyTargeted = gameObject;
        TargetedObject.SetActive(true);
        TargetObjectName.GetComponent<Text>().text = TargetBaseActor.name;

        TargetObjectFill.GetComponent<Image>().color = Color.red;
        TargetedObjectHealth.GetComponent<Slider>().maxValue = TargetBaseActor.GetComponent<MobActor>().MaxHealth;
        TargetedObjectHealth.GetComponent<Slider>().value = TargetBaseActor.GetComponent<MobActor>().Health;
    }

    //public GameObject GetCurrentlyTargeted()
    //{
    //    return CurrentlyTargeted;
    //}

    public BaseActor MoveTowards()
    {
        return TargetBaseActor;
    }
}
