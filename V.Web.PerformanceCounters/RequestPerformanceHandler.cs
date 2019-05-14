using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace V.Web.PerformanceCounters
{
    public class RequestPerformanceHandler : ActionFilterAttribute, IActionFilter
    {
        private readonly WebPerformanceCounter _PerformanceCounter;

        /// <summary>
        /// Initializes ActionFilterAttribute that will server for Counting Perfromance of requests
        /// </summary>
        /// <param name="categoryName">Name of the category(How it will be displayed in PerformanceCounter Winodw)</param>
        /// <param name="countOperationsASecond">Should the service count operations per second</param>
        /// <param name="countAverageTimePerOperation">Should the service count avergage time that operation took</param>
        public RequestPerformanceHandler(string categoryName, string categoryDescription = "",
            bool countOperationsASecond = true, bool countAverageTimePerOperation = true)
        {
            _PerformanceCounter = new WebPerformanceCounter(categoryName, categoryDescription, countOperationsASecond, countAverageTimePerOperation);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _PerformanceCounter.Start();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _PerformanceCounter.Stop();
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
