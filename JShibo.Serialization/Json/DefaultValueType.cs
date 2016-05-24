using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    [Flags]
    internal enum DefaultValueType
    {
        /// <summary>
        /// Include members where the member value is the same as the member's default value when serializing objects.
        /// Included members are written to JSON. Has no effect when deserializing.
        /// </summary>
        Include = 0,
        /// <summary>
        /// Ignore members where the member value is the same as the member's default value when serializing objects
        /// so that is is not written to JSON.
        /// This option will ignore all default values (e.g. <c>null</c> for objects and nullable typesl; <c>0</c> for integers,
        /// decimals and floating point numbers; and <c>false</c> for booleans). The default value ignored can be changed by
        /// placing the <see cref="DefaultValueAttribute"/> on the property.
        /// </summary>
        Ignore = 1,
        /// <summary>
        /// Members with a default value but no JSON will be set to their default value when deserializing.
        /// </summary>
        Populate = 2,
        /// <summary>
        /// Ignore members where the member value is the same as the member's default value when serializing objects
        /// and sets members to their default value when deserializing.
        /// </summary>
        IgnoreAndPopulate = Ignore | Populate
    }
}
