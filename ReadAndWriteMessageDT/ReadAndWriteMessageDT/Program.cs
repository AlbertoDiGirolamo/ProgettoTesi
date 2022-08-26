using System;
// <Azure_Digital_Twins_dependencies>
using Azure.DigitalTwins.Core;
using Azure.Identity;
// </Azure_Digital_Twins_dependencies>
// <Model_dependencies>
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Azure;
// </Model_dependencies>
// <Query_dependencies>
using System.Text.Json;
// </Query_dependencies>

namespace ReadAndWriteMessageDT
{
    class DigitalTwinsClientAppSample
    {
        // <Async_signature>
        static async Task Main(string[] args)
        {
            // </Async_signature>
            // <Authentication_code>
            string adtInstanceUrl = "https://Example2DT.api.wcus.digitaltwins.azure.net";

            var credential = new DefaultAzureCredential();
            var client = new DigitalTwinsClient(new Uri(adtInstanceUrl), credential);
            Console.WriteLine($"Service client created – ready to go");
            // </Authentication_code>

            // <Query_twins>
            // Run a query for all twins
            string query = "SELECT * FROM digitaltwins";
            AsyncPageable<BasicDigitalTwin> queryResult = client.QueryAsync<BasicDigitalTwin>(query);


            //READ VALUE PROPERTY
            await foreach (BasicDigitalTwin twin in queryResult)
            {


                Console.WriteLine(JsonSerializer.Serialize(twin));

                Console.WriteLine("---------------");
                string[] subs = JsonSerializer.Serialize(twin).Split(",");
                for (int i = 0; i < subs.Length; i++)
                {
                    if (subs[i].Contains("Temperature"))
                    {
                        Console.WriteLine(subs[i]);
                    }
                }

                //Console.WriteLine(parts["$dtId"]);

            }
            // construct your json telemetry payload by hand.

            //SET VALUE PROPERTY
            var updateTwinData = new JsonPatchDocument();
            updateTwinData.AppendAdd("/Temperature", 35.0);


            await client.UpdateDigitalTwinAsync("exampleTwin", updateTwinData).ConfigureAwait(false);


        }

    }
}