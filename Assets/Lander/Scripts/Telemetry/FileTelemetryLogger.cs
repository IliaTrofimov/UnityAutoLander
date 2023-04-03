using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Shared;
using Movement;

namespace Telemetry
{
    public class FileTelemetryLogger : ConsoleTelemetryLogger, IDisposable
    {
        public string Filename;
        private StreamWriter writer;

        protected override async Task LogData()
        {
            writer.WriteLine(data.ToString());
        }

        public void Dispose()
        {
            ((IDisposable)writer).Dispose();
        }


        private void Start()
        {
            writer = new StreamWriter(Filename, false);
        }

        private void OnApplicationPause(bool pause)
        {
            if (writer != null)
                writer.Close();
            writer = new StreamWriter(Filename, true);
        }

        private void OnApplicationQuit()
        {
            writer.Close();
        }
    }
}