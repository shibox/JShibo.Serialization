﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// Specifies the type of Json token.
    /// </summary>
    internal enum JsonToken
    {
        /// <summary>
        /// This is returned by the <see cref="JsonReader"/> if a <see cref="JsonReader.Read"/> method has not been called. 
        /// </summary>
        None,
        /// <summary>
        /// An object start token.
        /// </summary>
        StartObject,
        /// <summary>
        /// An array start token.
        /// </summary>
        StartArray,
        /// <summary>
        /// A constructor start token.
        /// </summary>
        StartConstructor,
        /// <summary>
        /// An object property name.
        /// </summary>
        PropertyName,
        /// <summary>
        /// A comment.
        /// </summary>
        Comment,
        /// <summary>
        /// Raw JSON.
        /// </summary>
        Raw,
        /// <summary>
        /// An integer.
        /// </summary>
        Integer,
        /// <summary>
        /// A float.
        /// </summary>
        Float,
        /// <summary>
        /// A string.
        /// </summary>
        String,
        /// <summary>
        /// A boolean.
        /// </summary>
        Boolean,
        /// <summary>
        /// A null token.
        /// </summary>
        Null,
        /// <summary>
        /// An undefined token.
        /// </summary>
        Undefined,
        /// <summary>
        /// An object end token.
        /// </summary>
        EndObject,
        /// <summary>
        /// An array end token.
        /// </summary>
        EndArray,
        /// <summary>
        /// A constructor end token.
        /// </summary>
        EndConstructor,
        /// <summary>
        /// A Date.
        /// </summary>
        Date,
        /// <summary>
        /// Byte data.
        /// </summary>
        Bytes
    }
}
