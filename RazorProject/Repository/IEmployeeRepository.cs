using RazorProject.Model;

namespace RazorProject.Repository
{
    public interface IEmployeeRepository
    {
        int Add(Employee product);
        //int Add(List<Employee> product);

        List<Employee> GetList();

        Employee GetEmployee(int id);

        int EditEmployee(Employee product);

        int DeleteEmployee(int id);
    }
}
