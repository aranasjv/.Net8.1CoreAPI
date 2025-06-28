using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }


        public string Department { get; set; }

        public string DateofJoining { get; set; }

        public string PhotoFileName { get; set; }
    }
}
