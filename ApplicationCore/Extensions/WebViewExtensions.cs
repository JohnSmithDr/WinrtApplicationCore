using System;
using System.Threading.Tasks;

namespace Windows.UI.Xaml.Controls
{
    public static class WebViewExtensions
    {
        public static async Task<string> GetDocumentHtmlAsync(this WebView control)
        {
            var html = await control.InvokeScriptAsync("eval", new[] { "document.documentElement.outerHTML;" });
            return html;
        }
    }
}
