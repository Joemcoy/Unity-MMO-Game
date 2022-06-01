using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;

namespace PiMMORPG.WebServer
{
    public static class NancyExtensions
    {
        public static Response AsNJson(this IResponseFormatter formatter, object value)
        {
            return new Response()
            {
                ContentType = "application/json",
                Contents = s =>
                {
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                    var buffer = Encoding.UTF8.GetBytes(data);
                    s.Write(buffer, 0, buffer.Length);
                }
            };
        }

        public static Response WithNJson(this Response response, object value)
        {
            response = response.WithContentType("application/json");
            response.Contents = s =>
            {
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                var buffer = Encoding.UTF8.GetBytes(data);
                s.Write(buffer, 0, buffer.Length);
            };
            return response;
        }
    }
}