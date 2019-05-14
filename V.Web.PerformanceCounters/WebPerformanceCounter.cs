using System.Diagnostics;

namespace V.Web.PerformanceCounters
{
    public class WebPerformanceCounter
    {
        private readonly System.Diagnostics.PerformanceCounter _OperationsPerSecond;
        private readonly System.Diagnostics.PerformanceCounter _AverageDuration;
        private readonly System.Diagnostics.PerformanceCounter _AverageDurationBase;
        private System.Diagnostics.Stopwatch _Stopwatch;

        /// <summary>
        /// Creates all the instances for counting performance for API calls
        /// </summary>
        /// <param name="categoryName">Name of the category(How it will be displayed in PerformanceCounter Winodw)</param>
        /// <param name="categoryDescription">Description of the category(How it will be displayed in PerformanceCounter Winodw)</param>
        /// <param name="countOperationsASecond">Should the service count operations per second</param>
        /// <param name="countAverageTimePerOperation">Should the service count avergage time that operation took</param>
        public WebPerformanceCounter(string categoryName, string categoryDescription, bool countOperationsASecond, bool countAverageTimePerOperation)
        {
            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                CounterCreationDataCollection counters = new CounterCreationDataCollection();

                if (countOperationsASecond)
                {
                    // 1. counter for counting operations per second:
                    //        PerformanceCounterType.RateOfCountsPerSecond32
                    CounterCreationData opsPerSecond = new CounterCreationData();
                    opsPerSecond.CounterName = "# operations / sec";
                    opsPerSecond.CounterHelp = "Number of operations executed per second";
                    opsPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
                    counters.Add(opsPerSecond);
                }

                if (countAverageTimePerOperation)
                {
                    // 2. counter for counting average time per operation:
                    //                 PerformanceCounterType.AverageTimer32
                    CounterCreationData avgDuration = new CounterCreationData();
                    avgDuration.CounterName = "average time per operation";
                    avgDuration.CounterHelp = "Average duration per operation execution";
                    avgDuration.CounterType = PerformanceCounterType.AverageTimer32;
                    counters.Add(avgDuration);


                    // 3. base counter for counting average time
                    //         per operation: PerformanceCounterType.AverageBase
                    CounterCreationData avgDurationBase = new CounterCreationData();
                    avgDurationBase.CounterName = "average time per operation base";
                    avgDurationBase.CounterHelp = "Average duration per operation execution base";
                    avgDurationBase.CounterType = PerformanceCounterType.AverageBase;
                    counters.Add(avgDurationBase);
                }

                // create new category with the counters above
                PerformanceCounterCategory.Create(categoryName, categoryDescription, PerformanceCounterCategoryType.Unknown, counters);
            }

            if (countOperationsASecond)
            {
                _OperationsPerSecond = new System.Diagnostics.PerformanceCounter
                {
                    CategoryName = categoryName,
                    CounterName = "# operations / sec",
                    MachineName = ".",
                    ReadOnly = false
                };
            }

            if (countAverageTimePerOperation)
            {
                _AverageDuration = new System.Diagnostics.PerformanceCounter
                {
                    CategoryName = categoryName,
                    CounterName = "average time per operation",
                    MachineName = ".",
                    ReadOnly = false
                };

                _AverageDurationBase = new System.Diagnostics.PerformanceCounter
                {
                    CategoryName = categoryName,
                    CounterName = "average time per operation base",
                    MachineName = ".",
                    ReadOnly = false
                };
            }
        }

        /// <summary>
        /// Start counting time (Happens at the beggining of the request)
        /// </summary>
        public void Start()
        {
            _Stopwatch = System.Diagnostics.Stopwatch.StartNew();
        }

        /// <summary>
        /// Stops counting time (Happens at the end of the request)
        /// </summary>
        public void Stop()
        {
            _Stopwatch.Stop();

            if (_OperationsPerSecond != null)
                _OperationsPerSecond.Increment();

            if (_AverageDuration != null & _AverageDurationBase != null)
            {
                var elapsedTicks = _Stopwatch.ElapsedTicks;
                _AverageDuration.IncrementBy(elapsedTicks);
                _AverageDurationBase.Increment();
            }
        }

    }
}
