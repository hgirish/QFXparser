using System;

namespace QFXparser
{
   // [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    internal class NodeNameAttribute : Attribute
    {
        private string _openTag;
        private string _closeTag;

        public NodeNameAttribute(string openTag, string closeTag = "")
        {
            _openTag = openTag;
            _closeTag = closeTag;
        }

        public string OpenTag
        {
            get
            {
                return _openTag;
            }
        }

        public string CloseTag
        {
            get
            {
                return _closeTag;
            }
        }
        public override object TypeId { get { return this; } }
    }
}