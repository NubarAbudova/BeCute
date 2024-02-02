using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs.Users
{
    public record RegisterDTO(string UserName,string Email,string Password,string ConfirmPassword,string Name,string Surname)
    {
    }
}
