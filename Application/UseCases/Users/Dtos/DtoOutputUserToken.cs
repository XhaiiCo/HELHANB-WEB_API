﻿using Application.UseCases.Roles.Dtos;

namespace Application.UseCases.Users.Dtos;

public class DtoOutputUserToken
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime AccountCreation { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    
    
    public DtoOutputRole Role { get; set; }
    
    public string? ProfilePicturePath { get; set; }
}
