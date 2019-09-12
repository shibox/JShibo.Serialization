using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Soc
{
    public class SocSerializer
    {
        public static ObjectBufferContext Create<T>()
        {
            return Create(typeof(T));
        }

        public static ObjectBufferContext Create(Type type)
        {
            return ShiboObjectBufferSerializer.GetContext(type);
        }

        public static ObjectStreamContext CreateStreamContext<T>()
        {
            return CreateStreamContext(typeof(T));
        }

        public static ObjectStreamContext CreateStreamContext(Type type)
        {
            return ShiboObjectStreamSerializer.GetContext(type);
        }
    }
}
