using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace MyApplication;

public static class HttpResponseExtensions
{
    internal static HttpResponseData OKResponse(this HttpRequestData req, string payload = "")
    {
        var response = req.CreateResponse();
        response.StatusCode = HttpStatusCode.OK;
        return response.FormalizeResponse(payload);
    }

    internal static HttpResponseData ServerErrorResponse(this HttpRequestData req, string payload = "")
    {
        var response = req.CreateResponse();
        response.StatusCode = HttpStatusCode.InternalServerError;
        return response.FormalizeResponse(payload);
    }    

    internal static HttpResponseData BadRequestResponse(this HttpRequestData req, string payload = "")
    {
        var response = req.CreateResponse();
        response.StatusCode = HttpStatusCode.BadRequest;
        return response.FormalizeResponse(payload);
    }  

    internal static HttpResponseData UnprocessableEntityResponse(this HttpRequestData req, string payload = "")
    {
        var response = req.CreateResponse();
        response.StatusCode = HttpStatusCode.UnprocessableEntity;
        return response.FormalizeResponse(payload);
    }  

    internal static HttpResponseData FormalizeResponse(this HttpResponseData response, string payload)
    {
        if (!string.IsNullOrWhiteSpace(payload))
        {
            payload = payload.Trim();
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(payload);
        }

        return response;
    }
}