namespace Otus.Teaching.PromoCodeFactory.WebHost.Models.Employee
{
    public class EmployeeInputModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Core.Domain.Administration.Employee Update(Core.Domain.Administration.Employee employee)
        {
            employee.Email = Email;
            employee.LastName = LastName;
            employee.FirstName = FirstName;
            return employee;
        }
    }
}