using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLink.mapper
{
    interface IApiAuth
    {
        bool Validate(string key);
    }
}
