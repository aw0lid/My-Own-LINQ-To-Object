using System.Collections;

namespace Helpers
{
    internal class QuickSorter<TSource>
    {
        protected readonly List<TSource> _buffer;
        protected readonly bool _descending;
        public bool Sorted = false;

        public List<TSource> GetSorted { get { if (Sorted) return _buffer; else return null; } }


        public QuickSorter(IEnumerable<TSource> source, bool descending = false)
        {
            _buffer = new List<TSource>(source);
            _descending = descending;
        }

        protected virtual int Partition(int start, int end)
        {
            var comparer = Comparer<TSource>.Default;
            int pivotIndex = start;
            var pivot = _buffer[pivotIndex];
            int left = start + 1;
            int right = end;

            while (left <= right)
            {
                if (_descending)
                {
                    while (left <= end && comparer.Compare(_buffer[left], pivot) > 0)
                        left++;
                    while (right >= start + 1 && comparer.Compare(_buffer[right], pivot) <= 0)
                        right--;
                }
                else
                {
                    while (left <= end && comparer.Compare(_buffer[left], pivot) <= 0)
                        left++;
                    while (right >= start + 1 && comparer.Compare(_buffer[right], pivot) > 0)
                        right--;
                }

                if (left < right)
                {
                    (_buffer[left], _buffer[right]) = (_buffer[right], _buffer[left]);
                    left++;
                    right--;
                }
            }

            (_buffer[pivotIndex], _buffer[right]) = (_buffer[right], _buffer[pivotIndex]);
            return right;
        }



        protected virtual void QuickSortProcess(int start, int end)
        {
            if (start >= end)
                return;

            int pivotIndex = Partition(start, end);
            QuickSortProcess(start, pivotIndex - 1);
            QuickSortProcess(pivotIndex + 1, end);
        }


        public void QuickSort()
        {
            QuickSortProcess(0, _buffer.Count - 1);
            Sorted = true;
        }

        public void QuickSort(int start, int end)
        {
            QuickSortProcess(start, end);
            Sorted = true;
        }

    }




    internal class QuickSorter<TSource, TKey> : QuickSorter<TSource>
    {
        private List<TKey> _keys;

        public QuickSorter(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending = false)
            : base(source, descending)
        {
            _keys = new List<TKey>();

            foreach (var item in _buffer) _keys.Add(keySelector(item));
        }

        protected override int Partition(int start, int end)
        {
            var comparer = Comparer<TKey>.Default;
            int pivotIndex = start;
            var pivot = _keys[pivotIndex];
            int left = start + 1;
            int right = end;


            while (left <= right)
            {
                if (_descending)
                {
                    while (left <= end && comparer.Compare(_keys[left], pivot) > 0)
                        left++;


                    while (right >= start + 1 && comparer.Compare(_keys[right], pivot) <= 0)
                        right--;
                }
                else
                {
                    while (left <= end && comparer.Compare(_keys[left], pivot) <= 0)
                        left++;


                    while (right >= start + 1 && comparer.Compare(_keys[right], pivot) > 0)
                        right--;
                }



                if (left < right)
                {
                    (_keys[left], _keys[right]) = (_keys[right], _keys[left]);
                    (_buffer[left], _buffer[right]) = (_buffer[right], _buffer[left]);


                    left++;
                    right--;
                }
            }

            (_keys[pivotIndex], _keys[right]) = (_keys[right], _keys[pivotIndex]);
            (_buffer[pivotIndex], _buffer[right]) = (_buffer[right], _buffer[pivotIndex]);


            return right;
        }



        protected override void QuickSortProcess(int start, int end)
        {
            if (start >= end)
                return;

            int pivotIndex = Partition(start, end);
            QuickSortProcess(start, pivotIndex - 1);
            QuickSortProcess(pivotIndex + 1, end);
        }
    }






    internal class Group<TKey, TSource> : IGrouping<TKey, TSource>
    {
        public TKey Key { get; }
        private IEnumerable<TSource> Elements;

        public Group(TKey key, IEnumerable<TSource> elements)
        {
            Key = key;
            Elements = elements;
        }

        public IEnumerator<TSource> GetEnumerator() => Elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    











    public class MySorter<TSource> : IEnumerable<TSource>
    {
        protected readonly IEnumerable<TSource> _source;
        protected readonly bool _Des;

        public MySorter(IEnumerable<TSource> source, bool des = false)
        {
            _source = source;
            _Des = des;
        }

        public virtual int Compare(TSource item1, TSource item2)
        {
            return Comparer<TSource>.Default.Compare(item1, item2);
        }
            

        public virtual IEnumerator<TSource> GetEnumerator()
        {
            var Sorter = new QuickSorter<TSource>(_source, _Des);
            Sorter.QuickSort();

            foreach (var item in Sorter.GetSorted) yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

  









    public class MySorter<TSource, TKey> : MySorter<TSource>
    {
        private readonly Func<TSource, TKey> _keySelector;
        private readonly MySorter<TSource>? _parent;



        public MySorter(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool des)
            : base(source, des)
        {
            _keySelector = keySelector;
            _parent = null;
        }



        public MySorter(Func<TSource, TKey> keySelector, MySorter<TSource> parent, bool des)
            : base(null, des)
        {
            _parent = parent;
            _keySelector = keySelector;
        }


        public override int Compare(TSource item1, TSource item2)
        {
            var key1 = _keySelector(item1);
            var key2 = _keySelector(item2);
            return Comparer<TKey>.Default.Compare(key1, key2);
        }


        public override IEnumerator<TSource> GetEnumerator()
        {
            
            if (_parent == null)
            {
                var Sorter = new QuickSorter<TSource, TKey>(_source, _keySelector, _Des);
                Sorter.QuickSort();
                foreach (var item in Sorter.GetSorted) yield return item;
            }
                


            else
            {
                var buffer = new List<TSource>(_parent);
                var sorter = new QuickSorter<TSource, TKey>(buffer, _keySelector, _Des);

                int start = 0;

                while (start < buffer.Count)
                {
                    int end = start;

                   
                    while (end + 1 < buffer.Count && _parent.Compare(buffer[start], buffer[end + 1]) == 0)
                        end++;

                    
                    if (end > start)
                        sorter.QuickSort(start, end);

                    start = end + 1;
                }

                foreach (var item in sorter.GetSorted)
                    yield return item;
            }  
        }
    }
}
