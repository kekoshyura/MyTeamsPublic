using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MyTeamsCore.Common {

public class HiddenAttribute: Attribute {}

public class 
FileExtensionAttribute : Attribute {
    public FileExtensionAttribute(string value) => Value = value;
    public string Value { get; }
}

internal class 
GroupAttribute: Attribute {
    public int GroupId {get; }
    public GroupAttribute(int id) => GroupId = id;
}

public class 
PathAttribute : Attribute {
    public string Value { get;}
    public PathAttribute(string value) => Value = value;
}

public class 
CloneableAttribute : Attribute { }

public class 
EditableAttribute : Attribute { }

public static class 
Attributes {
    
    //<EnumType, <AttributeType, <EnumValue, Attribute>>>
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, ConcurrentDictionary<object, object?>>> EnumAttributesCache = new();
    
    private static object _lockObject = new ();
    
    private static ConcurrentDictionary<object, object?>
    GetEnumAttributeCache(this Type enumType, Type attributeType) {
        if (EnumAttributesCache.TryGetValue(enumType, out var attributes)) {
            if (attributes.TryGetValue(attributeType, out var result))
                return result;
            result = new ConcurrentDictionary<object, object?>();
            attributes[attributeType] = result;
            return result;
        }
        
        lock(_lockObject){
            attributes = new ConcurrentDictionary<Type, ConcurrentDictionary<object, object?>>();
            attributes[attributeType] =  new ConcurrentDictionary<object, object?>();
            
            EnumAttributesCache[enumType] = attributes;
            return attributes[attributeType];
        }
    }

    public static bool 
    IsCloneable(this object @object) => @object.HasAttribute<CloneableAttribute>();
    
    public static bool 
    IsEditable(this object @object) => @object.HasAttribute<EditableAttribute>();
    
    public static T 
    GetAttribute<T>(this object @object) {
        var attributes = @object.GetCustomAttributes<T>();
        if (attributes.Length == 0)
            throw new InvalidOperationException($"No attribute {typeof(T)} on {@object}");
        return (T)attributes[0];
    }

    public static bool 
    HasAttribute<T>(this object @object)
    where T:Attribute => @object.GetCustomAttributes<T>().Length > 0;

    public static bool 
    TryGetAttribute<T>(this object @object, out T result)
    where T: Attribute {
        var attributes = @object.GetCustomAttributes<T>();
        if (attributes.Length == 0){
            result = default;
            return false;
        }
        result = (T)attributes[0];
        return true;
    }

    private static object[]
    GetCustomAttributes<T>(this object @object) => @object is PropertyInfo propertyInfo 
        ? propertyInfo.GetCustomAttributes(typeof(T), true)
        :  @object.GetType().GetCustomAttributes(typeof(T), true);

    public static bool 
    EnumHasAttribute<T>(this object @object)
    where T:Attribute {
        var enumType = @object.GetType();
        var attributesCache = enumType.GetEnumAttributeCache(attributeType: typeof(T));
        if (attributesCache.TryGetValue(@object, out var attribute))
            return attribute != null;
        
        lock(_lockObject) {
            if (attributesCache.TryGetValue(@object, out attribute))
                return attribute != null;
            
            if (TryGetEnumAttributeSlow<T>(@object, out var result)) {
                attributesCache[@object] = result;
                return true;
            }
            attributesCache[@object] = null;
            return false;
        }
    }

    private static bool 
    EnumHasAttributeSlow<T>(this object @object)
    where T:Attribute {
        var type = @object.GetType();
        return type.GetMember(@object.ToString()).First(x => x.DeclaringType == type).GetCustomAttribute<T>() != null;
    }

    public static T 
    GetEnumAttribute<T>(this object @object) where T: Attribute => 
        TryGetEnumAttribute<T>(@object, out var result) ? result 
        : throw new InvalidOperationException($"Attribute of type {typeof(T).Name} not found on the object {@object}");

    public static bool 
    TryGetEnumAttribute<T>(this object @object, [NotNullWhen(true)] out T? result) where T: Attribute {
        var enumType =  @object.GetType();
        var attributesCache = enumType.GetEnumAttributeCache(attributeType: typeof(T));
        if (attributesCache.TryGetValue(@object, out var attribute)) {
            result = (T)attribute;
            return result != null;
        }
        
        lock(_lockObject) {
            if (attributesCache.TryGetValue(@object, out attribute)) {
                result = (T)attribute;
                return result != null;
            }
            
            if (TryGetEnumAttributeSlow<T>(@object, out result)) {
                attributesCache[@object] = result;
                return true;
            }
            attributesCache[@object] = null;
            return false;
        }
       
    }
    
    private static bool 
    TryGetEnumAttributeSlow<T>(this object @object, [NotNullWhen(true)] out T? result) {
        var type = @object.GetType();
        var attributes = type.GetMember(@object.ToString()).First(x => x.DeclaringType == type).GetCustomAttributes(typeof(T), false);
        if (attributes?.Length > 0) {
            result = (T)attributes[0];
            return true;
        }
        result = default;
        return false;
    }

    private static bool 
    HasAttributeSlow<T>(this PropertyInfo propertyInfo) =>
        propertyInfo.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(T));
 
    public static string 
    GetDescription<T>(this T value)
    where T: Enum => value.TryGetEnumAttribute<DescriptionAttribute>(out var result) ? result.Description 
     : throw new InvalidOperationException($"{value} doesn't have a DescriptionAttribute");
}

public class 
RequiredAttribute: Attribute {}

}
