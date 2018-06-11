using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiSecurity.BasicAuthentification
{
    public class BasicAuthentification : IBasicAuthentification
    {
        public bool Valider(string email, string password)
        {
            if ((email == "x1@test.com" && password =="x1passwordtest" )|| 
                (email == "x2@test.com" && password == "x2passwordtest"))
                return true;

            return false;
        }

        public bool IsAuthentifier(string email)
        {

            // utiliser deux compte em mode mock
            if (email == "x1@test.com" || email == "x2@test.com")
                return true;
            else
                return false;
        }
    }
}
