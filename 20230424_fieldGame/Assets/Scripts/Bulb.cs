using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulb : MonoBehaviour
{
    public int num;
    public bool isOn=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetState()
    {
        if (isOn)
        {
            isOn = false;
            this.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
        }

        else if (!isOn)
        {
            isOn = true;
            this.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f);
        }
    }
    public void SetOn()
    {

        isOn = true;
        this.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f);
    }
    public void SetOff()
    {

        isOn = false;
        this.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
    }
}
