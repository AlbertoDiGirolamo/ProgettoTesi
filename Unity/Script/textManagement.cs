using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

using System;

public class textManagement : MonoBehaviour
{
    public enum objectType { TextMeshPro = 0, TextMeshProUGUI = 1 };

    public objectType ObjectType;

    public showHologram btn;
    public bool isStatic;

    private TMP_Text m_text;

    //private TMP_InputField m_inputfield;


    private const string k_label = "The count is <#0080ff>{0}</color>";
    private int count;

    string URL;
    Dictionary<string, string> headers;
    byte[] postData;


    void Awake()
    {
        // Get a reference to the TMP text component if one already exists otherwise add one.
        // This example show the convenience of having both TMP components derive from TMP_Text. 
        if (ObjectType == 0)
            m_text = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
        else
            m_text = GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();

        // Load a new font asset and assign it to the text object.
        m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");

        // Load a new material preset which was created with the context menu duplicate.
        m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");

        // Set the size of the font.
        m_text.fontSize = 1;

        // Set the text
        m_text.text = "A <#0080ff>simple</color> line of text.";

        // Get the preferred width and height based on the supplied width and height as opposed to the actual size of the current text container.
        Vector2 size = m_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

        // Set the size of the RectTransform based on the new calculated values.
        m_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);



        // Get the first Mesh Observer available, generally we have only one registered
        var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();

        // Suspends observation of spatial mesh data
        observer.Suspend();


        btn = FindObjectOfType<showHologram>();
        StartCoroutine(dataSimulationUpdate());

    }

    bool update = false;
    void Update()
    {
        if (btn.isUpdate() == false)
        {
            StartCoroutine(dataSimulationUpdate());
            btn.disableUpdate();
        }


    }



    int pr = 65;
    int modpr = 1;
    int modrr = 1;
    double temp = 36.5;
    int rr = 28;
    int spo2 = 99;


    public IEnumerator dataSimulationUpdate()
    {
        while (true)
        {
            if (pr > 68 || pr < 62)
            {
                modpr = -modpr;
                temp = temp + 0.3;

            }
            if (rr > 31 || rr < 26)
            {
                modrr = -modrr;
                temp = temp - 0.2;
            }
            rr = rr + modrr;
            pr = pr + modpr;

            m_text.SetText("PR: " + pr +
                          "\nRR: " + rr
                          + "\nSPO2: " + spo2
                          + "\nTEMP: " + Math.Round(temp, 1));

            yield return new WaitForSeconds(1);
        }

    }


    async Task dataUpdate()
    {
        string RR = "";
        string PR = "";
        string SPO2 = "";
        string TEMP = "";

        string json = @"{'query':'SELECT * FROM DIGITALTWINS'}";
        json = json.Replace("'", "\"");

        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "https://multiParameterMonitorTwin.api.wcus.digitaltwins.azure.net/query?api-version=2020-10-31";
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiIwYjA3ZjQyOS05ZjRiLTQ3MTQtOTM5Mi1jYzVlOGU4MGM4YjAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9lOTk2NDdkYy0xYjA4LTQ1NGEtYmY4Yy02OTkxODFiMzg5YWIvIiwiaWF0IjoxNjYzMTM5NzAzLCJuYmYiOjE2NjMxMzk3MDMsImV4cCI6MTY2MzE0NDgyMywiYWNyIjoiMSIsImFpbyI6IkFUUUF5LzhUQUFBQUo4RUhjTjNvYzJOZnEyY2cvdmNXang4dEh0dHZLbjJCUFhwM3gyaXhRSkZYSDkrcGd0enhWWFNOL1BFdFlWTEIiLCJhbXIiOlsid2lhIl0sImFwcGlkIjoiMDRiMDc3OTUtOGRkYi00NjFhLWJiZWUtMDJmOWUxYmY3YjQ2IiwiYXBwaWRhY3IiOiIwIiwiZmFtaWx5X25hbWUiOiJEaSBHaXJvbGFtbyIsImdpdmVuX25hbWUiOiJBbGJlcnRvIiwiaXBhZGRyIjoiMTM3LjIwNC4xMDcuMTEzIiwibmFtZSI6IkFsYmVydG8gRGkgR2lyb2xhbW8gLSBhbGJlcnRvLmRpZ2lyb2xhbW8yQHN0dWRpby51bmliby5pdCIsIm9pZCI6ImJhMTMwODZlLTBkNTEtNGFhMy1hZjI0LTBkNjA5ODM0ZTdjNCIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS03OTA1MjU0NzgtMTAzNTUyNTQ0NC02ODIwMDMzMzAtMTcyNjEyMyIsInB1aWQiOiIxMDAzMjAwMDNEQjExQzk0IiwicmgiOiIwLkFRVUEzRWVXNlFnYlNrV19qR21SZ2JPSnF5bjBCd3RMbnhSSGs1TE1YbzZBeUxBRkFHdy4iLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiJNR1ZEQ0hHTmdaelkxNFMzSTREWG82OXl1OTN2Q0VBaDI2RXZPV0V5UC1nIiwidGlkIjoiZTk5NjQ3ZGMtMWIwOC00NTRhLWJmOGMtNjk5MTgxYjM4OWFiIiwidW5pcXVlX25hbWUiOiJhbGJlcnRvLmRpZ2lyb2xhbW8yQHN0dWRpby51bmliby5pdCIsInVwbiI6ImFsYmVydG8uZGlnaXJvbGFtbzJAc3R1ZGlvLnVuaWJvLml0IiwidXRpIjoiMDQxdmhlRFVLRUs0VUxQcGprSU1BQSIsInZlciI6IjEuMCJ9.tIBBIXqd0lSxHNBgAvroGTiR7h-6wd6K5gefls0sBqy4naF165OR5LmLPPbJ80SuKVtJbsStLPH4vUMwZyqsx_8LfxBof70B0j6q-HIzX-vSxL_yQOB-rIQ8thqZF4Z71xTNMVuLPqNqG2R0LAATmrb3yaLXf31TuOu4L3FdUJ_PGG_ZhdJcaZj1VHrE7BoDoinTfbV_rPhnJ8Lvw6IZ-1xRGE9Kc0IECcmXiOiWr3wu8xds3C7marUUjI0cFM8llHJ-LiY4Nk2ELYgqs114ge1bLddl_u_LICVYzGmv2Of1LLrq-pctYkcscBcEaQijPC94G8wvTGj1vJBtPNqpZg");


        var response = await client.PostAsync(url, data);

        var result = await response.Content.ReadAsStringAsync();
        string[] dataValue = result.Split(",");
        for (int i = 0; i < dataValue.Length; i++)
        {
            if (dataValue[i].Contains("PR"))
            {
                PR = dataValue[i].Split(':')[1];

                break;
            }
        }
        for (int i = 0; i < dataValue.Length; i++)
        {
            if (dataValue[i].Contains("RR"))
            {
                RR = dataValue[i].Split(':')[1];

                break;
            }
        }
        for (int i = 0; i < dataValue.Length; i++)
        {
            if (dataValue[i].Contains("SPO2"))
            {
                SPO2 = dataValue[i].Split(':')[1];

                break;
            }
        }
        for (int i = 0; i < dataValue.Length; i++)
        {
            if (dataValue[i].Contains("TEMP"))
            {
                TEMP = dataValue[i].Split(':')[1];

                break;
            }
        }
        m_text.SetText("PR: " + PR +
                        "\nRR: " + RR
                        + "\nSPO2: " + SPO2
                        + "\nTEMP: " + TEMP);
    }

}
