using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.Controllers;

[ApiController]
[TypeFilter<ApiExceptionFilter>]
public class ApiBaseController : ControllerBase
{
    
}