using System;
using System.Collections.Generic;
using System.Reflection;

namespace WhaleTee.Runtime.InternalUtils {
  public class ReflectionUtils {
    internal struct AttributedField<T> where T : Attribute {
      public T attribute;
      public FieldInfo fieldInfo;
    }

    internal static void GetFieldsWithAttributeFromType<T>(
      Type classToInspect,
      IList<AttributedField<T>> output,
      BindingFlags reflectionFlags = BindingFlags.Default
    ) where T : Attribute {
      var type = typeof(T);

      do {
        var allFields = classToInspect.GetFields(reflectionFlags);

        foreach (var fieldInfo in allFields) {
          var attributes = Attribute.GetCustomAttributes(fieldInfo);

          foreach (var attribute in attributes) {
            if (!type.IsInstanceOfType(attribute))
              continue;

            output.Add(new AttributedField<T> { attribute = attribute as T, fieldInfo = fieldInfo });
            break;
          }
        }

        classToInspect = classToInspect.BaseType;
      }
      while (classToInspect != null);
    }
  }
}