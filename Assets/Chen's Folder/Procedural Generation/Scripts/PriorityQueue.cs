using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<TElement, TPriority>
{
    private SortedDictionary<TPriority, Queue<TElement>> dictionary = new SortedDictionary<TPriority, Queue<TElement>>();

    public int Count { get; private set; }

    public void Enqueue(TElement element, TPriority priority)
    {
        if (!dictionary.TryGetValue(priority, out var queue))
        {
            queue = new Queue<TElement>();
            dictionary[priority] = queue;
        }
        queue.Enqueue(element);
        Count++;
    }

    public TElement Dequeue()
    {
        var pair = dictionary.First();
        var element = pair.Value.Dequeue();
        if (pair.Value.Count == 0)
        {
            dictionary.Remove(pair.Key);
        }
        Count--;
        return element;
    }
}
