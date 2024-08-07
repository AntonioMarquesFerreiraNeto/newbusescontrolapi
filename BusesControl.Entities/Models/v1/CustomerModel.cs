﻿using BusesControl.Entities.Enums.v1;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Entities.Models.v1;

public class CustomerModel
{
    public Guid Id { get; set; }
    [MaxLength(90)]
    public string Name { get; set; } = default!;
    [MaxLength(11)]
    public string? Cpf { get; set; }
    [MaxLength(14)]
    public string? Cnpj { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? OpenDate { get; set; }
    [MaxLength(80)]
    public string Email { get; set; } = default!;
    [MaxLength(11)]
    public string PhoneNumber { get; set; } = default!;
    [MaxLength(20)]
    public string HomeNumber { get; set; } = default!;
    [MaxLength(60)]
    public string Logradouro { get; set; } = default!;
    [MaxLength(60)]
    public string ComplementResidential { get; set; } = default!;
    [MaxLength(60)]
    public string Neighborhood { get; set; } = default!;
    [MaxLength(60)]
    public string City { get; set; } = default!;
    [MaxLength(60)]
    public string State { get; set; } = default!;
    public bool Active { get; set; } = true;
    public CustomerTypeEnum Type { get; set; }
    public PersonGenderEnum? Gender { get; set; }
    public string ExternalId { get; set; } = default!;
}
