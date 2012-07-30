Janga - A Validation Framework with a fluent API
================================================

Write validation code that looks like this:

``` csharp
bool passed = employee.Enforce()
              .When("Age", Compares.IsGreaterThan, 45)
              .When("Department", Compares.In, deptList)
              .IsValid();
if(passed)
{
    SomeProcess();
}
```

For background read this blog post http://activeengine.wordpress.com/2010/09/26/janga-a-validation-framework-with-a-fluent-api/
