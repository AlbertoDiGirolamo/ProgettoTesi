using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System;

public class showHologram : MonoBehaviour
{

    private TMP_Text m_text;

    public GameObject txt;

    // Start is called before the first frame update
    void Start()
    {


    }


    void Update()
    {

    }

    public void whenButtonClicked()
    {
        if (txt.activeInHierarchy == true)
        {
            txt.SetActive(false);
            update = false;
        }
        else
        {
            txt.SetActive(true);
        }
    }

    bool update = false;
    public bool isUpdate()
    {
        return update;
    }
    public void disableUpdate()
    {
        update = true;
    }


}
