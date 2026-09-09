using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;

namespace Infosight.Web.ActionFilters
{
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public class ValidateCutOffTimeAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;
        public ValidateCutOffTimeAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string cutoffTimeString = _configuration["CutoffTime"] ?? "";
            TimeSpan cutOffTimeSpan;

            if (TimeSpan.TryParse(cutoffTimeString, out cutOffTimeSpan))
            {
                DateTime cutOffTime = DateTime.Now.Date.Add(cutOffTimeSpan);

                if (DateTime.Now > cutOffTime)
                {
                    context.Result = new ObjectResult("Cut off time exceeded.") { StatusCode = StatusCodes.Status422UnprocessableEntity };
                }
            }
        }

    }
}
