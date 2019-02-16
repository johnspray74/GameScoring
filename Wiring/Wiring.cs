using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;


namespace Wiring
{
    public static class Wiring
    {
        /// <summary>
        /// if object A (this) has a private field of an interface, and object B implements the interface, then wire them together. Returns this for fluent style programming.
        /// </summary>
        /// <param name="A">
        /// The object on which the method is called is the the wireFrom object
        /// </param> 
        /// <param name="B">The wireTo object</param> 
        /// <returns></returns>
        public static T WireTo<T>(this T A, object B)
        {
            if (B == null)
            {
                return A;
            }

            // achieve the following via reflection
            // A.field = (<type of interface>)B;
            // A.list.Add( (<type of interface>)B );

            var fieldInfos = A.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var pType = B.GetType();
            foreach (var implementedInterface in pType.GetInterfaces()) // consider every interface implemented by B 
            {
                // look for normal private fields first
                var fieldInfo = fieldInfos.FirstOrDefault(f => f.FieldType == implementedInterface && f.GetValue(A)==null); // find the first field in A that matches the interface type of B
                if (fieldInfo != null)  // there is a match
                {
                    fieldInfo.SetValue(A, B);  // do the wiring
                    continue;  // could be more than one interface to wire
                }

                // do the same as above for private fields that are a list of the interface of the matching type
                foreach (var info in fieldInfos)
                {
                    if (!info.FieldType.IsGenericType)
                    {
                        continue;
                    }
                    var listField = info.GetValue(A);
                    
                    var genericArguments = info.FieldType.GetGenericArguments();
                    if (genericArguments.Any(t => t == implementedInterface))
                    {
                        if (listField == null)
                        {
                            var listRef = typeof(List<>);
                            Type[] listParam = { implementedInterface };
                            listField = Activator.CreateInstance(listRef.MakeGenericType(listParam));
                            info.SetValue(A, listField);
                        }

                        listField.GetType().GetMethod("Add").Invoke(listField, new []{ B });
                        break;
                    }
                    
                }
            }
            return A;
        }
    }
}

