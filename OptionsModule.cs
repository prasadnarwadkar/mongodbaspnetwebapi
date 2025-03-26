using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using ZstdSharp.Unsafe;

namespace MongoDbASPNetWebAPI
{
    public class OptionsModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) =>
            {
                var app = (HttpApplication)sender;

                if (app.Request.HttpMethod == "OPTIONS")
                {
                    app.Response.StatusCode = 200;
                    app.Response.AddHeader("Access-Control-Allow-Headers", "*");
                    app.Response.AddHeader("Access-Control-Allow-Origin", "*");

                    app.Response.AddHeader("Access-Control-Allow-Methods", "*");

                    app.Response.End();
                }
                else
                {
                    app.Response.End();
                }
            };
        }

        public void Dispose()
        {
        }
    }

    public class PreflightRequestsHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Origin") && request.Method.Method.Equals("OPTIONS"))
            {
                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
                // Define and add values to variables: origins, headers, methods (can be global)               
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Headers", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "*");
                response.Headers.Add("Access-Control-Allow-Credentials", "true");
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

    }
}