using System;
using System.Collections.Generic;
#if !NET20 && (!SILVERLIGHT || WINDOWS_PHONE) && !PORTABLE40
//using System.Linq;
#endif

using System.Text;

namespace JShibo.Serialization
{
    /// <summary>
    /// Specifies how strings are escaped when writing JSON text.
    /// 指定字符串在写入文本的时候如何进行转义
    /// </summary>
    public enum StringEscape
    {
        /// <summary>
        /// Only control characters (e.g. newline) are escaped.
        /// </summary>
        Default,
        /// <summary>
        /// All non-ASCII and control characters (e.g. newline) are escaped.
        /// </summary>
        EscapeNonAscii,
        /// <summary>
        /// HTML (&lt;, &gt;, &amp;, &apos;, &quot;) and control characters (e.g. newline) are escaped.
        /// </summary>
        EscapeHtml
    }
}
