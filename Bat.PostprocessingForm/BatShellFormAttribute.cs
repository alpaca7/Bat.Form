using System;

namespace Bat.Shell.PostprocessingForm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BatShellFormAttribute : System.Attribute
    {
        private string _title;//窗口标题

        private string _author;//开发者

        private string _version;//版本号

        public BatShellFormAttribute(string title)
        {
            _title = title;
        }

        public BatShellFormAttribute(string title, string author, string version)
        {
            _title = title;
            _author = author;
            _version = version;
        }


        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Author
        {
            get
            {
                return _author;
            }
        }

        public string Version
        {
            get
            {
                return _version;
            }
        }
    }
}
