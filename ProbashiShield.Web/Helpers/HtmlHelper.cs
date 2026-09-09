using Microsoft.AspNetCore.Mvc.Rendering;
using NetEscapades.AspNetCore.SecurityHeaders;
using System;

namespace Infosight.Web.Helpers
{
    public static class HtmlHelper
    {
        public static string GenerateNonce(this IHtmlHelper helper)
        {
            return helper.ViewContext.HttpContext.GetNonce() ?? "";
        }

        public static string GetIconDom(this IHtmlHelper helper, string icon, byte[] iconImg, string iconImgName)
        {
            if (iconImg == null || string.IsNullOrEmpty(iconImgName))
            {
                if (string.IsNullOrEmpty(icon))
                {
                    return null;
                }
                return @$"<i class=""quick-link-icon {icon}""></i>";
            }

            if (iconImgName.Contains("."))
            {
                iconImgName = iconImgName.Substring(iconImgName.IndexOf('.') + 1);
            }

            string base64Image = Convert.ToBase64String(iconImg);
            string imageSrc = $"data:image/{iconImgName};base64,{base64Image}";
            return $@"<img class = ""quick-link-icon-image"" src=""{imageSrc}"" />";
        }
    }
}
