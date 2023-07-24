using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Repository;
using static Dapper.SqlMapper;

namespace RazorProject.Pages.Employeepage
{
    public class AddModel : PageModel
    {

        IEmployeeRepository _productRepository;
        public AddModel(IEmployeeRepository employeeRepository)
        {
            _productRepository = employeeRepository;
        }

        [BindProperty]
        public Model.Employee employee { get; set; }

        [TempData]
        public string Message { get; set; }



        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var count = _productRepository.Add(employee);
                if (count > 0)
                {
                    Message = "New Product Added Successfully !";
                    return RedirectToPage("/Index");
                }
            }

            return Page();
        }
    }
}
