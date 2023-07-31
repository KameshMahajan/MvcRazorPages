using Dapper;
using RazorProject.Model;
using System.Data;
using System.Data.SqlClient;

namespace RazorProject.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        IConfiguration _configuration;
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /*connnection string*/
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("connectMe").Value;
            return connection;
        }


        public int Add(Employee employee)
        {
            var connectionString = this.GetConnection();
            int count = 0;
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    //var query = "INSERT INTO NewEmployee([FirstName],[LastName],[Department],[Designation]) values (@FirstName,@LastName,@Department,@Designation); SELECT CAST(SCOPE_IDENTITY() as INT);";
                    //count = con.Execute(query, product);
                    count = con.Query<int>("dbo.usp_InsertNewEmployee",
                       new
                       {
                           FirstName = employee.FirstName,
                           LastName = employee.LastName,
                           Department = employee.Department,
                           Designation = employee.Designation
                       },
                       commandType: CommandType.StoredProcedure)
                       .Single();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count;
            }
        }

        public int DeleteEmployee(int id)
           {
            var connectionString = this.GetConnection();
            var count = 0;

            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    //var query = "DELETE FROM newEmployee WHERE Id =" + id;
                    //count = con.Execute(query);
                     count = con.Execute("dbo.usp_DeleteEmployee",
                    new { id },
                    commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count;
            }
        }

        public int EditEmployee(Employee employee)
        {
            var connectionString = this.GetConnection();
            var count = 0;

            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    //var query = "UPDATE NewEmployee SET FirstName = @FirstName, LastName = @LastName, Department = @Department,Designation=@Designation  WHERE Id = @Id";
                    //count = con.Execute(query, product);
                    count = con.Execute("dbo.usp_UpdateEmployee",
                        new
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Department = employee.Department,
                            Designation = employee.Designation
                        },
                        commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count;
            }
        }

        public Employee GetEmployee(int id)
        {
            var connectionString = this.GetConnection();
            Employee employee = new Employee();

            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    //var query = "SELECT id, FirstName, LastName, Department, Designation FROM newEmployee WHERE Id = " + id;
                    //employee = con.Query<Employee>(query).FirstOrDefault();
                    employee = con.Query<Employee>("dbo.usp_GetEmployeeById",new { id }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return employee;
            }
        }


        /*for fetching Employees*/
        public List<Employee> GetList()
        {
            var connectionString = this.GetConnection();
            List<Employee> employees = new List<Employee>();

            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    //var query = "SELECT Id, FirstName, LastName, Department, Designation FROM NewEmployee";
                    //employees = con.Query<Employee>(query).ToList();
                    employees = con.Query<Employee>("dbo.usp_GetAllEmployees", commandType: CommandType.StoredProcedure).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return employees;
            }
        }
    }
}
