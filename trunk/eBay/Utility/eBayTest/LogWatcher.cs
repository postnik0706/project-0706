using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Utility;

namespace eBayTest
{
    public class LogWatcher
    {
        FileStream Stream;
        StreamReader Reader;

        public string Filename { get; set; }
        public RichTextBox Control { get; set; }
        public Form1 Parent { get; set; }
        public bool Quit { get; set; }

        public LogWatcher(string Filename, Form1 Parent, RichTextBox Control)
        {
            this.Filename = Filename;
            this.Control = Control;
            this.Parent = Parent;
            Quit = false;

            ThreadPool.QueueUserWorkItem(t =>
                {
                    RefreshControl();
                });
        }

        private void RefreshControl()
        {
            string Contents = "";

            while (!Quit)
            {
                eBayClass.LogFileAccess.WaitOne();

                if (File.Exists(Filename))
                {
                    try
                    {
                        using (Stream = new System.IO.FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (Reader = new System.IO.StreamReader(Stream))
                            {
                                Contents = Reader.ReadToEnd();
                            }
                        }
                    }
                    finally
                    {
                        eBayClass.LogFileReader.Set();
                    }
                }

                Parent.Invoke(Parent.OnRefresh, Contents);
            }
        }
        
        public void OnChanged(object o, FileSystemEventArgs e)
        {
            RefreshControl();
        }
    }
}