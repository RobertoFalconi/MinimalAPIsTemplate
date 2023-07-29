using MinimalAPIs.Models.API;
namespace MinimalAPIs.Endpoints;

public static class CustomerEndpoint
{
    public static void MapMyEndpoints(this WebApplication app)
    {
        app.MapPost("/customer", async ([FromBody] Customer customer) =>
        {
            var validationResult = new CustomerValidator().Validate(customer);

            if (validationResult.IsValid)
            {
                // TODO: Add customer in DB
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest(validationResult.Errors);
            }
        });
    }
}