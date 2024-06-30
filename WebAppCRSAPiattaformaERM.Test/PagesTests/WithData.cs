//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.Configuration;
//using System.Text;

//namespace WebAppCRSAPiattaformaERM.Test.PagesTests;

//public class WithDataTests
//{
//    [Theory]
//    [InlineData("/")]
//    public async Task Html_whenCallingAPage_thenRetrieveData(string url)
//    {
//        string fakeAppSettings = File.ReadAllText("Contents/fakeAppSettings.json");
//        var builder = new ConfigurationBuilder()
//            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(fakeAppSettings)));

//        IConfigurationRoot configuration = builder.Build();

//        var application = new WebApplicationFactory<Program>()
//            .WithWebHostBuilder(builder =>
//            {
//                builder.UseConfiguration(configuration);
//            });
//        var client = application.CreateClient();
//        client.DefaultRequestHeaders.Add("INPS-MATRICOLA", "aaa");
//        client.DefaultRequestHeaders.Add("INPS-RUOLI", "cn = A1234:P00001,dc = inps,dc = it");
//        client.DefaultRequestHeaders.Add("INPS-ACCOUNT-WINDOWS", "aaa");
//        var response = await client.GetAsync(url);

//        var result = await response.Content.ReadAsStringAsync();

//        Assert.True(string.IsNullOrWhiteSpace(result));
//    }
//}