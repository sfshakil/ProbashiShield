using Microsoft.AspNetCore.Mvc.Rendering;

namespace Infosight.Web.Helpers
{
    public static class ActiveMenuHelper
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string url)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var area = "";
            object areaObj;
            if (routeData.Values.TryGetValue("area", out areaObj))
            {
                area = "/" + areaObj.ToString();
            }

            var generatedUrl = area + "/" + routeController + "/" + routeAction;

            return url.Equals(generatedUrl) ? "active" : "";
        }
    }
}
