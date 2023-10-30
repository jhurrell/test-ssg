using System.Text.Json.Serialization;

namespace MyApplication;

public class EmailMessage
{
    [JsonPropertyName("name")]
    public string Name { get; set;} = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set;} = string.Empty;
    
    [JsonPropertyName("enquiry")]
    public string Enquiry { get; set;} = string.Empty;
}