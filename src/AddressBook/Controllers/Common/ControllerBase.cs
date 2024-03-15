using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
}