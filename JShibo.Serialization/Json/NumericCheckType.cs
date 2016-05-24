using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// 对于数值序列化的时候检测的排序方式
    /// </summary>
    public enum NumericCheckType
    {
        /// <summary>
        /// 从最小值开始检测
        /// </summary>
        Min,
        /// <summary>
        /// 从中间开始检测
        /// </summary>
        Middle,
        /// <summary>
        /// 从最大值开始检测
        /// </summary>
        Max
    }
}
