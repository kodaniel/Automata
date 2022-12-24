using System.Collections.ObjectModel;

namespace Automata.Helpers;
public static class CollectionExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
    {
        return new ObservableCollection<T>(collection);
    }
}
