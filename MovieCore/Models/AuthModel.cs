using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesCore.Models
{
    public class AuthModel
    {
        public string Message { get; set; } = default!;
        public bool IsAuthanticated { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public List<string> Roles { get; set; } = default!;
        public string Token { get; set; } = default!;
        public DateTime ExpiresOn { get; set; }



    }
}
