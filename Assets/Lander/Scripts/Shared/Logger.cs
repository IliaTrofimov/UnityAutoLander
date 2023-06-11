using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GlobalShared
{
    public class ArrayLogger<T> : Logger<T[]>
    {
        public ArrayLogger(string filename, int maxItemsWaiting, TimeSpan maxWaitingTime, string separator = ";")
            : base(filename, maxItemsWaiting, maxWaitingTime, (T[] arr) => string.Join(separator, arr))
        {
        }

        /// <summary>Записать данные в журнал.</summary>
        public new void Log(params T[] data)
        {
            base.Log(data);
        }

        /// <summary><inheritdoc cref="Log(T)"/></summary>
        public new async Task LogAsync(params T[] data)
        {
            await base.LogAsync(data);
        }
    }

    /// <summary>Файловый логгер с возможностью отложенной записи в файл.</summary>
    /// <typeparam name="T">Тип сохраняемых данных.</typeparam>
    public class Logger<T>
    {
        private static object locker = new();

        private static HashSet<string> files = new();

        protected LinkedList<T> cachedItems = new(); 

        protected Func<T, string> stringConverter = (T data) => data.ToString();

        protected DateTime? lastLogTime = null;


        public string Filename { get; protected set; }

        public int MaxItemsWaiting { get; private set; } = 50;

        public TimeSpan MaxWaitingTime { get; private set; } = new TimeSpan(0, 1, 0);


        public Logger(string filename, int maxItemsWaiting, TimeSpan maxWaitingTime, Func<T, string> stringConverter)
            : this(filename, maxItemsWaiting, maxWaitingTime)
        {
            files.Add(filename);
            this.stringConverter = stringConverter;
        }

        public Logger(string filename, int maxItemsWaiting, TimeSpan maxWaitingTime)
            : this(filename)
        {
            files.Add(filename);
            MaxItemsWaiting = maxItemsWaiting;
            MaxWaitingTime = maxWaitingTime;
        }

        public Logger(string filename, Func<T, string> stringConverter)
            : this(filename)
        {
            files.Add(filename);
            this.stringConverter = stringConverter;
        }

        public Logger(string filename)
        {
            files.Add(filename);
            Filename = filename;
            if (!File.Exists(filename))
                File.Create(filename);
        }


        /// <summary>Записать данные в журнал.</summary>
        public virtual void Log(T data)
        {
            if (!lastLogTime.HasValue)
                lastLogTime = DateTime.Now;

            if (cachedItems.Count < MaxItemsWaiting && (DateTime.Now - lastLogTime) < MaxWaitingTime)
                cachedItems.AddLast(data);
            else
            {
                cachedItems.AddLast(data);

                lock (locker)
                {
                    File.AppendAllLines(Filename, cachedItems.Select(stringConverter));
                }
                cachedItems.Clear();
                lastLogTime = DateTime.Now;
            }
        }

        /// <summary><inheritdoc cref="Log(T)"/></summary>
        public virtual async Task LogAsync(T data)
        {
            if (!lastLogTime.HasValue)
                lastLogTime = DateTime.Now;

            if (cachedItems.Count < MaxItemsWaiting && (DateTime.Now - lastLogTime) < MaxWaitingTime)
                cachedItems.AddLast(data);
            else
            {
                cachedItems.AddLast(data);
                await File.AppendAllLinesAsync(Filename, cachedItems.Select(stringConverter));
                cachedItems.Clear();
                lastLogTime = DateTime.Now;
            }
        }

        /// <summary>Принудительно выгрузить журнал в файл.</summary>
        public virtual void ForceLog()
        {
            File.AppendAllLines(Filename, cachedItems.Select(stringConverter));
            cachedItems.Clear();
            lastLogTime = DateTime.Now;
        }

        /// <summary><inheritdoc cref="ForceLog()"/></summary>
        public virtual async Task ForceLogAsync()
        {
            await File.AppendAllLinesAsync(Filename, cachedItems.Select(stringConverter));
            cachedItems.Clear();
            lastLogTime = DateTime.Now;
        }
    }
}