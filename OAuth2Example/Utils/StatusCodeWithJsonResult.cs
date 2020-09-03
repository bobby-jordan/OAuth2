using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace OAuth2Example.Utils
{
    // Returns an arbitrary status code accompanied by a given object, serialized to JSON
    public class StatusCodeWithJsonResult : IHttpActionResult
    {
        private int statusCode;
        private object result;
        private HttpRequestMessage request;

        public StatusCodeWithJsonResult(int statusCode, object result, HttpRequestMessage request)
        {
            this.statusCode = statusCode;
            this.result = result;
            this.request = request;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage message = new HttpResponseMessage((System.Net.HttpStatusCode)statusCode);

            try
            {
                Encoding encoding = new UTF8Encoding(false, true);

                JsonSerializer serializer = JsonSerializer.CreateDefault();

                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter textWriter = new StreamWriter(stream, encoding, 1024, true))
                    {
                        using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter))
                        {
                            serializer.Serialize(jsonWriter, result);
                            await jsonWriter.FlushAsync();
                        }
                    }

                    ArraySegment<byte> arraySeg = new ArraySegment<byte>(stream.GetBuffer(), 0, (int)stream.Length);
                    message.Content = new ByteArrayContent(arraySeg.Array, arraySeg.Offset, arraySeg.Count);

                    MediaTypeHeaderValue header = new MediaTypeHeaderValue("application/json");
                    header.CharSet = encoding.WebName;

                    message.Content.Headers.ContentType = header;
                    message.RequestMessage = request;
                }
            }
            catch
            {
                message.Dispose();
                throw;
            }

            return message;
        }
    }
}