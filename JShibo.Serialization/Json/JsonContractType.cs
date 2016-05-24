using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    internal enum JsonContractType
    {
        None,
        Object,
        Array,
        Primitive,
        String,
        Dictionary,
#if !(NET35 || NET20 || PORTABLE40)
        Dynamic,
#endif
#if !(SILVERLIGHT || NETFX_CORE || PORTABLE || PORTABLE40)
        Serializable,
#endif
        Linq
    }
}
