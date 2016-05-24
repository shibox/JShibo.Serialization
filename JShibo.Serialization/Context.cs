
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
    public class ObjectBufferContext : ObjectContext<ObjectBuffer, ObjectUbuffer, ObjectBufferSize>
    {
        public byte[] Serialize(object graph)
        {
            ObjectBuffer buffer = new ObjectBuffer();
            Serializer(buffer, graph);
            return buffer.ToArray();
        }

        internal void Serialize(Stream stream, object graph)
        {
            int size = ShiboObjectBufferSerializer.GetSize(this, graph);
            ObjectBuffer buffer = new ObjectBuffer(size);
            Serializer(buffer, graph);
            stream.Write(buffer.GetBuffer(), 0, buffer.position);
        }

        public void Serialize(ObjectBuffer stream, object graph)
        {
            Serializer(stream, graph);
        }

        public T Deserialize<T>(ObjectUbuffer stream)
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

    public class ObjectStreamContext : ObjectContext<ObjectStream, ObjectUstream, ObjectStreamSize>
    {


    }

    public class JsonStringContext : JsonContext<JsonString, JsonUstring, JsonStringSize>
    {
    }

    public class CsvStringContext : CsvContext<CsvString, CsvUstring, CsvStringSize>
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

    public class XmlStringContext : XmlContext<CsvString, CsvUstring, CsvStringSize>
    {
        

    }

    public class ConvertContext : XmlContext<PivotEncode, PivotDecode, PivotEncodeSize>
    {


    }

    


}
