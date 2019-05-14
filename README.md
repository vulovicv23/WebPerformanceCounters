Web Performance Counter
======
A simple library for code/performance instrumentation in .NET

Getting Started
==

### Installation


### Usage
Since this library uses ActionFilterAttributes, you can measure all or specific calls.

In the constructor, you will see these parameters:
``` csharp
        /// <summary>
        /// Initializes ActionFilterAttribute that will server for Counting Perfromance of requests
        /// </summary>
        /// <param name="categoryName">Name of the category(How it will be displayed in PerformanceCounter Winodw)</param>
        /// <param name="countOperationsASecond">Should the service count operations per second</param>
        /// <param name="countAverageTimePerOperation">Should the service count avergage time that operation took</param>
        public RequestPerformanceHandler(string categoryName, string categoryDescription = "",
            bool countOperationsASecond = true, bool countAverageTimePerOperation = true)
```

So, `categoryDescription`, `countnOperationsASecond`, `countAverageTimePerOperation` is optional, `categoryName` is mandatory.
By default, this will count number of operations per second and avergate time per operation.

For `categoryName`, we are usualy using it like: `ProjectName.Controller.Method` or if global `ProjectName.Global`.

`NOTE: PLEASE RUN VISUAL STUDIO AS ADMINISTRATOR WHEN RUNNING YOUR PROJECT WITH THIS LIBRARY`

In order to set global filter and measure all of the calls, please open `App_Start\WebApiConfig.cs` and add:
``` csharp
config.Filters.Add(new RequestPerformanceHandler("ProjectName.Global"));
```

In order to use it on a single controller, please use it as:

``` csharp
[RequestPerformanceHandler("ProjectName.ControllerName")]
public class ValuesController : Controller
```

In order to use it on a single call, please use it as:

``` csharp
[RequestPerformanceHandler("ProjectName.ControllerName.MethodName")]
public IEnumerable<string> Get()
```

When you fire up your project, counters will be created and you will need to open PerformanceCounter monitor in Windows, and there, you will find your counters by the `categoryName` you defined.