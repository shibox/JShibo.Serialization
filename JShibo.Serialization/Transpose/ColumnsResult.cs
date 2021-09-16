using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public class DataColumn
    {
        /// <summary>
        /// 对应的列类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 对应的列值数组
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 对应的列名称
        /// </summary>
        public string Name { get; set; }
    }

    public class ColumnResultList
    {

    }

    public class ColumnArray
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Data { get; set; }

    }

}
