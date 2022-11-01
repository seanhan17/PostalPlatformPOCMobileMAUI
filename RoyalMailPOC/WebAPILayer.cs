using System;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace RoyalMailPOC
{
	public class WebAPILayer
	{
        private HttpClient client = new HttpClient();
        private WebClient clientProxy;

        public WebAPILayer()
        {
            SetUpAPIClient();
            SetUpProxy();
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                  ((sender, certificate, chain, sslPolicyErrors) => true);
        }

        protected void SetUpAPIClient()
        {
            try
            {
                //string URI = "https://tnspostalplatformwebapi-sit.tnsglobal.com/api/HandScanner/";
                string URI = "http://192.168.0.238:49909/api/HandScannerV2/";
                client.BaseAddress = new Uri(URI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string guidToken = "5CBB5E0B-FAD4-448E-BD59-AB79E21ED3BC";
                client.DefaultRequestHeaders.Add("Authentication", guidToken);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void SetUpProxy()
        {

            string ProxyURL = "http://kthydvir701.kt.group.local/wpad.dat";
            clientProxy = new WebClient()
            {
                Proxy = new WebProxy(ProxyURL)
                {
                    Credentials = CredentialCache.DefaultCredentials
                }
            };
        }

        public async Task<ScannerResponseModels> AssociateRFID(int id, string returnDataItemId, string returnDataRFIDId)
        {
            string UriAPI = "AssociateRfid";

            ScannerModels scannerInfo = new ScannerModels();
            scannerInfo.ItemId = Convert.ToInt64(returnDataItemId);
            scannerInfo.UserId = id;
            scannerInfo.RFIDId = Convert.ToInt64(returnDataRFIDId);
            scannerInfo.ClientID = 1;
            HttpResponseMessage response = client.PostAsJsonAsync(UriAPI, scannerInfo).Result;
            response.EnsureSuccessStatusCode();
            var ReturnObj = JsonSerializer.Deserialize<ScannerResponseModels>(response.Content.ReadAsStringAsync().Result);

            return ReturnObj;
        }


        public class ScannerModels
        {
            public ScannerModels()
            {
                this.isPostalPlatform = true;
            }
            /// <summary>
            /// Scanner ID
            /// </summary>
            public int ScannerId { get; set; }
            /// <summary>
            /// UserID that is log on to the scanner
            /// </summary>
            public int UserId { get; set; }
            /// <summary>
            /// The Item ID to be associated
            /// </summary>
            public long ItemId { get; set; }
            /// <summary>
            /// The RFIDID that is used for the action
            /// </summary>
            public long RFIDId { get; set; }
            /// <summary>
            /// The Panellists involved in the action
            /// </summary>
            public int PanellistID { get; set; }
            /// <summary>
            /// ClientID
            /// </summary>
            public int ClientID { get; set; }
            /// <summary>
            /// Box ID for failed CT RFID (FTLRS)
            /// </summary>
            public int? BoxID { get; set; }
            /// <summary>
            /// RFIDType for RFID
            /// </summary>
            public string RFIDTypeID { get; set; }
            /// <summary>
            /// RFID From for RFID
            /// </summary>
            public string RFIDFrom { get; set; }
            /// <summary>
            /// RFID From for RFID
            /// </summary>
            public bool isPostalPlatform { get; set; }
            /// <summary>
            /// RFID Model ID for RFID
            /// </summary>
            public int? RFIDModelId { get; set; }
            /// <summary>
            /// Receiver Item Number 
            /// </summary>
            public int? ReceiverItemNumber { get; set; }
            /// <summary>
            /// Is Item ID
            /// </summary>
            public bool IsItemID { get; set; }
            /// <summary>
            /// RFIDID Start id
            /// </summary>
            public long RFIDStartId { get; set; }
            /// <summary>
            /// RFIDID end id
            /// </summary>
            public long RFIDEndId { get; set; }
            /// <summary>
            /// Reference Id Prefix
            /// </summary>
            public int RefIdPrefix { get; set; }
            /// <summary>
            /// Reference Id
            /// </summary>
            public long ReferenceId { get; set; }
            /// <summary>
            /// RefId Id(used for deutsche post client)
            /// same as referenceid but will be passed as string
            /// </summary>
            public string RefId { get; set; }
        }

        public class ScannerResponseModels
        {
            public string ResultMessage { get; set; }
            public object Data { get; set; }
            public object ResultCode { get; set; }
        }

        public class RFIDModel
        {
            public enum RFIDModelList : int
            {
                Active = 1,
                Passive = 2,
                Duel = 3
            }

            public static int getRFIDModelID(string Model)
            {
                int modelID = 0;
                if (Model.ToUpper() == "ACTIVE")
                {
                    modelID = Convert.ToInt32(RFIDModelList.Active);
                }
                else if (Model.ToUpper() == "PASSIVE")
                {
                    modelID = Convert.ToInt32(RFIDModelList.Passive);
                }
                else if (Model.ToUpper() == "DUEL")
                {
                    modelID = Convert.ToInt32(RFIDModelList.Duel);
                }

                return modelID;
            }

        }
    }
}

