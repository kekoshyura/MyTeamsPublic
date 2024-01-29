using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore.Common;

public interface 
IHasId{
    public int Id { get; }
}


public class 
Counter{
    public int Value {get; private set;} = 0;

    public int
    Increment() {
        Value ++; 
        return Value;
    }
    
}

public static class 
Collections{

    public static List<TOut>
    MapToList<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn,TOut> convert) =>
        source.Select(convert).ToList();

    public static bool
    TryGet<T>(this IList<T> list, Func<T, bool> predicate, [NotNullWhen(true)] out T? result) {
        result = default;
        foreach (var item in list) {
            if (predicate(item))
                result = item;
        }
        return result != null;
    }

    public static T
    GetOrThrow<T>(this IList<T> list, Func<T, bool> predicate){
        if (!list.TryGet(predicate, out var result))
            throw new Exception("Item not found");
        
        return result;
    }

    /// <summary>
    /// First index is 1
    /// </summary>
    public static IEnumerable<(int index, T item)>
    WithIndex<T>(this IEnumerable<T> items) {
        var counter = new Counter();
        return items.Select(item => (counter.Increment(), item));
    }

    public static bool
    TryGetFirst<T>(this IList<T> list, [NotNullWhen(true)] out T? result) {
        result = default;
        if (list.Count == 0)
            return false;
        result = list[0];
        return result != null;
    }

    public static IList<T>
    ReplaceById<T>(this IList<T> list, T newItem) where T : IHasId{
        var old = list.TryGet(x => x.Id == newItem.Id, out var oldItem) ? oldItem : throw new InvalidOperationException();
        list.Remove(old);
        list.Add(newItem);
        return list;
    }

    public static IList<T>
    ReplaceByIdOrAdd<T>(this IList<T> list, T newItem) where T : IHasId{
        if (list.TryGet(x => x.Id == newItem.Id, out var oldItem))
            list.Remove(oldItem);
        list.Add(newItem);
        return list;
    }
}