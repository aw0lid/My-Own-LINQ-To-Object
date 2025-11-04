using MyLINQ;
using Repo;

var allEmp = Repository.LoadEmployees();
var result = allEmp.MyOrder().MyOrderBy(x => x.Salary).MyThenBy(x => x.LastName, true).MyThenBy(x => x.Salary, true);


foreach (var emp in result) Console.WriteLine(emp);


Console.ReadKey();