using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MyApplication;

public class APIFunctions
{
    const string DevEnvValue = "Development";
    
    private readonly ILogger _logger;

    public APIFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<APIFunctions>();
    }

    [Function("SendEmailMessage")]
    public async Task<HttpResponseData> SendEmailMessage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req
    ) 
    {
        _logger.LogInformation("SendEmailMessage processing...");
        var isDevEnv = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == DevEnvValue;
        var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
        var smtpServerPort = Environment.GetEnvironmentVariable("SMTP_SERVER_PORT");
        var smtpServerSSLSetting = Environment.GetEnvironmentVariable("SMTP_SERVER_SSL");
        var smtpServerUsername = Environment.GetEnvironmentVariable("SMTP_SERVER_USERNAME");
        var smtpServerPassword = Environment.GetEnvironmentVariable("SMTP_SERVER_PASSWORD");
        var smtpServerEmailFrom = Environment.GetEnvironmentVariable("SMTP_SERVER_EMAIL_FROM");
        var smtpServerEmailTo = Environment.GetEnvironmentVariable("SMTP_SERVER_EMAIL_TO");

        var smtpServerSSL = false;

        // If the SMTP Server setting is invalid, return an error.
        if(string.IsNullOrWhiteSpace(smtpServer))
        {
            var errorMessage = "SMTP Server setting is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }

        if(string.IsNullOrWhiteSpace(smtpServerPort))
        {
            var errorMessage = "SMTP Server Port is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }        

        if(string.IsNullOrWhiteSpace(smtpServerSSLSetting))
        {
            var errorMessage = "SMTP Server SSL is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }
        else if(!Boolean.TryParse(smtpServerSSLSetting, out smtpServerSSL))
        {
            var errorMessage = "SMTP Server SSL must be true or false";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);            
        }  
        
        if(string.IsNullOrWhiteSpace(smtpServerUsername))
        {
            var errorMessage = "SMTP Server Username is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }                 

        if(string.IsNullOrWhiteSpace(smtpServerPassword))
        {
            var errorMessage = "SMTP Server Password is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }   

        if(string.IsNullOrWhiteSpace(smtpServerEmailFrom))
        {
            var errorMessage = "SMTP Server Email From is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }   

        var contentType = string.Empty;
        if (!req.Headers.TryGetValues("Content-Type", out IEnumerable<string>? headerValues))
        {
            var errorMessage = $"Contact Us message is unreadable";
            _logger.LogError(errorMessage);
            return req.UnprocessableEntityResponse(errorMessage);            
        }
        else
        {
            contentType = headerValues.First();
        }

        if (!string.IsNullOrWhiteSpace(contentType) && !contentType.Contains("application/json")) 
        {
            var errorMessage = $"{contentType} is not allowed";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);
        }
        
        // Obtain the entire message body.
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonSerializer.Deserialize<EmailMessage>(requestBody);

        // Validate the message.
        if(data is null || string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Enquiry)) 
        {
            var errorMessage = $"Contact Us message is incomplete";
            _logger.LogError(errorMessage);
            return req.UnprocessableEntityResponse(errorMessage);
        }

        return req.OKResponse($"SendEmailMessage name: {data.Name}, email: {data.Email}, enquiry: {data.Enquiry}");        
    }
}

public class EmailMessage
{
    [JsonPropertyName("name")]
    public string Name { get; set;} = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set;} = string.Empty;
    
    [JsonPropertyName("enquiry")]
    public string Enquiry { get; set;} = string.Empty;
}