using System.Collections;
using System.Reflection;

namespace Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Department {  get; set; }
        public override string ToString()
        {
            return string.Format($"" +
                    $"{Id}\t" +
                    $" {String.Concat(LastName, ", ", FirstName).PadRight(15, ' ')}\t" +
                    $"{Department}");
                    
        }
    }
}
