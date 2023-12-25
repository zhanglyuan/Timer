using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Events
{
    public class Mediator
    {
        private static readonly object _eventAggregatorLock = new object();

        private static IEventAggregator _eventAggregator;

        /// <summary>
        /// Gets the <see cref="IEventAggregator"/> instance.
        /// </summary>
        public static IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                {
                    lock (_eventAggregatorLock)
                    {
                        if (_eventAggregator == null)
                        {
                            _eventAggregator = new AppEventAggregator();
                        }
                    }
                }

                return _eventAggregator;
            }
        }
    }
}