using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// 检查转义的模式
    /// </summary>
    public enum CheckEscape
    {
        /// <summary>
        /// 不检查转义
        /// </summary>
        None,
        /// <summary>
        /// 检查可能的转义
        /// </summary>
        May,
        /// <summary>
        /// 必须检查转义
        /// </summary>
        Must,
        /// <summary>
        /// 只检查引号转义
        /// </summary>
        OnlyCheckQuote,
    }
}
