using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame.Utility
{
    public class DebugTimer: IDisposable
    {
        public IEnumerable<Entry> Entries => _entries;
        
        private readonly List<Entry> _entries = new List<Entry>();
        private readonly DateTime _start;
        private DateTime _last;
        
        private readonly Action<string> _reporter;
        private readonly bool _richText;

        public DebugTimer(Action<string> reporter, bool richText = true)
        {
            _start = DateTime.Now;
            _last = _start;
            _reporter = reporter;
            _richText = richText;
        }

        public DebugTimer() : this(null, true) { }


        public void Mark(string ID)
        {
            var now = DateTime.Now;
            _entries.Add(new Entry(ID, now-_last, now-_start));
            _last = now;
        }
        
        public class Entry
        {
            public readonly string ID;
            public readonly TimeSpan Delta;
            public readonly TimeSpan SinceStart;

            public Entry(string id, TimeSpan delta, TimeSpan sinceStart)
            {
                ID = id;
                Delta = delta;
                SinceStart = sinceStart;
            }
        }

        public void Dispose()
        {
            if (_reporter == null) return;
            
            var length = _entries.Aggregate(
                _entries[0],
                (longest, next) =>
                    next.ID.Length > longest.ID.Length ? next : longest,
                entry => entry.ID.Length);
            var sb = new StringBuilder();
            sb.AppendLine(_richText ? "<b>[DebugTimer]</b>" : "[DebugTimer]");
            foreach (var entry in _entries)
            {
                var label = _richText ? $"<b>{entry.ID.PadRight(length)}:</b>" : $"{entry.ID.PadRight(length)}:"; 
                sb.AppendLine($"{label} {entry.Delta} ({entry.SinceStart})");
            }

            _reporter(sb.ToString());
        }
    }
}