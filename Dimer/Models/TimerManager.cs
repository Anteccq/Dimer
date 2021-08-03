using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimer.Models
{
    public interface ITimeManager
    {
        int Add(MessageTimer timer);
        MessageTimer Find(int key);
        bool Exists(int key);
        bool TryRemoveIfExists(int key);
        bool TryRemove(int key);
    }
    public class TimerManager : ITimeManager
    {
        private ConcurrentDictionary<int, MessageTimer> Timers { get; } = new();

        public int Add(MessageTimer timer)
        {
            if (timer is null) throw new ArgumentException($"timer can not be null.");
            var id = Timers.Count;
            Timers.TryAdd(id, timer);
            return id;
        }

        public MessageTimer Find(int key)
        {
            Timers.TryGetValue(key, out var timer);
            return timer;
        }

        public bool Exists(int key)
            => Timers.ContainsKey(key);

        public bool TryRemoveIfExists(int key)
        {
            return Exists(key) && Timers.TryRemove(key, out _);
        }

        public bool TryRemove(int key)
            => Timers.TryRemove(key, out _);
    }
}
