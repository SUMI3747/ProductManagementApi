using Microsoft.AspNetCore.Mvc;

namespace PrdouctsApi.Helpers
{
    public interface IServiceResponseHelper
    {
        ActionResult HandleServiceResponse<T>(T response, string errorMessage);
    }
}
