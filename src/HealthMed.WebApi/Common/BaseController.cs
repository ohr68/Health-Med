using Microsoft.AspNetCore.Mvc;

namespace HealthMed.WebApi.Common;

public class BaseController : ControllerBase
{
    protected IActionResult Ok<T>(T data) =>
        base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

   
}