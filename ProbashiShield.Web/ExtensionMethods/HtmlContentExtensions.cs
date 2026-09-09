using Microsoft.AspNetCore.Html;
using System;
using System.Text.Encodings.Web;

namespace Infosight.Web.ExtensionMethods
{
    public static class HtmlContentExtensions
    {
        public static HtmlString ToHtmlStringCustom(this IHtmlContent htmlContent)
        {
            if (htmlContent is HtmlString htmlString)
            {
                return htmlString;
            }

            string tagString = "";
            using (var writer = new System.IO.StringWriter())
            {
                htmlContent.WriteTo(writer, HtmlEncoder.Default);
                tagString = writer.ToString();
            }

            tagString = tagString.Replace(Environment.NewLine, "").Replace("\"", "'");

            return new HtmlString(@tagString);
        }

    }
}
