using System.Net;
using System.Net.Http;

namespace CRM.Extend.HttpResponseMessages
{
    public class PrimaryRepeatedMessge : HttpResponseMessage
    {
        public PrimaryRepeatedMessge()
        {
            StatusCode = HttpStatusCode.NotImplemented;
            ReasonPhrase = "Primary Repeate";
        }
    }
}