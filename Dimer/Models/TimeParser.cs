using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dimer.Models
{
    public class TimeParser
    {
        private const int MaxArrayLength = 4;
        public static bool TryParse(string timeString, out TimeSpan time)
        {
            time = TimeSpan.Zero;
            if (string.IsNullOrWhiteSpace(timeString) || timeString.StartsWith(':')) return false;
            if (timeString.Length != 1) return TryMultipleTimeParse(timeString, out time);
            if (!int.TryParse(timeString, out var i)) return false;

            time = TimeSpan.FromSeconds(i);
            return true;

        }

        private static bool TryMultipleTimeParse(string timeString, out TimeSpan time)
        {
            time = TimeSpan.Zero;
            var seek = timeString.Length - 1;
            var index = timeString.Length - 1;
            var count = 0;
            var numbers = new int[MaxArrayLength];
            try
            {
                while ((index = timeString.LastIndexOf(':', index)) != -1)
                {
                    count++;
                    if (count > MaxArrayLength-1) return false;

                    if (!int.TryParse(
                        timeString.AsSpan().Slice(index+1, seek-index),
                        out numbers[MaxArrayLength-count]))
                        return false;

                    seek = index - 1;
                    index--;
                }

                if (!int.TryParse(
                    timeString.AsSpan().Slice(index+1, seek-index),
                    out numbers[MaxArrayLength-count-1])) return false;
                time = new TimeSpan(numbers[0], numbers[1], numbers[2], numbers[3]);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
