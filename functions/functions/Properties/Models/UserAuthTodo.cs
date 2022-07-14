using functions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace functions.Models
{
    public class UserAuthTodo : Todo
    {
        public string UserId { get; set; }
    }
}
