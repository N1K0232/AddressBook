using AddressBook.BusinessLayer.Services.Interfaces;
using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Common;
using AddressBook.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using OperationResults.AspNetCore;

namespace AddressBook.Controllers;

public class PeopleController(IPeopleService peopleService) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await peopleService.DeleteAsync(id);
        return HttpContext.CreateResponse(result);
    }

    [HttpGet("{id:guid}", Name = "GetPerson")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await peopleService.GetAsync(id);
        return HttpContext.CreateResponse(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListResult<Person>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList(string searchText = null, string orderBy = "FirstName, LastName", int pageIndex = 0, int itemsPerPage = 10)
    {
        var result = await peopleService.GetListAsync(searchText, orderBy, pageIndex, itemsPerPage);
        return HttpContext.CreateResponse(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Person), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Save([FromBody] SavePersonRequest person)
    {
        var result = await peopleService.SaveAsync(person);
        return HttpContext.CreateResponse(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Save(Guid id, SavePersonRequest person)
    {
        var result = await peopleService.SaveAsync(id, person);
        return HttpContext.CreateResponse(result);
    }
}