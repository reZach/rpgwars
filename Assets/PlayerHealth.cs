using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject HealthDisplay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(ushort health)
    {
        HealthDisplay.GetComponent<Text>().text = $"{health}";
    }
}
