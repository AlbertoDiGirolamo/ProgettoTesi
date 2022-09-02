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

    private WWW www;
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
        m_text.fontSize = 250;

        // Set the text
        m_text.text = "A <#0080ff>simple</color> line of text.";

        // Get the preferred width and height based on the supplied width and height as opposed to the actual size of the current text container.
        Vector2 size = m_text.GetPreferredValues(Mathf.Infinity, Mathf.Infinity);

        // Set the size of the RectTransform based on the new calculated values.
        m_text.rectTransform.sizeDelta = new Vector2(size.x, size.y);





        
       
    }

    async Task Update()
    {


        string json = @"{'query':'SELECT * FROM DIGITALTWINS'}";
        json = json.Replace("'", "\"");

        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var url = "https://Example2DT.api.wcus.digitaltwins.azure.net/query?api-version=2020-10-31";
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiIwYjA3ZjQyOS05ZjRiLTQ3MTQtOTM5Mi1jYzVlOGU4MGM4YjAiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9lOTk2NDdkYy0xYjA4LTQ1NGEtYmY4Yy02OTkxODFiMzg5YWIvIiwiaWF0IjoxNjYxOTUxNTE2LCJuYmYiOjE2NjE5NTE1MTYsImV4cCI6MTY2MTk1NTc0MiwiYWNyIjoiMSIsImFpbyI6IkFUUUF5LzhUQUFBQTlCU2g4cmRPWEhJTjE0ZFpLR2RocmVCWFA3SzB3QWhmdm1sWTJ4eXZUYWxndXpHUGpOTnIwUlNmWEJnL3FHYWUiLCJhbXIiOlsicHdkIl0sImFwcGlkIjoiMDRiMDc3OTUtOGRkYi00NjFhLWJiZWUtMDJmOWUxYmY3YjQ2IiwiYXBwaWRhY3IiOiIwIiwiZmFtaWx5X25hbWUiOiJEaSBHaXJvbGFtbyIsImdpdmVuX25hbWUiOiJBbGJlcnRvIiwiaXBhZGRyIjoiODcuNy4yNDIuMTMwIiwibmFtZSI6IkFsYmVydG8gRGkgR2lyb2xhbW8gLSBhbGJlcnRvLmRpZ2lyb2xhbW8yQHN0dWRpby51bmliby5pdCIsIm9pZCI6ImJhMTMwODZlLTBkNTEtNGFhMy1hZjI0LTBkNjA5ODM0ZTdjNCIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS03OTA1MjU0NzgtMTAzNTUyNTQ0NC02ODIwMDMzMzAtMTcyNjEyMyIsInB1aWQiOiIxMDAzMjAwMDNEQjExQzk0IiwicmgiOiIwLkFRVUEzRWVXNlFnYlNrV19qR21SZ2JPSnF5bjBCd3RMbnhSSGs1TE1YbzZBeUxBRkFHdy4iLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiJNR1ZEQ0hHTmdaelkxNFMzSTREWG82OXl1OTN2Q0VBaDI2RXZPV0V5UC1nIiwidGlkIjoiZTk5NjQ3ZGMtMWIwOC00NTRhLWJmOGMtNjk5MTgxYjM4OWFiIiwidW5pcXVlX25hbWUiOiJhbGJlcnRvLmRpZ2lyb2xhbW8yQHN0dWRpby51bmliby5pdCIsInVwbiI6ImFsYmVydG8uZGlnaXJvbGFtbzJAc3R1ZGlvLnVuaWJvLml0IiwidXRpIjoieFVUeDRhZlI5a3F1QzBVRkxKa2ZBQSIsInZlciI6IjEuMCJ9.CyVKnAno5PhElpDl5Swkg8BB1VUzUGf7ztNW8oeC5arHJqGJYqnPe1XIP9vVbouiPS_u69WFTxAm89wfrUJ-Qw7Qy_fOH4OpD8d6U1QMj6PWPCYs6MKNjF76gkfAOH0qBH1i5aSXhxavqTOAfP-E3VkDND-bv6oB7fhmCxmMKniwL8Ct5IsXPK_HBXuY4TqhDHWkEIWy2vF5rH2PETQ6Jb9Yw2Rq2C_-6Ic6oBmP2-zG4Z-gogqpvCesKUT47v-jUc7hCJe-eMdPgCXBjSwTMlpCa4ord46M5RFPP0m3TIho2zmVBo974Q-tTCd1tRQ5f4eJvPDM7RgT6Z-OKERh7A");


        var response = await client.PostAsync(url, data);

        var result = await response.Content.ReadAsStringAsync();
        string[] dataValue = result.Split(",");
        for (int i = 0; i < dataValue.Length; i++)
        {
            if (dataValue[i].Contains("Temperature"))
            {

                m_text.SetText(dataValue[i]);
                break;
            }
        }
        

        //InvokeRepeating("doPost", 2, 2);

    }




}
