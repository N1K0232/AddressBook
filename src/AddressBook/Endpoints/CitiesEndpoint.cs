using AddressBook.BusinessLayer.Services.Interfaces;
using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using MinimalHelpers.Routing;
using OperationResults.AspNetCore.Http;

namespace AddressBook.Endpoints;

public class CitiesEndpoint : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var citiesApiGroup = endpoints.MapGroup("/api/cities");

        citiesApiGroup.MapDelete("{id:guid}", DeleteAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteCity")
            .WithOpenApi();

        citiesApiGroup.MapGet("{id:guid}", GetAsync)
            .Produces<City>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetCity")
            .WithOpenApi();

        citiesApiGroup.MapGet(string.Empty, GetListAsync)
            .Produces<IEnumerable<City>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetCities")
            .WithOpenApi();

        citiesApiGroup.MapPost(string.Empty, InsertAsync)
            .Produces<City>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .WithName("InsertCity")
            .WithOpenApi();

        citiesApiGroup.MapPut("{id:guid}", UpdateAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateCity")
            .WithOpenApi();
    }

    private static async Task<IResult> DeleteAsync(ICityService cityService, Guid id, HttpContext httpContext)
    {
        var result = await cityService.DeleteAsync(id, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    private static async Task<IResult> GetAsync(ICityService cityService, Guid id, HttpContext httpContext)
    {
        var result = await cityService.GetAsync(id, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    private static async Task<IResult> GetListAsync(ICityService cityService, string name, HttpContext httpContext)
    {
        var result = await cityService.GetListAsync(name, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    private static async Task<IResult> InsertAsync(ICityService cityService, SaveCityRequest request, HttpContext httpContext)
    {
        var result = await cityService.InsertAsync(request, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result, "GetPerson", new { id = result.Content?.Id });
        return response;
    }

    private static async Task<IResult> UpdateAsync(ICityService cityService, Guid id, SaveCityRequest request, HttpContext httpContext)
    {
        var result = await cityService.UpdateAsync(id, request, httpContext.RequestAborted);

        var response = httpContext.CreateResponse(result);
        return response;
    }
}