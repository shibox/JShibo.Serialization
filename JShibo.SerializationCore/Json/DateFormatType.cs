using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// 日期时间格式化类型
    /// Specifies how dates are formatted when writing JSON text.
    /// </summary>
    internal enum DateFormatType
    {
        /// <summary>
        /// Dates are written in the ISO 8601 format, e.g. "2012-03-21T05:40Z".
        /// </summary>
        IsoDateFormat,
        /// <summary>
        /// Dates are written in the Microsoft JSON format, e.g. "\/Date(1198908717056)\/".
        /// </summary>
        MicrosoftDateFormat
    }
}
