﻿namespace Application.UseCases.Users.Dtos;

public class DtoOutputUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime AccountCreation { get; set; }
    public string Email { get; set; }
//    public DateOnly BirthDate { get; set; }
    public int RoleId { get; set; }
}