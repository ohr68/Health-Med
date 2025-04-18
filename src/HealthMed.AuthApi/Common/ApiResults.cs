using KeycloakAuthIntegration.WebApi.Common;

namespace HealthMed.AuthApi.Common;

public static class ApiResults
{
    public static IResult Ok<T>(T data) =>
        Results.Ok(new ApiResponseWithData<T>
        {
            Data = data,
            Success = true
        });

    public static IResult Created<T>(string routeName, object routeValues, T data) =>
        Results.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T>
        {
            Data = data,
            Success = true
        });
}