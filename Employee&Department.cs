
namespace Entites
{
    public class Employee : IComparable<Employee>
    {
        public Employee() { }
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime HireDate { get; set; }

        public string Gender { get; set; }

        public int DepartmentId { get; set; }

        public bool HasHealthInsurance { get; set; }

        public bool HasPensionPlan { get; set; }

        public decimal Salary { get; set; }

        public string FullName => $"{FirstName} {LastName}";


        public override string ToString()
        {
            return
                    string.Format($"" +
                    $"{Id}\t" +
                    $" {String.Concat(LastName, ", ", FirstName).PadRight(15, ' ')}\t" +
                    $"{HireDate.Date.ToShortDateString()}\t" +
                    $"{Gender.PadRight(10, ' ')}\t" +
                    $"{DepartmentId.ToString().PadRight(10, ' ')}\t" +
                    $"{HasHealthInsurance}\t" +
                    $"{HasPensionPlan}\t" +
                    $"${Salary.ToString("0.00")}");
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if(obj == null) return false;
            if(obj.GetType() != this.GetType()) return false;

            var other = (Employee)obj;

            return (this.Id == other.Id &&
                this.FirstName == other.FirstName &&
                this.LastName == other.LastName &&
                this.HireDate == other.HireDate &&
                this.Gender == other.Gender &&
                this.DepartmentId == other.DepartmentId &&
                this.Salary == other.Salary &&
                this.FullName == other.FullName &&
                this.HasHealthInsurance == other.HasHealthInsurance &&
                this.HasPensionPlan == other.HasPensionPlan
                );
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Id.GetHashCode();
            return hash;
        }

        public int CompareTo(Employee? other)
        {
            if (ReferenceEquals(this, other))
                throw new ArgumentNullException("other");

            if (other == null)
                throw new ArgumentNullException("other");

            if (other.GetType() != this.GetType())
                throw new ArgumentException();

            var obj = (Employee)other;

            if (this.Id < other.Id) return -1;
            else if (this.Id > other.Id) return 1;
            else return 0;
        }
    }


    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return
            string.Format($"" +
                    $"{Id}\t" +
                    $" {Name}");
        }
    }
}
