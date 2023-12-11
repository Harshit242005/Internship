using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enhance.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public string Code { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                string typedCode = Code; // Get the typed code from the Code property

                // Compare the typed code with the randomly generated code
                if (typedCode == SignupModel.code)
                {
                    // Code matches, do something
                    return RedirectToPage("/login"); // Redirect to a success page
                }
                else
                {
                    // Code does not match, show an error message or perform appropriate actions
                    return Page();
                }
            }

            // If the code doesn't match or there are other validation errors, return to the page with the error message
            return Page();
        }

    }
}