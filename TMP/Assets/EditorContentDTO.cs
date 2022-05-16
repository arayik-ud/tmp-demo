using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class EditorContentDto
    {
        public string text;
        public List<string> images;
        public int scale;
    }
}