using System;
using System.Collections.Generic;

namespace SpaceGame.Utility
{
    public class DebugTimer
    {
        public IReadOnlyList<Entry> Entries => _entries;
        
        private readonly List<Entry> _entries = new List<Entry>();
        private readonly DateTime _start;
        private DateTime _last;

        public DebugTimer()
        {
            _start = DateTime.Now;
            _last = _start;
        }

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

            public override string ToString()
            {
                return $"{ID}: {Delta} ({SinceStart})";
            }
        }
    }
}