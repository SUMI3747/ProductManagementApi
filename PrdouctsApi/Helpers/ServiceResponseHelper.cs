using Microsoft.AspNetCore.Mvc;

namespace PrdouctsApi.Helpers
{
    public class ServiceResponseHelper : IServiceResponseHelper
    {
        public ActionResult HandleServiceResponse<T>(T response, string errorMessage)
        {
            if (response == null)
            {
                return new NotFoundObjectResult(errorMessage);
            }

            return new OkObjectResult(response);
        }

    }
}
