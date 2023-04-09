using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GlobalShared
{
    /// <summary>Файловый логгер с возможностью отложенной записи в файл.</summary>
    /// <typeparam name="T">Тип сохраняемых данных.</typeparam>
    public class Logger<T>
    {
        protected LinkedList<T> cachedItems = new(); 

        protected Func<T, string> stringConverter = (T data) => data.ToString();

        protected DateTime? lastLogTime = null;


        public string Filename { get; protected set; }

        public int MaxItemsWaiting { get; private set; } = 50;

        public TimeSpan MaxWaitingTime { get; private set; } = new TimeSpan(0, 1, 0);


        public Logger(string filename, int maxItemsWaiting, TimeSpan maxWaitingTime, Func<T, string> stringConverter)
            : this(filename, maxItemsWaiting, maxWaitingTime)
        {
            this.stringConverter = stringConverter;
        }

        public Logger(string filename, int maxItemsWaiting, TimeSpan maxWaitingTime)
            : this(filename)
        {
            MaxItemsWaiting = maxItemsWaiting;
            MaxWaitingTime = maxWaitingTime;
        }

        public Logger(string filename, Func<T, string> stringConverter)
            : this(filename)
        {
            this.stringConverter = stringConverter;
        }

        public Logger(string filename)
        {
            Filename = filename;
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
                File.AppendAllLines(Filename, cachedItems.Select(stringConverter));
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