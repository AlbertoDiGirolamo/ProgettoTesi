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

public class textManagement : MonoBehaviour
{
    public enum objectType { TextMeshPro = 0, TextMeshProUGUI = 1 };

    public objectType ObjectType;
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
        m_text.fontSize = 2;

        // Set the text
        m_text.text = "A <#0080ff>simple</color> line of text.";

        // Get the preferred width and height based on the supplied width and height as opposed to the actual size of the current text container.
        Vector2 size = m_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

        // Set the size of the RectTransform based on the new calculated values.
        m_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);


    }

    async Task Update()
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
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiIwYjA3ZjQyOS05ZjRiLTQ3MTQtOTM5Mi1jYzVlOGU4MGM4YjAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9lOTk2NDdkYy0xYjA4LTQ1NGEtYmY4Yy02OTkxODFiMzg5YWIvIiwiaWF0IjoxNjYzMDYyNDU3LCJuYmYiOjE2NjMwNjI0NTcsImV4cCI6MTY2MzA2Njk4NSwiYWNyIjoiMSIsImFpbyI6IkFUUUF5LzhUQUFBQVA0STNCQTdiL0ZwdFRob2lvTndWWFJQbXlqK0k0RE8yZlYwLzZ2SFNWeUR3TjJiWWJFSnlHeFFhTzBmY3VQTGwiLCJhbXIiOlsid2lhIl0sImFwcGlkIjoiMDRiMDc3OTUtOGRkYi00NjFhLWJiZWUtMDJmOWUxYmY3YjQ2IiwiYXBwaWRhY3IiOiIwIiwiZmFtaWx5X25hbWUiOiJEaSBHaXJvbGFtbyIsImdpdmVuX25hbWUiOiJBbGJlcnRvIiwiaXBhZGRyIjoiMTM3LjIwNC4xMDcuMTEzIiwibmFtZSI6IkFsYmVydG8gRGkgR2lyb2xhbW8gLSBhbGJlcnRvLmRpZ2lyb2xhbW8yQHN0dWRpby51bmliby5pdCIsIm9pZCI6ImJhMTMwODZlLTBkNTEtNGFhMy1hZjI0LTBkNjA5ODM0ZTdjNCIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS03OTA1MjU0NzgtMTAzNTUyNTQ0NC02ODIwMDMzMzAtMTcyNjEyMyIsInB1aWQiOiIxMDAzMjAwMDNEQjExQzk0IiwicmgiOiIwLkFRVUEzRWVXNlFnYlNrV19qR21SZ2JPSnF5bjBCd3RMbnhSSGs1TE1YbzZBeUxBRkFHdy4iLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiJNR1ZEQ0hHTmdaelkxNFMzSTREWG82OXl1OTN2Q0VBaDI2RXZPV0V5UC1nIiwidGlkIjoiZTk5NjQ3ZGMtMWIwOC00NTRhLWJmOGMtNjk5MTgxYjM4OWFiIiwidW5pcXVlX25hbWUiOiJhbGJlcnRvLmRpZ2lyb2xhbW8yQHN0dWRpby51bmliby5pdCIsInVwbiI6ImFsYmVydG8uZGlnaXJvbGFtbzJAc3R1ZGlvLnVuaWJvLml0IiwidXRpIjoiUzM5ZVdsd0dja2E5bTJQZzJWTFBBQSIsInZlciI6IjEuMCJ9.E-woFF-tYdzoBWhjaU-ib4KpJOBFnBpP255g0x8m9tb0Am5t4bSVpxRpbhyY-7g40u3EPnXHSWl8TQ2mVJ8fho275_rnJ-je3NZKIemwGu5qnOOCsLNaf99k2iKkBu0RznrXL8K1esyw-t92u6I0o14UOD90jngwtC5Z67VlTd1_ldBKYOLG4g3q_16R7DVAgKKKfYBGhwZHMxk8sSKHwfsl7M4N6SgV4UwQz-BeT5N2Q2xnXoKxAVseQCfmXZfABBH1uhRQb7aOP7gcqzg5scEwC3MiRo3bH_2EvyED902FsAIwyGMeJSjIDQ_KyzfP-1RTzMBXpoWllDhNKkkcGg");


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

        //InvokeRepeating("doPost", 2, 2);

    }




}
