﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DotVVM.Framework.ViewModel.Serialization
{
    public class EncryptedValuesWriter
    {
        JsonWriter writer;
        JsonSerializer serializer;
        Stack<int> propertyIndices = new Stack<int>();
        int virtualNests = 0;
        int lastPropertyIndex = -1;

        public EncryptedValuesWriter(JsonWriter jsonWriter)
        {
            this.writer = jsonWriter;
            serializer = new JsonSerializer();
        }

        public void Nest() => Nest(lastPropertyIndex + 1);

        /// <summary>
        /// Indicates that serializer should nest to a inner object.
        /// Adds a new property to current object, and pushes the state to the stack.
        /// </summary>
        public void Nest(int property)
        {
            propertyIndices.Push(property);
            lastPropertyIndex = -1;
            virtualNests++;
        }

        /// <summary>
        /// Indicates that object has ended.
        /// Pops state from the stack.
        /// </summary>
        public void End()
        {
            if (virtualNests > 0)
            {
                virtualNests--;
            }
            else
            {
                writer.WriteEndObject();
            }
            lastPropertyIndex = propertyIndices.Pop();
        }

        /// <summary>
        /// Ensure that the subtree is empty (did not contain any protected value) and clear it.
        /// </summary>
        public void ClearEmptyNest()
        {
            if (virtualNests <= 0) throw new NotSupportedException("There is no empty (virtual) nest to be cleared.");
            virtualNests--;
            lastPropertyIndex = propertyIndices.Pop();
        }

        private void WritePropertyName(int index)
        {
            writer.WritePropertyName(index.ToString());
        }

        private void EnsureObjectStarted()
        {
            if (virtualNests > 0)
            {
                foreach (var p in propertyIndices.Take(virtualNests).Reverse())
                {
                    WritePropertyName(p); // the property was not written, -1 to write it
                    writer.WriteStartObject();
                }
                virtualNests = 0;
            }
        }

        public bool IsVirtualNest() => virtualNests > 0;

        /// <summary>
        /// Write a value to the object.
        /// </summary>
        public void WriteValue(int propertyIndex, object value)
        {
            EnsureObjectStarted();
            WritePropertyName(propertyIndex);
            lastPropertyIndex = propertyIndex;
            serializer.Serialize(writer, value);
        }
    }
}
