namespace Fanex.NCB.Main.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Text;

    public class WhoamiController : Controller
    {
        [Route("Whoami")]
        [AllowAnonymous]
        public IActionResult Whoami()
        {
            var context = HttpContext;
            var content = new StringBuilder();
            content.Append($"Web name: {context.Request.Host.Host}{Environment.NewLine}"
                + $"Port: {context.Connection.LocalPort}{Environment.NewLine}"
                + $"Server IP: {context.Connection.LocalIpAddress}{Environment.NewLine}"
                + $"Server Time: {DateTime.Now}{Environment.NewLine}"
                + $"Client IP: {context.Connection.RemoteIpAddress}{Environment.NewLine}"
                + $"Request Scheme: {context.Request.Scheme}{Environment.NewLine}"
                + $"Request Path: {context.Request.Path}{Environment.NewLine}{Environment.NewLine}"
                + $"Request Headers:{Environment.NewLine}");

            foreach (var header in context.Request.Headers)
            {
                content.Append($"- {header.Key}: " +
                    $"{header.Value}{Environment.NewLine}");
            }

            return Content(content.ToString());
        }
    }
}