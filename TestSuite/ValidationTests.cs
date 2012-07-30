using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Janga.Validation;

namespace TestSuite
{
    [TestFixture]
    public class ValidationTests
    {
        
        /// <summary>
        /// Should be able to execute validation using mulitple when clauses.  
        /// </summary>
        [Test]
        public void CanUseWhenClauseLamdaOnEmployee()
        {
            var employee = new Employee() { LastName = "Robbins", FirstName = "Dave", Age = 45};

            var results = employee.Enforce("Person", true)
                                    .When("Age", Compare.GreaterThan, 45)
                                    .When("FirstName", Compare.Equal, "Dave");

            Assert.IsTrue(results.IsValid);            
        }

        /// <summary>
        /// Should be able to chain When clauses and execute all queries despite
        /// an individual query returing false.  A log of errors should be available.
        /// </summary>
        [Test]
        public void CanUseWhenClauseAndReportErrors()
        {
            var employee = new Employee() { LastName = "Robbins", FirstName = "Dave", Age = 45 };

            //  Fail on purpose - Age is wrong and Name is wrong
            var results = employee.Enforce("Person", true)
                                    .When("Age", Compare.GreaterThan, 47)
                                    .When("FirstName", Compare.Equal, "Daver");

            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessages.Count == 2);

            Console.Out.WriteLine("Test CanUseWhenClauseAndReportErrors:");
            results.ErrorMessages
                    .ToList()
                    .ForEach(x => Console.Out.WriteLine(x));
        }

        /// <summary>
        /// Should be able to execute a "Contains" clause with a when.
        /// </summary>
        [Test]
        public void CanUseWhenWithContainsClauseLamdbaOnEmployee()
        {
            var employee = new Employee() { LastName = "Robbins", FirstName = "Dave", Age = 45 };
            
            var results = employee.Enforce("Employee", true)
                                        .When("FirstName", Compare.Contains, "Da")
                                        .When("Age", Compare.LessThan, 47);

            Assert.IsTrue(results.IsValid);
        }

        /// <summary>
        /// Should be able to execute a chain of When queries with a contains clause
        /// and report errors.
        /// </summary>
        [Test]
        public void CanUseWhenWithClauseAndReportErrors()
        {
            var employee = new Employee() { LastName = "Robbins", FirstName = "Dave", Age = 45 };

            //  Fail on purpose - age is wrong
            var results = employee.Enforce("Employee", true)
                                        .When("FirstName", Compare.Contains, "Da")
                                        .When("Age", Compare.GreaterThan, 47);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.ErrorMessages.Count);

            Console.Out.WriteLine("Test CanUseWhenWithClauseAndReportErrors:");
            results.ErrorMessages
                    .ToList()
                    .ForEach(x => Console.Out.WriteLine(x));
        }

        /// <summary>
        /// Should be able to execute a chain a queries and use an "In" clause
        /// as in:  In(45,47) as well as: In(10.5, 45.5)
        /// </summary>
        [Test]
        public void CanUseWhenWithInClauseLambdaOnEmployeeAge()
        {
            var inValues = new List<int>() { 415, 416, 417 };
            var employee = new Employee() { LastName = "Robbins", FirstName = "Dave", Age = 45, Salary = 1000 };

            var result = employee.Enforce("Employee", true)
                                       .When("Age", Compare.In, inValues);

            Assert.IsFalse(result.IsValid);

            Console.Out.WriteLine("Test CanUseWhenWithInClauseLambdaOnEmployeeAge:");
            result.ErrorMessages
                    .ToList()
                    .ForEach(x => Console.Out.WriteLine(x));

            var newValues = new List<int>() { 45, 47, 34};
            
            var newResult = employee.Enforce("Employee", true)
                                        .When("Age", Compare.In, newValues);

            Assert.IsTrue(newResult.IsValid);

            var salaryRange = new List<decimal>() { 1000, 2000.7m, 2500.124m};
            var salResult = employee.Enforce("Employee", true)
                                        .When("Salary", Compare.In, salaryRange);

            Assert.IsTrue(salResult.IsValid);
        }
    }
}
