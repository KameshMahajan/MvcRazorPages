using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorProject.Pages.Map
{
    public class GoogleMapModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
