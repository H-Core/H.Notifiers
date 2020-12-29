using System;
using System.Threading.Tasks;
using System.Timers;
using H.Core.Notifiers;

namespace H.Notifiers
{
    /// <summary>
    /// 
    /// </summary>
    public class TimerNotifier : Notifier
    {
        #region Properties

        private int _intervalInMilliseconds;

        /// <summary>
        /// 
        /// </summary>
        public int IntervalInMilliseconds {
            get => _intervalInMilliseconds;
            set {
                _intervalInMilliseconds = value;

                if (value <= 0)
                {
                    return;
                }

                Timer?.Dispose();
                Timer = new Timer(value);
                Timer.Elapsed += OnElapsed;
                Timer.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(IntervalInMilliseconds);
            set => IntervalInMilliseconds = (int)value.TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Frequency { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int RequiredCount { get; set; }

        private Timer? Timer { get; set; }

        private int CurrentCount { get; set; }
        private DateTime LastEventTime { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TimerNotifier()
        {
            AddSetting(nameof(IntervalInMilliseconds), o => IntervalInMilliseconds = o, Positive, int.MaxValue);
            AddSetting(nameof(Frequency), o => Frequency = o, NotNegative, 0);
            AddSetting(nameof(RequiredCount), o => RequiredCount = o, Positive, 1);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            Timer?.Stop();
            Timer?.Dispose();
            Timer = null;

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Task<bool> OnResultAsync()
        {
            return Task.FromResult(true);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnResult()
        {
            return true;
        }

        private async void OnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var value = OnResult() && await OnResultAsync().ConfigureAwait(false);
                if (!value)
                {
                    CurrentCount = 0;
                    return;
                }

                CurrentCount++;
                if (CurrentCount < RequiredCount ||
                    DateTime.Now < LastEventTime.AddMilliseconds(Frequency))
                {
                    return;
                }

                OnEventOccurred();
                LastEventTime = DateTime.Now;
                CurrentCount = 0;
            }
            catch (Exception exception)
            {
                OnExceptionOccurred(exception);

                CurrentCount = 0;
            }
        }

        #endregion

    }
}
