
using Avalonia.Controls;
using System;

namespace Client.Utilities
{
    public static class AppLog
    {
        private static TextBox _textBox;
        private static bool _initialized;

        public static void Initialize(TextBox textBox)
        {
            if (_initialized) return;

            _textBox = textBox;

            _initialized = true;
        }

        public static void Log(string message)
        {
            if (!_initialized) return;

            _textBox.Text += $"[{DateTime.Now}] {message}\n";
        }
        public static void LogError(string message)
        {
            if (!_initialized) return;

            _textBox.Text += $"[{DateTime.Now}] ERROR: {message}\n";
        }

        public static void ClearLog()
        {
            if (!_initialized) return;

            _textBox.Clear();
        }
    }
}
