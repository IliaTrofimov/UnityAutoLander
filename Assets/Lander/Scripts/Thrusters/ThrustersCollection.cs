using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Utils;

namespace Thrusters
{
    public abstract class BaseThrustersCollection<T> : IDictionary<ThrusterPosition, T>
    {
        private Dictionary<ThrusterPosition, T> data = new();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public ICollection<ThrusterPosition> Keys => ((IDictionary<ThrusterPosition, T>)data).Keys;

        public ICollection<T> Values => ((IDictionary<ThrusterPosition, T>)data).Values;

        public T this[ThrusterPosition key]
        {
            get => ((IDictionary<ThrusterPosition, T>)data)[key];
            set => ((IDictionary<ThrusterPosition, T>)data)[key] = value;
        }

        public T this[AxisInfo.Axis axis, AxisInfo.Direction direction, ThrusterPlacement placement]
        {
            get => data[new ThrusterPosition(axis, direction, placement)];
            set => data[new ThrusterPosition(axis, direction, placement)] = value;
        }

        public T[] this[AxisInfo.Axis axis, AxisInfo.Direction direction]
            => data.Where(t => t.Key.Axis == axis && t.Key.Direction == direction).Select(t => t.Value).ToArray();

        public T[] this[AxisInfo.Axis axis, ThrusterPlacement placement]
            => data.Where(t => t.Key.Axis == axis && t.Key.Placement == placement).Select(t => t.Value).ToArray();

        public T[] this[AxisInfo.Direction direction, ThrusterPlacement placement]
            => data.Where(t => t.Key.Direction == direction && t.Key.Placement == placement).Select(t => t.Value).ToArray();

        public T[] this[AxisInfo.Axis axis]
            => data.Where(t => t.Key.Axis == axis).Select(t => t.Value).ToArray();

        public T[] this[AxisInfo.Direction direction]
            => data.Where(t => t.Key.Direction == direction).Select(t => t.Value).ToArray();

        public T[] this[ThrusterPlacement placement]
            => data.Where(t => t.Key.Placement == placement).Select(t => t.Value).ToArray();

        public void Add(ThrusterPosition key, T value)
        {
            ((IDictionary<ThrusterPosition, T>)data).Add(key, value);
        }

        public bool ContainsKey(ThrusterPosition key)
        {
            return ((IDictionary<ThrusterPosition, T>)data).ContainsKey(key);
        }

        public bool Remove(ThrusterPosition key)
        {
            return ((IDictionary<ThrusterPosition, T>)data).Remove(key);
        }

        public bool TryGetValue(ThrusterPosition key, out T value)
        {
            return ((IDictionary<ThrusterPosition, T>)data).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<ThrusterPosition, T> item)
        {
            ((ICollection<KeyValuePair<ThrusterPosition, T>>)data).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<ThrusterPosition, T>>)data).Clear();
        }

        public bool Contains(KeyValuePair<ThrusterPosition, T> item)
        {
            return ((ICollection<KeyValuePair<ThrusterPosition, T>>)data).Contains(item);
        }

        public void CopyTo(KeyValuePair<ThrusterPosition, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<ThrusterPosition, T>>)data).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<ThrusterPosition, T> item)
        {
            return ((ICollection<KeyValuePair<ThrusterPosition, T>>)data).Remove(item);
        }

        public IEnumerator<KeyValuePair<ThrusterPosition, T>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<ThrusterPosition, T>>)data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)data).GetEnumerator();
        }
    }

    public class ThrustersCollection : BaseThrustersCollection<Thruster>
    {
    }

    public class ThrustCollection : BaseThrustersCollection<float>
    {
    }
}