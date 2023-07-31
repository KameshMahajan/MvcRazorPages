using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorProject.Model;
using RazorProject.Repository;

namespace RazorProject.Pages.CRUD
{
    public class SinglePageModel : PageModel
    {
        IEmployeeRepository _Repository;
        public SinglePageModel(IEmployeeRepository EmployeeRepository)
        {
            _Repository = EmployeeRepository;
        }

        [BindProperty]
        public List<Model.Employee> EmployeeList { get; set; }

        [BindProperty]
        public Employee Emp { get; set; }

        [TempData]
        public string Message { get; set; }
        public IActionResult OnGetData()
        {
            var EmployeeList = _Repository.GetList();
            return new JsonResult(EmployeeList);
        }

       
        [ValidateAntiForgeryToken]
        public IActionResult OnPostNewData()
        {
            if (ModelState.IsValid)
            {
                var count = _Repository.Add(Emp);
                if (count > 0)
                {
                    Message = "New Product Added Successfully !";
                    return new JsonResult(new { success = true, message = Message });
                }
            }

            return new JsonResult(new { success = false, message = "Error adding product" });
        }


        [ValidateAntiForgeryToken]
        public IActionResult OnPostDeleteData(int id)
        {
            if (id > 0)
            {
                var count = _Repository.DeleteEmployee(id);
                if (count > 0)
                {
                    return new JsonResult(new { success = true, Message = "Employee deleted successfully." });
                }
            }

            return Page();
        }


        public IActionResult OnGetDataByKey(int id)
        {
            if (id > 0)
            {
                Emp = _Repository.GetEmployee(id);
                return new JsonResult(Emp);
            }

            return NotFound();
        }

        
        [ValidateAntiForgeryToken]
        public IActionResult OnPostUpdateData()
        {
            var data = Emp;

            if (ModelState.IsValid)
            {
                var count = _Repository.EditEmployee(data);
                if (count > 0)
                {
                    return new JsonResult(new { success = true, Message = "Employee deleted successfully." });
                }
            }

            return NotFound();
        }
    }
}
