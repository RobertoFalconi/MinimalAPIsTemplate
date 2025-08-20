using GPTOpenAIwithDotNetAspireAPIs.Commands;
using GPTOpenAIwithDotNetAspireAPIs.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GPTOpenAIwithDotNetAspireAPIs.CommandHandlers;

public class OpenAICommandHandler :
    IRequestHandler<OpenAICompletionCommand, OpenAIResponseModel>,
    IRequestHandler<OpenAIFileProcessingCommand, OpenAIResponseModel>
{
    private readonly ILogger<OpenAICommandHandler> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IMemoryCache _cache;

    public OpenAICommandHandler(
        ILogger<OpenAICommandHandler> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IMemoryCache cache)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("OpenAI");
        _apiKey = configuration["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI API key not configured.");
        _cache = cache;
    }

    public async Task<OpenAIResponseModel> Handle(OpenAICompletionCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"openai:{request.GetHashCode()}";

        if (_cache.TryGetValue(cacheKey, out OpenAIResponseModel? cachedResult) && cachedResult != null)
        {
            return cachedResult;
        }

        var result = await ProcessOpenAIRequestAsync(
            request.UserMessage,
            request.SystemPrompt,
            request.Model,
            request.Temperature,
            request.MaxTokens,
            cancellationToken);

        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(request.CacheMinutes ?? 5));
        return result;
    }

    public async Task<OpenAIResponseModel> Handle(OpenAIFileProcessingCommand request, CancellationToken cancellationToken)
    {
        return await ProcessOpenAIRequestAsync(
            request.FileContent,
            request.SystemPrompt,
            request.Model,
            cancellationToken: cancellationToken);
    }

    private async Task<OpenAIResponseModel> ProcessOpenAIRequestAsync(
        string userMessage,
        string? systemPrompt = null,
        string? model = "gpt-5-nano",
        double? temperature = 0.7,
        int? maxTokens = null,
        CancellationToken cancellationToken = default)
    {
        var messages = new List<object>();

        if (!string.IsNullOrEmpty(systemPrompt))
        {
            messages.Add(new { role = "system", content = systemPrompt });
        }

        messages.Add(new { role = "user", content = userMessage });

        var requestContent = new
        {
            model = model ?? "gpt-5-nano",
            messages = messages.ToArray(),
            temperature,
            max_tokens = maxTokens
        };

        var jsonContent = JsonSerializer.Serialize(requestContent);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", httpContent, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("OpenAI API error: {StatusCode} - {Body}", response.StatusCode, responseBody);
            throw new HttpRequestException($"OpenAI API error: {response.StatusCode}");
        }

        var root = JsonNode.Parse(responseBody);
        var content = root?["choices"]?[0]?["message"]?["content"]?.ToString();

        if (content == null)
        {
            throw new InvalidOperationException("Risposta OpenAI non valida o inattesa.");
        }

        return new OpenAIResponseModel
        {
            Content = content,
            Model = model ?? "gpt-5-nano",
            Usage = new UsageModel
            {
                PromptTokens = root?["usage"]?["prompt_tokens"]?.GetValue<int>() ?? 0,
                CompletionTokens = root?["usage"]?["completion_tokens"]?.GetValue<int>() ?? 0,
                TotalTokens = root?["usage"]?["total_tokens"]?.GetValue<int>() ?? 0
            }
        };
    }
}