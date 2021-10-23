using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode;

public interface IPriorityQueue<T> : IEnumerable<T>
{
    /// <summary>
    /// Enqueue a node to the priority queue. Lower values are placed in front.
    /// </summary>
    void Enqueue(T node, double priority);

    /// <summary>
    /// Removes the head of the queue and returns it.
    /// </summary>
    T Dequeue();

    /// <summary>
    /// Removes every node from the queue.
    /// </summary>
    void Clear();

    /// <summary>
    /// Returns whether the given node is in the queue.
    /// </summary>
    bool Contains(T node);

    /// <summary>
    /// Call this method to change the priority of a node.  
    /// </summary>
    void SetPriority(T node, double priority);

    /// <summary>
    /// Gets the priority of the given node.
    /// </summary>
    double GetPriority(T node);

    /// <summary>
    /// Returns the head of the queue, without removing it (use Dequeue() for that).
    /// </summary>
    T Peek();

    /// <summary>
    /// Returns the number of nodes in the queue.
    /// </summary>
    int Count { get; }
}
    
public class PriorityQueue<TItem> : IPriorityQueue<TItem> where TItem : notnull
{
    private class MinComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            double dif = x - y;
            return (int)(dif < 0 ? Math.Floor(dif) : Math.Ceiling(dif));
        }
    }

    private class MaxComparer : IComparer<double>
    {
        public int Compare(double x, double y)
        {
            double dif = y - x;
            return (int)(dif < 0 ? Math.Floor(dif) : Math.Ceiling(dif));
        }
    }

    private class ItemRecord
    {
        public int Index;
        public readonly TItem Item;
        public double Priority;

        public ItemRecord(int index, TItem item, double priority)
        {
            Index = index;
            Item = item;
            Priority = priority;
        }
    }

    private readonly IComparer<double> _comparer;
    private readonly List<ItemRecord> _data;
    private readonly Dictionary<TItem, ItemRecord> _itemRecordsMap;
    private ItemRecord? _lastItem;

    public int Count => _data.Count;

    public PriorityQueue(IEqualityComparer<TItem> keyComparer, bool maxPriority = false)
    {
        _data = new List<ItemRecord>();
        _itemRecordsMap = new Dictionary<TItem, ItemRecord>(keyComparer);

        if (maxPriority)
            _comparer = new MaxComparer();
        else
            _comparer = new MinComparer();
    }

    public PriorityQueue(bool maxPriority = false)
    {
        _data = new List<ItemRecord>();
        _itemRecordsMap = new Dictionary<TItem, ItemRecord>();

        if (maxPriority)
            _comparer = new MaxComparer();
        else
            _comparer = new MinComparer();
    }

    public PriorityQueue(IComparer<double> priorityComparer)
    {
        _data = new List<ItemRecord>();
        _itemRecordsMap = new Dictionary<TItem, ItemRecord>();
        _comparer = priorityComparer;
    }

    public PriorityQueue(IEqualityComparer<TItem> keyComparer, IComparer<double> priorityComparer)
    {
        _data = new List<ItemRecord>();
        _itemRecordsMap = new Dictionary<TItem, ItemRecord>(keyComparer);
        _comparer = priorityComparer;
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Enqueue(TItem item, double priority)
    {
        ItemRecord record = new(_data.Count, item, priority);
        _data.Add(record);
        _itemRecordsMap[item] = record;

        HeapifyUp(_data.Count - 1);

        if (_lastItem == null || _comparer.Compare(priority, _lastItem.Priority) > 0)
            _lastItem = record;
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public TItem Dequeue()
    {
#if DEBUG
        if (_data.Count == 0)
            throw new InvalidOperationException("Cannot dequeue from an empty PriorityQueue");
#endif

        var lastIndex = _data.Count - 1;

        var frontItem = _data[0];
        _data[0] = _data[lastIndex];
        _data.RemoveAt(lastIndex);

        HeapifyDown(0);

        _itemRecordsMap.Remove(frontItem.Item);

        if (_data.Count == 0)
            _lastItem = default(ItemRecord);

        return frontItem.Item;
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public TItem Peek()
    {
#if DEBUG
        if (_data.Count == 0)
            throw new InvalidOperationException("The PriorityQueue is empty");
#endif

        var frontItem = _data[0];
        return frontItem.Item;
    }


#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    /// <summary>
    /// Returns the item with the highest priority value,
    /// or default(TItem) if the queue is empty
    /// </summary>
    public TItem? PeekLast()
    {
        return _lastItem == null
            ? default(TItem) 
            : _lastItem.Item;
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Clear()
    {
        _data.Clear();
        _itemRecordsMap.Clear();
        _lastItem = default(ItemRecord);
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void SetPriority(TItem item, double priority)
    {
        if (_lastItem == null) throw new InvalidOperationException();
        ItemRecord record = _itemRecordsMap[item];
        record.Priority = priority;
        if (priority > _lastItem.Priority)
            _lastItem = record;

        var index = record.Index;

        var parentIndex = (index) / 2;
        if (_comparer.Compare(priority, _data[parentIndex].Priority) < 0)
        {
            HeapifyUp(index);
            return;
        }

        var childIndex = index * 2 + 1;
        var rc = childIndex + 1;
        if ((childIndex < _data.Count && _comparer.Compare(priority, _data[childIndex].Priority) > 0)
            || (rc < _data.Count && _comparer.Compare(priority, _data[childIndex + 1].Priority) > 0))
        {
            HeapifyDown(index);
        }
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public double GetPriority(TItem item)
    {
        return _itemRecordsMap[item].Priority;
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public bool Contains(TItem item)
    {
        return _itemRecordsMap.ContainsKey(item);
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private void HeapifyUp(int i)
    {
        var childIndex = i;

        while (childIndex > 0)
        {
            var parentIndex = (childIndex) / 2;

            if (_comparer.Compare(_data[childIndex].Priority, _data[parentIndex].Priority) >= 0)
                break;

            var tmp = _data[childIndex];
            _data[childIndex] = _data[parentIndex];
            _data[childIndex].Index = childIndex;
            _data[parentIndex] = tmp;
            _data[parentIndex].Index = parentIndex;

            childIndex = parentIndex;
        }
    }

#if NET_VERSION_4_5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private void HeapifyDown(int i)
    {
        var parentIndex = i;
        var lastIndex = _data.Count - 1;

        while (true)
        {
            var childIndex = parentIndex * 2 + 1;

            if (childIndex > lastIndex)
                break;

            var rc = childIndex + 1;

            if (rc <= lastIndex && _comparer.Compare(_data[rc].Priority, _data[childIndex].Priority) < 0)
                childIndex = rc;

            if (_comparer.Compare(_data[parentIndex].Priority, _data[childIndex].Priority) <= 0)
                break;

            var tmp = _data[parentIndex];
            _data[parentIndex] = _data[childIndex];
            _data[parentIndex].Index = parentIndex;
            _data[childIndex] = tmp;
            _data[childIndex].Index = childIndex;

            parentIndex = childIndex;
        }
    }

    public IEnumerator<TItem> GetEnumerator()
    {
        return _data.Select(ir => ir.Item).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}