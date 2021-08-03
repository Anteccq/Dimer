using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimer.Models
{
    public class MessageTimer : IDisposable
    {
        private readonly IObservable<long> _timer;
        private IDisposable _subscription;
        public TimeSpan Time { get; }

        public event Action Finished;

        public string Message { get; set; }

        public MessageTimer(TimeSpan time)
        {
            Time = time;
            _timer = Observable
                .Interval(Time)
                .FirstAsync();
        }

        public void Start(Func<string, Task> execute)
        {
            _subscription = _timer
                .Select(x => Message)
                .Finally(() => Finished?.Invoke())
                .Subscribe(
                    async _ => await execute(Message),
                    async ex => await execute(ExceptionToMessage(ex)));
        }

        public void Cancel() => _subscription?.Dispose();

        private static string ExceptionToMessage(Exception ex)
            => ex.InnerException switch
                {
                    OperationCanceledException => $"キャンセルされました。",
                    _ => $"不明なエラーによりタイマーが終了しました。"
                };

        public void Dispose()
        {
            _subscription?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
