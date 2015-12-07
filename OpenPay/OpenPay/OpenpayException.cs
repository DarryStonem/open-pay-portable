using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace OpenPay
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OpenpayException : Exception
    {
        [JsonConstructor]
        internal OpenpayException()
        {
        }

        internal static OpenpayException GetFromJSON(HttpStatusCode code, string json)
        {
            OpenpayException result = JsonConvert.DeserializeObject<OpenpayException>(json);
            result.StatusCode = code;
            return result;
        }

        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }

        [JsonProperty(PropertyName = "category")]
        public String Category { get; set; }

        [JsonProperty(PropertyName = "request_id")]
        public String RequestId { get; set; }

        [JsonProperty(PropertyName = "error_code")]
        public int ErrorCode { get; set; }

        public HttpStatusCode StatusCode { get; internal set; }

        public override string Message
        {
            get
            {
                return this.ErrorCode + ": " + this.Description;
            }
        }
    }
}
