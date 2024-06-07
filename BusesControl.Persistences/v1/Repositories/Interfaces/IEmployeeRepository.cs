﻿using BusesControl.Entities.Models;

namespace BusesControl.Persistence.v1.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<EmployeeModel?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(EmployeeModel record);
    bool Update(EmployeeModel record);
    Task<bool> ExistsByEmailOrPhoneNumberOrCpfAsync(string email, string phoneNumber, string cpf, Guid? id = null);
}
