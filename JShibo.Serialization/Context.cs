
using JShibo.Serialization.Csv;
using JShibo.Serialization.Json;
using JShibo.Serialization.Soc;
using JShibo.Serialization.Transpose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    public class ObjectBufferContext : ObjectContext<ObjectWriter, ObjectReader, ObjectWriterSize>
    {
        public byte[] Serialize(object graph)
        {
            var buffer = new ObjectWriter();
            Serializer(buffer, graph);
            return buffer.ToArray();
        }

        internal void Serialize(Stream stream, object graph)
        {
            int size = ObjectBufferSerializer.GetSize(this, graph);
            ObjectWriter buffer = new ObjectWriter(size);
            Serializer(buffer, graph);
            stream.Write(buffer.GetBuffer(), 0, buffer.position);
        }

        public void Serialize(ObjectWriter stream, object graph)
        {
            Serializer(stream, graph);
        }

        public T Deserialize<T>(ObjectReader stream)
        {
            //if (info.IsJsonBaseType == false)
            //    stream.position++;

            stream.desers = this.Deserializers;
            stream.types = this.Types;
            stream.typeCounts = this.TypeCounts;
            //stream.nameCounts = info.NameCounts;
            //stream.names = info.Names;
            //stream.nameLens = info.NameLens;

            object value = Deserializer(stream);
            return (T)value;
        }

    }

    public class ObjectStreamContext : ObjectContext<ObjectStreamWriter, ObjecStreamReader, ObjectStreamSize>
    {


    }

    public class JsonStringContext : JsonContext<JsonString, JsonUstring, JsonStringSize>
    {
    }

    public class CsvStringContext : CsvContext<CsvStringWriter, CsvStringReader, CsvStringSize>
    {
        public int HeaderSize;

        /// <summary>
        /// 获取csv头最大占用的长度
        /// </summary>
        /// <returns></returns>
        public int GetHeaderSize()
        {
            int size = 0;
            for (int i = 0; i < Names.Length; i++)
                size += (Names[i].Length << 1) + 2;
            return size;
        }
    }

    public class CsvBytesContext : CsvContext<Utf8CsvWriter, Utf8CsvReader, CsvBytesSize>
    {
        public int HeaderSize;

        public int GetHeaderSize()
        {
            int size = 0;
            for (int i = 0; i < Names.Length; i++)
                size += (Names[i].Length << 1) + 2;
            return size;
        }
    }

    public class XmlStringContext : XmlContext<CsvStringWriter, CsvStringReader, CsvStringSize>
    {
        

    }

    public class ConvertContext : ConvertContext<PivotEncode, PivotDecode>
    {


    }

    


}
