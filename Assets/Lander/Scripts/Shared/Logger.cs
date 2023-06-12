using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Lander.Shared
{
    public class ArrayLogger<T> : Logger<T[]>
    {
        public ArrayLogger(string filename, int skip, string separator = ";")
            : base(filename, skip, (T[] arr) => string.Join(separator, arr))
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

        /// <summary>Записать данные в журнал.</summary>
        public new void ForceLog(params T[] data)
        {
            base.ForceLog(data);
        }

        /// <summary><inheritdoc cref="Log(T)"/></summary>
        public new async Task ForceLogAsync(params T[] data)
        {
            await base.ForceLogAsync(data);
        }
    }

    /// <summary>Файловый логгер с возможностью отложенной записи в файл.</summary>
    /// <typeparam name="T">Тип сохраняемых данных.</typeparam>
    public class Logger<T>
    {
        private static HashSet<string> files = new();

        protected Func<T, string> stringConverter = (T data) => data.ToString();

      

        public string Filename { get; protected set; }

        public int SkipItems { get; private set; } = 0;
        private int skip = 0;
        private long loggingStart = DateTime.Now.Ticks;

        public Logger(string filename, int skip, Func<T, string> stringConverter)
        {
            SkipItems = skip;
            Filename = filename;

            if (!File.Exists(filename) && files.Add(Filename))
                File.Create(filename);
            this.stringConverter = stringConverter;
        }


        /// <summary>Записать данные в журнал.</summary>
        public virtual void Log(T data)
        {
            if (skip == SkipItems)
            {
                File.AppendAllText(Filename, $"{DateTime.Now.Ticks - loggingStart};{stringConverter(data)}\n");
                skip = 0;
            }
            else
            {
                skip++;
            }
        }

        /// <summary><inheritdoc cref="Log(T)"/></summary>
        public virtual async Task LogAsync(T data)
        {
            if (skip == SkipItems)
            {
                await File.AppendAllTextAsync(Filename, $"{DateTime.Now.Ticks - loggingStart};{stringConverter(data)}\n");
                skip = 0;
            }
            else
            {
                skip++;
            }
        }

        /// <summary>Принудительно выгрузить журнал в файл.</summary>
        public virtual void ForceLog(T data)
        {
            File.AppendAllText(Filename, $"{DateTime.Now.Ticks - loggingStart};{stringConverter(data)}\n");
            skip = 0;
        }

        /// <summary><inheritdoc cref="ForceLog()"/></summary>
        public virtual async Task ForceLogAsync(T data)
        {
            await File.AppendAllTextAsync(Filename, $"{DateTime.Now.Ticks - loggingStart};{stringConverter(data)}\n");
            skip = 0;
        }
    }
}