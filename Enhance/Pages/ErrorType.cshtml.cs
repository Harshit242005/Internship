using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enhance.Pages
{
    public class ErrorTypeModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Page"] = "Show";
        }
    }
}
