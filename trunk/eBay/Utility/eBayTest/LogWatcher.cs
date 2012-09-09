using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace eBayTest
{
    public class LogWatcher
    {
        FileStream Stream;
        StreamReader Reader;
        FileSystemWatcher Watcher;

        public string Filename { get; set; }
        public RichTextBox Control { get; set; }
        public Form1 Parent { get; set; }

        public LogWatcher(string Filename, Form1 Parent, RichTextBox Control)
        {
            this.Filename = Filename;
            this.Control = Control;
            this.Parent = Parent;

            RefreshControl();
            Watcher = new FileSystemWatcher();
            Watcher.Path = Path.GetDirectoryName(Filename);
            Watcher.NotifyFilter = (NotifyFilters.LastWrite | NotifyFilters.Size);
            Watcher.Filter = Path.GetFileName(Filename);
            Watcher.Changed += OnChanged;
            Watcher.EnableRaisingEvents = true;
        }

        private void RefreshControl()
        {
            string Contents;

            using (Stream = new System.IO.FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Reader = new System.IO.StreamReader(Stream))
                {
                    Contents = Reader.ReadToEnd();
                }
            }

            Parent.Invoke(Parent.OnRefresh, Contents);
        }
        
        public void OnChanged(object o, FileSystemEventArgs e)
        {
            RefreshControl();
        }

        public void Pause()
        {
            Watcher.EnableRaisingEvents = false;
        }
        
        public void Resume()
        {
            RefreshControl();
            Watcher.EnableRaisingEvents = true;
        }
    }
}
