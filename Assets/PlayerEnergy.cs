using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    public GameObject EnergyDisplay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(byte energy)
    {
        EnergyDisplay.GetComponent<Text>().text = $"{energy}";
    }
}
