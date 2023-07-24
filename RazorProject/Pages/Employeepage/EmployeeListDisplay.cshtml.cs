using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Repository;
using static Dapper.SqlMapper;

namespace RazorProject.Pages.Employeepage
{
    public class EmployeeModel : PageModel
    {
        IEmployeeRepository _productRepository;
        public EmployeeModel(IEmployeeRepository EmployeeRepository)
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
    }
}
