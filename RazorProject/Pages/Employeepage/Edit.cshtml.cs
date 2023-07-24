using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Repository;
using static Dapper.SqlMapper;

namespace RazorProject.Pages.Employeepage
{
    public class EditModel : PageModel
    {

        IEmployeeRepository _productRepository;
        public EditModel(IEmployeeRepository employeeRepository)
        {
            _productRepository = employeeRepository;
        }


        [BindProperty]
        public Model.Employee employee { get; set; }

        public void OnGet(int id)
        {
            employee = _productRepository.GetEmployee(id);
        }
        public IActionResult OnPost()
        {
            var data = employee;

            if (ModelState.IsValid)
            {
                var count = _productRepository.EditEmployee(data);
                if (count > 0)
                {
                    return RedirectToPage("/Index");
                }
            }

            return Page();
        }
    }
}
