using AddressBook.BusinessLayer.Services.Interfaces;
using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using MinimalHelpers.Routing;
using OperationResults;
using OperationResults.AspNetCore.Http;

namespace AddressBook.Endpoints;

public class PeopleEndpoint : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var peopleApiGroup = endpoints.MapGroup("/api/people");

        peopleApiGroup.MapDelete("{id:guid}", DeleteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeletePerson")
            .WithOpenApi();

        peopleApiGroup.MapGet("{id:guid}", GetAsync)
            .Produces<Person>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetPerson")
            .WithOpenApi();

        peopleApiGroup.MapGet(string.Empty, GetListAsync)
            .Produces<PaginatedList<Person>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetPeople")
            .WithOpenApi();

        peopleApiGroup.MapPost(string.Empty, InsertAsync)
            .Produces<Person>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .WithName("InsertPerson")
            .WithOpenApi();

        peopleApiGroup.MapPut("{id:guid}", UpdateAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdatePerson")
            .WithOpenApi();
    }

    private static async Task<IResult> DeleteAsync(IPeopleService peopleService, Guid id, HttpContext httpContext)
    {
        var result = await peopleService.DeleteAsync(id, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    private static async Task<IResult> GetAsync(IPeopleService peopleService, Guid id, HttpContext httpContext)
    {
        var result = await peopleService.GetAsync(id, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    private static async Task<IResult> GetListAsync(IPeopleService peopleService, HttpContext httpContext, string searchText, int pageIndex = 0, int itemsPerPage = 50, string orderBy = "FirstName, LastName")
    {
        var result = await peopleService.GetListAsync(searchText, pageIndex, itemsPerPage, orderBy, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    private static async Task<IResult> InsertAsync(IPeopleService peopleService, SavePersonRequest request, HttpContext httpContext)
    {
        var result = await peopleService.InsertAsync(request, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result, "GetPerson", new { id = result.Content?.Id });
        return response;
    }

    private static async Task<IResult> UpdateAsync(IPeopleService peopleService, Guid id, SavePersonRequest request, HttpContext httpContext)
    {
        var result = await peopleService.UpdateAsync(id, request, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }
}