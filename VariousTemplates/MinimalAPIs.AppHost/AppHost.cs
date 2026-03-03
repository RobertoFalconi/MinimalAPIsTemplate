var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.GPTOpenAIwithDotNetAspireAPIs>("gptopenaiwithdotnetaspireapis");

builder.Build().Run();
