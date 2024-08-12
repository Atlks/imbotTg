//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using System.Threading.Tasks;
//using RestSharp;
//using Newtonsoft.Json.Linq;
//using BinanceAPI;
//using Org.BouncyCastle.Asn1.Crmf;
//namespace mdsj.lib
//{
 

//    namespace HuggingFaceExample
//    {
//        class Program
//        {
//            static async Task Main(string[] args)
//            {
//                var client = new RestClient("https://api-inference.huggingface.co/models/gpt2");
//                var request = new RestRequest(Method.POST);
//                request.AddHeader("Authorization", "Bearer your-huggingface-api-key-here");
//                request.AddHeader("Content-Type", "application/json");
//                request.AddParameter("application/json", "{\"inputs\":\"Hello, world!\"}", ParameterType.RequestBody);

//                IRestResponse response = await client.ExecuteAsync(request);
//               print(response.Content);
//            }
//        }
//    }

//}
