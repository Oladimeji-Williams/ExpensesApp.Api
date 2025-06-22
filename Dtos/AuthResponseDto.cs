using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApp.API.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public AuthResponseDto(string token)
        {
            Token = token;
        }
    }
}