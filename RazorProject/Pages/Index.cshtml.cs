using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Pages.Employeepage;
using RazorProject.Repository;

namespace RazorProject.Pages
{
    public class IndexModel : PageModel
    {
        
        IEmployeeRepository _productRepository;
        public IndexModel(IEmployeeRepository EmployeeRepository)
        {
            _productRepository = EmployeeRepository;
        }

        [BindProperty]
        public List<Model.Employee> EmployeeList { get; set; }


        [BindProperty]
        public Model.Employee employee { get; set; }

        [TempData]
        public string Message { get; set; }
        public void OnGet()
        {
            EmployeeList = _productRepository.GetList();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (id > 0)
            {
                var count = _productRepository.DeleteEmployee(id);
                if (count > 0)
                {
                    Message = "Product Deleted Successfully !";
                    return RedirectToPage("/Index");
                }
            }

            return Page();

        }
    }
}