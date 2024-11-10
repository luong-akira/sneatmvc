using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sneat.MVC.Models.DTO.Bank
{
    public class BankVietQROutputModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bin")]
        public string Bin { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("transferSupported")]
        public int TransferSupported { get; set; }

        [JsonProperty("lookupSupported")]
        public int LookupSupported { get; set; }

        [JsonProperty("swift_code")]
        public string SwiftCode { get; set; }

        public string DisplayValue { get; set; }
    }

    public class BankApiResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("data")]
        public List<BankVietQROutputModel> Data { get; set; }
    }
}