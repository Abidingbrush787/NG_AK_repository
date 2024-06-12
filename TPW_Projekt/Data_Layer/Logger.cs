using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Data_Layer
{
    public class Logger
    {
        private static readonly object _lock = new object();
        private readonly string _filePath;

        public Logger()
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
                string logDirectory = Path.Combine(projectDirectory, "Logs");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                _filePath = Path.Combine(logDirectory, "log.txt");

                // Usunięcie istniejącego pliku logów, aby zresetować logi przy każdym uruchomieniu
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Logger initialization: {ex.Message}");
            }
        }

        public void LogBalls(List<Ball> balls)
        {
            Task.Run(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        var logEntry = new
                        {
                            Timestamp = DateTime.UtcNow,
                            Balls = balls
                        };
                        var json = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
                        File.AppendAllText(_filePath, json + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception during LogBalls: {ex.Message}");
                    }
                }
            });
        }

        public void Log(string message)
        {
            Task.Run(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        var logEntry = new
                        {
                            Timestamp = DateTime.UtcNow,
                            Message = message
                        };
                        var json = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
                        File.AppendAllText(_filePath, json + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception during Log: {ex.Message}");
                    }
                }
            });
        }
    }

}
