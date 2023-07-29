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
                var isCreated = await mediator.Send(new CreateCustomerRequest(customer));
                if (isCreated)
                {
                    return Results.Ok("Your customer has been added");
                }
                else
                {
                    return Results.Problem(); // TODO: Add details
                }
            }
            else
            {
                return Results.BadRequest(validationResult.Errors);
            }
        });

        // TODO GET, PUT, DELETE
        //customer.MapGet("/customer", async (IMediator mediator, CustomerAPI customer) =>
        //{
        //    var validationResult = new CustomerValidator().Validate(customer);

        //    if (validationResult.IsValid)
        //    {
        //        var isCreated = await mediator.Send(new CreateCustomerRequest(customer));
        //        if (isCreated)
        //        {
        //            return Results.Ok("Your customer has been added");
        //        }
        //        else
        //        {
        //            return Results.Problem(); // TODO: Add details
        //        }
        //    }
        //    else
        //    {
        //        return Results.BadRequest(validationResult.Errors);
        //    }
        //});
    }
}