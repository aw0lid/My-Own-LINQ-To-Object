# Custom LINQ to Object

## Overview
**Custom LINQ to Object** is a C# library designed to simplify working with in-memory data. It allows you to perform a variety of operations on collections, including sorting, filtering, grouping, joining, and transforming data. 

The library was primarily created as an educational tool to help understand how LINQ works in C# and how data is processed step by step through a series of operations. It clearly demonstrates how queries are executed internally on in-memory objects without relying on external databases.

---

## Deferred vs Immediate Execution
One of the core concepts demonstrated by Custom LINQ to Object is the difference between:

- **Deferred Execution:** Methods such as `MyWhere`, `MySelect`, and `MyOrderBy` build a pipeline of operations that are executed only when the final result is enumerated (e.g., via `foreach`). This allows you to chain multiple query operations efficiently before the data is actually processed.

- **Immediate Execution:** Methods such as `MyCount`, `MyToList`, and `MyToArray` execute immediately, processing all in-memory data and returning results right away.

This design makes it easy to understand how LINQ pipelines work internally and how operations are applied sequentially on data.

---

## Project Structure

### Repository
Contains the main in-memory data sources, including employees and departments. Serves as the primary source for all queries.

### Employee & Department
- **Employee:** Represents an employee with properties like first name, last name, salary, and department.
- **Department:** Represents a department with its details.

### EmployeeDto
Data Transfer Objects (DTOs) used to represent query results after processing data. They simplify transferring and displaying results across different layers of the project.

### MyLinqMethods
The core library containing custom LINQ-like extension methods, including:

- **Sorting:** `MyOrder`, `MyOrderBy`, `MyThenBy`
- **Filtering:** `MyWhere`
- **Grouping & Aggregation:** `MyGroupBy`, `MyToLookup`, `MyAggregate`
- **Joining:** `MyJoin`, `MyLeftJoin`
- **Projection & Transformation:** `MySelect`, `MySelectMany`, `MyCast`, `MyTypeOf`
- **Collection Operations:** `MyTake`, `MySkip`, `MyDistinct`, `MyUnion`, `MyIntersect`, `MyExcept`

### MySorter
A helper class for advanced sorting in Custom LINQ to Object. Supports sorting by keys in ascending or descending order and enables multi-level sorting using `ThenBy`.

### QuickSorter
An internal utility used by `MySorter` to efficiently sort large collections using the QuickSort algorithm.

---

## Usage Example

```csharp
using CustomLINQtoObject;
using Repo;

var allEmp = Repository.LoadEmployees();
var result = allEmp
    .MyOrder()                        // Base sorting
    .MyOrderBy(x => x.Salary)         // Sort by salary
    .MyThenBy(x => x.LastName, true)  // Descending by last name
    .MyThenBy(x => x.Salary, true);   // Descending by salary

foreach (var emp in result)
    Console.WriteLine(emp);


This example demonstrates how to chain multiple sorting operations while keeping the code clean and readable.

**Key Concepts Demonstrated:**

- Step-by-step execution of LINQ queries on in-memory collections.
- Difference between deferred and immediate execution.
- Chaining operations in a pipeline to manipulate data efficiently.
