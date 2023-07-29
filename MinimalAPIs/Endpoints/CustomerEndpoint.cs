namespace MinimalAPIs.Endpoints;

public static class CustomerEndpoint
{
    public static void MapCustomerEndpoint(this WebApplication app)
    {
        var customer = app.MapGroup("").WithTags("A Customer APIs");

        customer.MapPost("/customer", async (IMediator mediator, [FromBody] CustomerAPI customer) =>
        {
            var validationResult = new CustomerValidator().Validate(customer);

            if (validationResult.IsValid)
            {
                var result = await mediator.Send(new CreateCustomerRequest(customer));
                return result;
            }
            else
            {
                return Results.BadRequest(validationResult.Errors);
            }
        });

        customer.MapGet("/customer", async (IMediator mediator, int customerId) =>
        {
            var result = await mediator.Send(new ReadCustomerRequest(customerId));
            return result;
        });
    }
}