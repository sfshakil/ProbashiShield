using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProbashiShield.Shared.Hashing.Contracts;

namespace ProbashiShield.Web.Controllers
{
    public class GlobalController : Controller
    {
        private readonly ICipher _encrypter;

        public GlobalController(ICipher encrypter)
        {
            _encrypter = encrypter;
        }

        [Authorize]
        public IActionResult Error()
        {
            return View();
        }

        [Authorize]
        public IActionResult DynamicError(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(msg);
                var plainMessage = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                string message = _encrypter.DecryptString(plainMessage);

                ViewBag.ErrorMessage = message;
            }
            return View();
        }

        [Authorize]
        public IActionResult BadRequest()
        {
            return View();
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ErrorExternal(string errorMessage)
        {
            ViewBag.Errormessage = errorMessage;
            return View();
        }

        [Authorize]
        public IActionResult ContentNotFound()
        {
            return View();
        }
    }
}
