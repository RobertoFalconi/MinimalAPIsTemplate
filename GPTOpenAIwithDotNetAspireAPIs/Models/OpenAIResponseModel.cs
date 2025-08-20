namespace GPTOpenAIwithDotNetAspireAPIs.Models;

public class OpenAIResponseModel
{
    public string Content { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public UsageModel Usage { get; set; } = new();
}

public class UsageModel
{
    public int PromptTokens { get; set; }

    public int CompletionTokens { get; set; }

    public int TotalTokens { get; set; }
}