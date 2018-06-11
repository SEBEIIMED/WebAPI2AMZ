using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiSecurity.BasicAuthentification
{
    public interface IBasicAuthentification
    {
        bool Valider(string email, string password);
        bool IsAuthentifier(string email);
    }
}
