namespace GPTOpenAIwithDotNetAspireAPIs.Models;

public class OpenAIRequestModel
{
    public string UserMessage { get; set; } = string.Empty;

    public string? SystemPrompt { get; set; }

    public string? Model { get; set; } = "gpt-5-nano";

    public double? Temperature { get; set; } = 0.7;

    public int? MaxTokens { get; set; }

    public int? CacheMinutes { get; set; } = 5;
}