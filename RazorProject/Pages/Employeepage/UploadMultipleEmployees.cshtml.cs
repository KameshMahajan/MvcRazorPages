using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using RazorProject.Model;
using RazorProject.Repository;

namespace RazorProject.Pages.Employeepage
{
    public class UploadMultipleEmployeesModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmployeeRepository _dbContext;
        public UploadMultipleEmployeesModel(IWebHostEnvironment webHostEnvironment, IEmployeeRepository dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _dbContext = dbContext;
        }

        [BindProperty]
        public IFormFile ExcelFile { get; set; }

        [TempData]
        public string Message { get; set; }


        public void OnGet()
        {

        }
        public IActionResult OnPost(IFormFile excelFile)
        {


            // Check if a file was uploaded
            if (ExcelFile == null || ExcelFile.Length == 0)
            {
                Message = "No file selected for upload.";
                return Page();
            }

            // Check the file extension to make sure it's an Excel file
            if (!Path.GetExtension(ExcelFile.FileName).Equals(".xlsx", System.StringComparison.OrdinalIgnoreCase))
            {
                Message = "Please upload an Excel file (.xlsx).";
                return Page();
            }

            try
            {

                List<Employee> employees = new List<Employee>();

                using (var stream = new MemoryStream())
                {
                    ExcelFile.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet != null)
                        {
                            // Assuming your Excel file has columns FirstName, LastName, Department, and Designation
                            int rowCount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                var firstName = worksheet.Cells[row, 1].Value.ToString();
                                var lastName = worksheet.Cells[row, 2].Value.ToString();
                                var department = worksheet.Cells[row, 3].Value.ToString();
                                var designation = worksheet.Cells[row, 4].Value.ToString();

                                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
                                    !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(designation))
                                {
                                    // Add the employee data to the list
                                    employees.Add(new Employee
                                    {
                                        FirstName = firstName,
                                        LastName = lastName,
                                        Department = department,
                                        Designation = designation
                                    });
                                    // Use AddRange method to add multiple employees at once
                                    
                                }
                            }
                            foreach (var employee in employees)
                            {
                                _dbContext.Add(employee);
                            }
                           

                        }
                    }
                }
              
                return RedirectToPage("/Index");
                    
            }
            catch (System.Exception ex)
            {
                Message = $"An error occurred while processing the file: {ex.Message}";
            }
            return Page();
        }

    }
}
