using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showHologram : MonoBehaviour
{

    private GameObject m_text;
    bool isVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        m_text = GameObject.Find("MultiParameterMonitor");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showHologramMethod()
    {
        if (isVisible)
        {
            isVisible = false;
            m_text.SetActive(false);
        }
        else {
            isVisible = true;
            m_text.SetActive(true);
        }
       
    }

    

}
