using System.Collections.Generic;
using System.Linq;

namespace Chen_s_Folder.Scripts.Procedural_Generation
{
    public class PriorityQueue<TElement, TPriority>
    {
        private SortedDictionary<TPriority, Queue<TElement>> _dictionary = new SortedDictionary<TPriority, Queue<TElement>>();

        public int Count { get; private set; }

        public void Enqueue(TElement element, TPriority priority)
        {
            if (!_dictionary.TryGetValue(priority, out var queue))
            {
                queue = new Queue<TElement>();
                _dictionary[priority] = queue;
            }
            queue.Enqueue(element);
            Count++;
        }

        public TElement Dequeue()
        {
            var pair = _dictionary.First();
            var element = pair.Value.Dequeue();
            if (pair.Value.Count == 0)
            {
                _dictionary.Remove(pair.Key);
            }
            Count--;
            return element;
        }
    }
}
