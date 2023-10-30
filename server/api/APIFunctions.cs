using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MyApplication;

public class APIFunctions
{
    const string DevEnvValue = "Development";
    const string authCookieName = "authCookie";
    
    private readonly ILogger _logger;

    public APIFunctions(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<APIFunctions>();
    }

    private static string GetFunctionBaseUrl(HttpRequestData req)
    {
        var headerValues = req.Headers.GetValues("Referrer");
        var referrer = headerValues.FirstOrDefault() ?? null;

        if(referrer is null)
        {
            return string.Empty;
        }

        var uriReferrer = new UriBuilder(referrer).Uri;
        if(uriReferrer == null){
            return string.Empty;
        }

        referrer = uriReferrer.GetLeftPart(UriPartial.Authority).ToLower();
        return referrer;
    }

    [Function("Authenticate")]
    public async Task<HttpResponseData> Authenticate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req
    ) 
    {
        await Task.Delay(0);

        var isDevEnv = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == DevEnvValue;
        var jwtSecret = Environment.GetEnvironmentVariable("JwtSecret");
        var jwtAudience = Environment.GetEnvironmentVariable("JwtAudience"); 
        var domainWhitelist = Environment.GetEnvironmentVariable("DOMAIN_WHITELIST");

        if(string.IsNullOrWhiteSpace(jwtSecret))
        {
            var errorMessage = "JwtSecret setting is empty";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse (errorMessage);
        }    

        if(string.IsNullOrWhiteSpace(jwtAudience))
        {
            var errorMessage = "JwtAudience setting is empty";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);
        }

        if(string.IsNullOrWhiteSpace(domainWhitelist))
        {
            var errorMessage = "Domain Whitelist setting is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }

        var domains = domainWhitelist.Split(',').ToArray();

        var referrer = GetFunctionBaseUrl(req);
        if(referrer is null) 
        {
            var errorMessage = "Referrer header was not supplied";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);
        }

        if(!domains.Where(x => x.ToLower().Equals(referrer)).Any()) 
        {
            var errorMessage = $"Request is denied as it came from {referrer} and is not in the domain whitelist";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);
        }

        if(req.Headers.Contains("ContentType"))
        {
            var errorMessage = "ContentType header is not allowed";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);            
        }

        // Create the actual token.
        var token = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(jwtSecret)
            .Id(Guid.NewGuid())
            .IssuedAt(DateTime.Now)
            .Issuer(referrer)
            .NotBefore(DateTime.Now)
            .ExpirationTime(DateTime.Now.AddHours(3))
            .Audience(jwtAudience)
            .Encode();

        var authCookie = new HttpCookie(authCookieName, token)
        {
            HttpOnly = true,     // No javascript can manipulate this coookie.
            Path = "/api/",
            SameSite = SameSite.Strict,
            Secure = !isDevEnv,
            Expires = null
        };

        var response = req.OKResponse();
        response.Cookies.Append(authCookie);
        return response;
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
        var jwtSecret = Environment.GetEnvironmentVariable("JwtSecret");
        var jwtAudience = Environment.GetEnvironmentVariable("JwtAudience");         

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

        if(string.IsNullOrWhiteSpace(jwtSecret))
        {
            var errorMessage = "JWT Secret is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }   
        
        if(string.IsNullOrWhiteSpace(jwtAudience))
        {
            var errorMessage = "JWT Audience is empty";
            _logger.LogError(errorMessage);
            return req.ServerErrorResponse(errorMessage);
        }                   

        var token = req.Cookies.Where(p => p.Name == authCookieName).FirstOrDefault()?.Value;
        if(token is null)
        {
            var errorMessage = "Auth Cookie not found";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);            
        }

        // Decode the token.
        var decodedToken = JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(jwtSecret)
            .MustVerifySignature()
            .Decode<IDictionary<string, object>>(token);

        if(decodedToken is null)
        {
            var errorMessage = "Unable to decrypt token";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);   
        }

        var referrer = GetFunctionBaseUrl(req);
        if(referrer is null) 
        {
            var errorMessage = "Referrer Header was not supplied";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);                
        }

        string propToTest;

        // Validate that the referrer was from us and not stolen and being used from some other website.
        propToTest = decodedToken["iss"]?.ToString() ?? string.Empty;
        if(string.Compare(propToTest, referrer, true) != 0)
        {
            var errorMessage = $"Token Issuer Value {propToTest} does not match {referrer}";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);
        }

        // The JWT has not been tampered with.
        propToTest = decodedToken["aud"]?.ToString() ?? string.Empty;
        if(string.Compare(propToTest, jwtAudience, true) != 0)
        {
            var errorMessage = $"Token Issuer Value {propToTest} does not match {jwtAudience}";
            _logger.LogError(errorMessage);
            return req.BadRequestResponse(errorMessage);
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
