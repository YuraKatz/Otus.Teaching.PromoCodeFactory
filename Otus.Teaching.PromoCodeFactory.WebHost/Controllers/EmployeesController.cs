using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Otus.Teaching.PromoCodeFactory.WebHost.Models.Employee;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    ///     Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        ///     Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        ///     Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        //Реализовать Create/Delete/Update методы в EmployeesController 

        /// <summary>
        ///     Delete сотрудника
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var products = await _employeeRepository.GetByIdAsync(id);
            if (products == null) return NotFound();

            await _employeeRepository.DeleteAsync(products);

            return NoContent();
        }
        /// <summary>
        ///     Update  сотрудника
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeInputModel employeeInputModel)
        {
            if (employeeInputModel == null )
                return BadRequest();

            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _employeeRepository.UpdateAsync(employeeInputModel.Update(employee));

            return NoContent();
        }
        /// <summary>
        ///     создать сотрудника
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeInputModel employee)
        {
            if (employee == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _employeeRepository.AddAsync(employee.Update(new Employee()));
            return NoContent();
         
        }
    }
}