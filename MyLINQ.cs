using Helpers;

namespace MyLINQ
{
    public static class MyLinqMethods
    {
       
        public static bool MyAny<TSource>(this IEnumerable<TSource> source)
        {
            return (source != null && source.GetEnumerator().MoveNext());
        }

        public static bool MyAny<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source != null && Predicate != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (Predicate(Enumerator.Current))
                        return true;
                }

                return false;
            }

            throw new Exception("Invalid source");
        }

        public static int MyCount<TSource>(this IEnumerable<TSource> source)
        {
            int counter = 0;

            if (source != null)
            {
                var Enumerator = source.GetEnumerator();


                while (Enumerator.MoveNext())
                    counter++;
            }

            return counter;
        }

        public static int MyCount<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            int counter = 0;

            if (source != null)
            {
                var Enumerator = source.GetEnumerator();


                while (Enumerator.MoveNext())
                {
                    if (Predicate(Enumerator.Current))
                        counter++;
                }
            }

            return counter;
        }

        public static bool All<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source != null && Predicate != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (!Predicate(Enumerator.Current))
                        return false;
                }

                return true;
            }

            throw new Exception("Invalid source");
        }

        public static bool MyContains<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source != null && Predicate != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (Predicate(Enumerator.Current))
                        return true;
                }

                return false;
            }

            throw new Exception("Invalid source");
        }

        public static bool MyContains<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            if (source != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (Enumerator.Current.Equals(value))
                        return true;
                }

                return false;
            }

            throw new Exception("Invalid source");
        }

        public static bool MyContains<TSource>(this IEnumerable<TSource> source, TSource value, IComparer<TSource> comparer)
        {
            if (source != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                    if (comparer.Compare(Enumerator.Current, value) == 0) return true;

                return false;
            }

            throw new Exception("Invalid source");
        }

        public static IEnumerable<TSource> MyWhere<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source != null && Predicate != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (Predicate(Enumerator.Current))
                        yield return Enumerator.Current;
                }
            }
        }


        public static IEnumerable<TResult> MySelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> funcation)
        {
            if (source != null && funcation != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    yield return funcation(Enumerator.Current);
                }
            }
        }


        public static IEnumerable<TResult> MySelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> funcation)
        {
            if (source != null && funcation != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    var innerCollection = funcation(Enumerator.Current);

                    if (innerCollection != null)
                    {
                        foreach (var item in innerCollection)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }


        public static IEnumerable<TResult> MyZip<TSource1, TSource2, TResult>(this IEnumerable<TSource1> source1, IEnumerable<TSource2> source2, Func<TSource1, TSource2, TResult> funcation)
        {
            if (source1 != null && source2 != null && funcation != null && source1.MyAny() && source2.MyAny())
            {
                var Enumerator1 = source1.GetEnumerator();
                var Enumerator2 = source2.GetEnumerator();

                while (Enumerator1.MoveNext() && Enumerator2.MoveNext())
                {
                    yield return funcation(Enumerator1.Current, Enumerator2.Current);
                }
            }
        }

        public static IEnumerable<TSource> MySkip<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source != null && source.MyAny())
            {
                int N = 1;
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (N <= count)
                    {
                        N++;
                        continue;
                    }

                    yield return Enumerator.Current;
                }
            }
        }

        public static IEnumerable<TSource> MySkipWhile<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            using var enumerator = source.GetEnumerator();
            bool skipping = true;

            while (enumerator.MoveNext())
            {
                if (skipping && predicate(enumerator.Current))
                    continue; 

                skipping = false;
                yield return enumerator.Current;
            }
        }


        public static IEnumerable<TSource> MySkipLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));


            if (count <= 0)
            {
                foreach (var item in source)
                    yield return item;

                yield break;
            }

            var queue = new Queue<TSource>();


            foreach (var item in source)
            {
                queue.Enqueue(item);

                if (queue.Count > count)
                    yield return queue.Dequeue();
            }
        }

        public static IEnumerable<TSource> MyTake<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();
                int N = 1;

                while (Enumerator.MoveNext() && N <= count)
                {
                    N++;
                    yield return Enumerator.Current;
                }
            }
        }

        public static IEnumerable<TSource> MyTakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
                yield break;

            var queue = new Queue<TSource>();

            foreach (var item in source)
            {
                queue.Enqueue(item);

                if (queue.Count > count)
                    queue.Dequeue();
            }

            while (queue.Count > 0)
                yield return queue.Dequeue();
        }


        public static IEnumerable<TSource> MyTakeWhile<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source != null && source.MyAny() && Predicate != null)
            {
                var Enumerator = source.GetEnumerator();

                while (Enumerator.MoveNext())
                {
                    if (Predicate(Enumerator.Current))
                        yield return Enumerator.Current;

                    else
                        break;
                }
            }
        }

        public static IEnumerable<TSource[]> MyChunk<TSource>(this IEnumerable<TSource> source, int seed)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (seed <= 0) seed = 10;

            List<TSource> group = new List<TSource>();
            var Enumerator = source.GetEnumerator();
            var counter = 1;

            Enumerator.MoveNext();

            do
            {
                group.Add(Enumerator.Current);

                if (counter == seed)
                {
                    yield return group.MyToArray();
                    group.Clear();
                    counter = 0;
                }

                counter++;

            } while (Enumerator.MoveNext());

            if (group.Count > 0)
                yield return group.MyToArray();
        }

        public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int? Page, int? Size)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (!Page.HasValue || Page.Value <= 0) Page = 1;
            if (!Size.HasValue || Size.Value <= 0) Size = 10;

            return source.MySkip((Page.Value - 1) * Size.Value).MyTake(Size.Value);
        }

        public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> source, int? Page, int? Size, Predicate<TSource> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.MyWhere(predicate).Paginate(Page.Value, Size.Value);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> MyGroupBy<TKey, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> Predicate)
        {
            if (source != null && source.MyAny() && Predicate != null)
            {
                var Enumerator = source.GetEnumerator();
                var Dictionary = new Dictionary<TKey, List<TSource>>();

                while (Enumerator.MoveNext())
                {
                    var Key = (Predicate(Enumerator.Current));

                    if (!Dictionary.ContainsKey(Key))
                        Dictionary.Add(Key, new List<TSource>());

                    Dictionary[Key].Add(Enumerator.Current);
                }

                foreach (var Pair in Dictionary)
                    yield return new Group<TKey, TSource>(Pair.Key, Pair.Value);
            }
        }

        public static Dictionary<TKey, List<TSource>> MyToLookup<TKey, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> Predicate)
        {
            if (source != null && source.MyAny() && Predicate != null)
            {
                var Dictionary = new Dictionary<TKey, List<TSource>>();

                foreach (var value in source)
                {
                    var Key = Predicate(value);

                    if (!Dictionary.ContainsKey(Key))
                        Dictionary.Add(Key, new List<TSource>());

                    Dictionary[Key].Add(value);
                }

                return Dictionary;
            }

            return null;
        }

        public static IEnumerable<TResult> MyJoin<TS1, TS2, TInner, TResult>(this IEnumerable<TS1> source1, IEnumerable<TS2> source2,
                                                              Func<TS1, TInner> Key1, Func<TS2, TInner> Key2, Func<TS1, TS2, TResult> result)
        {
            if (source1 != null && source1.MyAny() && source2 != null && source2.MyAny() && Key1 != null && Key2 != null && result != null)
            {
                foreach (var e1 in source1)
                {
                    var In1 = Key1(e1);

                    foreach (var e2 in source2)
                    {
                        var In2 = Key2(e2);

                        if (In1.Equals(In2))
                        {
                            yield return result(e1, e2);
                            break;
                        }
                    }
                }
            }
        }

        public static IEnumerable<TResult> MyLeftJoin<TS1, TS2, TInner, TResult>(this IEnumerable<TS1> source1, IEnumerable<TS2> source2,
                                                             Func<TS1, TInner> Key1, Func<TS2, TInner> Key2, Func<TS1, TS2, TResult> result)
        {
            if (source1 != null && source1.MyAny() && source2 != null && source2.MyAny() && Key1 != null && Key2 != null && result != null)
            {
                foreach (var e1 in source1)
                {
                    var In1 = Key1(e1);
                    bool Match = false;

                    foreach (var e2 in source2)
                    {
                        var In2 = Key2(e2);

                        if (In1.Equals(In2))
                        {
                            Match = true;
                            yield return result(e1, e2);
                            break;
                        }
                    }

                    if (!Match)
                        yield return result(e1, default);
                }
            }
        }

        public static IEnumerable<TSource> MyDefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (source != null && source.MyAny())
            {
                foreach (var e in source)
                    yield return e;
            }
            else
                yield return default;
        }

        public static IEnumerable<TSource> MyDefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
        {
            if (source != null && source.MyAny())
            {
                foreach (var e in source)
                    yield return e;
            }
            else
            {
                yield return defaultValue;
            }
        }


        public static IEnumerable<int> MyRange(int start, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return start++;
            }
        }

        public static IEnumerable<TSource> MyRepeat<TSource>(TSource Elment, int count)
        {

            if (count <= 0) count = 1;

            for (int i = 1; i <= count; i++)
            {
                yield return Elment;
            }
        }

        public static TSource MyElemntAt<TSource>(this IEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new InvalidDataException();

            if (index < 0)
                throw new IndexOutOfRangeException();

            int i = 0;

            foreach (var item in source)
            {
                if (i == index)
                    return item;

                i++;
            }

            throw new IndexOutOfRangeException();
        }

        public static TSource MyFirst<TSource>(this IEnumerable<TSource> source)
        {
            if (source != null && source.MyAny())
            {
                var Enumerator = source.GetEnumerator();

                if (Enumerator.MoveNext())
                    return Enumerator.Current;


                throw new InvalidDataException();
            }

            else
                throw new InvalidDataException();
        }

        public static TSource MyFirst<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source == null || Predicate == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();

            while (Enumerator.MoveNext())
            {
                if (Predicate(Enumerator.Current))
                    return Enumerator.Current;
            }

            throw new InvalidDataException();
        }

        public static TSource MyFirstOrDefult<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();

            if (Enumerator.MoveNext())
                return Enumerator.Current;

            return default;
        }

        public static TSource MyFirstOrDefult<TSource>(this IEnumerable<TSource> source, Predicate<TSource> Predicate)
        {
            if (source == null || Predicate == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();

            while (Enumerator.MoveNext())
            {
                if (Predicate(Enumerator.Current))
                    return Enumerator.Current;
            }

            return default;
        }


        public static TSource MyLast<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();


            TSource last = default;
            bool found = false;


            foreach (var item in source)
            {
                last = item;
                found = true;
            }


            if (!found)
                throw new InvalidDataException();


            return last;
        }

        public static TSource MyLast<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null)
                throw new InvalidDataException();

            TSource last = default;
            bool found = false;


            foreach (var item in source)
            {
                if (predicate(item))
                {
                    last = item;
                    found = true;
                }
            }

            if (!found)
                throw new InvalidDataException();


            return last;
        }

        public static TSource MyLastOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                return default;


            TSource last = default;


            foreach (var item in source)
                last = item;


            return last;
        }

        public static TSource MyLastOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null)
                return default;


            TSource last = default;
            foreach (var item in source)
            {
                if (predicate(item))
                    last = item;
            }


            return last;
        }

        public static TSource MySingle<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null)
                throw new InvalidDataException();

            TSource result = default;
            bool found = false;



            foreach (var item in source)
            {
                if (predicate(item))
                {
                    if (found)
                        throw new InvalidDataException();


                    result = item;
                    found = true;
                }
            }


            if (!found)
                throw new InvalidDataException();


            return result;
        }

        public static TSource MySingleOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null)
                throw new InvalidDataException();

            TSource result = default;
            bool found = false;


            foreach (var item in source)
            {
                if (predicate(item))
                {
                    if (found)
                        throw new InvalidDataException();


                    result = item;
                    found = true;
                }
            }

            return result;
        }


        public static bool MySeqEqual<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();

            var enumerator1 = source1.GetEnumerator();
            var enumerator2 = source2.GetEnumerator();

            while (true)
            {
                bool hasNext1 = enumerator1.MoveNext();
                bool hasNext2 = enumerator2.MoveNext();

                if (!hasNext1 && !hasNext2)
                    return true;

                if (hasNext1 != hasNext2)
                    return false;

                if (!enumerator1.Current.Equals(enumerator2.Current))
                    return false;
            }
        }

        public static IEnumerable<TSource> MyDistinct<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            HashSet<TSource> set = new HashSet<TSource>();
            var Enumerator = source.GetEnumerator();

            while (Enumerator.MoveNext())
            {
                if (set.Add(Enumerator.Current))
                    yield return Enumerator.Current;
            }
        }

        public static IEnumerable<TSource> MyDistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> key)
        {
            if (source == null || key == null)
                throw new InvalidDataException();


            var set = new HashSet<TKey>();


            foreach (var item in source)
            {
                var k = key(item);

                if (set.Add(k))
                    yield return item;
            }
        }

        public static IEnumerable<TSource> MyExcept<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();


            var set = new HashSet<TSource>(source2);


            foreach (var item in source1)
            {
                if (!set.Contains(item)) yield return item;

            }
        }

        public static IEnumerable<TSource> MyExpetBy<TSource, TKey>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TKey> Key)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();

            var Enumerator1 = source1.GetEnumerator();
            var Enumerator2 = source2.GetEnumerator();
            HashSet<TKey> set = new HashSet<TKey>();

            while (Enumerator2.MoveNext())
            {
                var KeyItem = Key(Enumerator2.Current);
                set.Add(KeyItem);
            }


            while (Enumerator1.MoveNext())
            {
                var KeyItem = Key(Enumerator1.Current);
                if (!set.Contains(KeyItem))
                    yield return Enumerator1.Current;
            }
        }

        public static IEnumerable<TSource> MyIntersect<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();

            var Enumerator1 = source1.GetEnumerator();
            var Enumerator2 = source2.GetEnumerator();

            HashSet<TSource> set = new HashSet<TSource>();

            while (Enumerator2.MoveNext())
                set.Add(Enumerator2.Current);

            while (Enumerator1.MoveNext())
            {
                if (set.Contains(Enumerator1.Current))
                {
                    yield return Enumerator1.Current;
                    set.Remove(Enumerator1.Current);
                }
            }
        }

        public static IEnumerable<TSource> MyIntersectBy<TSource, TKey>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TKey> Key)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();

            var Enumerator1 = source1.GetEnumerator();
            var Enumerator2 = source2.GetEnumerator();

            HashSet<TKey> set = new HashSet<TKey>();

            while (Enumerator2.MoveNext())
            {
                var ItemKey = Key(Enumerator2.Current);
                set.Add(ItemKey);
            }


            while (Enumerator1.MoveNext())
            {
                var ItemKey = Key(Enumerator1.Current);
                if (set.Contains(ItemKey))
                {
                    yield return Enumerator1.Current;
                    set.Remove(ItemKey);
                }
            }
        }

        public static IEnumerable<TSource> MyUnion<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();

            var Enumerator1 = source1.GetEnumerator();
            var Enumerator2 = source2.GetEnumerator();

            HashSet<TSource> set = new HashSet<TSource>();

            while (Enumerator1.MoveNext())
            {
                if (set.Add(Enumerator1.Current))
                    yield return Enumerator1.Current;
            }

            while (Enumerator2.MoveNext())
            {
                if (set.Add(Enumerator2.Current))
                    yield return Enumerator2.Current;
            }
        }

        public static IEnumerable<TSource> MyUnionBy<TSource, TKey>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TKey> Key)
        {
            if (source1 == null || source2 == null)
                throw new InvalidDataException();

            var Enumerator1 = source1.GetEnumerator();
            var Enumerator2 = source2.GetEnumerator();

            HashSet<TKey> set = new HashSet<TKey>();

            while (Enumerator1.MoveNext())
            {
                var ItemKey = Key(Enumerator1.Current);

                if (set.Add(ItemKey))
                    yield return Enumerator1.Current;
            }

            while (Enumerator2.MoveNext())
            {
                var ItemKey = Key(Enumerator2.Current);

                if (set.Add(ItemKey))
                    yield return Enumerator2.Current;
            }
        }

        public static TSource MyAggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> logic)
        {
            if (source == null || logic == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();
            Enumerator.MoveNext();
            TSource seed = Enumerator.Current;

            while (Enumerator.MoveNext())
            {
                seed = logic(seed, Enumerator.Current);
            }

            return seed;
        }

        public static TSource MyAggregate<TSource>(this IEnumerable<TSource> source, TSource seed, Func<TSource, TSource, TSource> logic)
        {
            if (source == null || logic == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();

            while (Enumerator.MoveNext())
            {
                seed = logic(seed, Enumerator.Current);
            }

            return seed;
        }

        public static TResult MyAggregate<TSource, TResult>(this IEnumerable<TSource> source, TSource seed, Func<TSource, TSource, TSource> logic, Func<TSource, TResult> result)
        {
            if (source == null || logic == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();

            while (Enumerator.MoveNext())
            {
                seed = logic(seed, Enumerator.Current);
            }

            return result(seed);
        }

        public static int MyMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> Key)
        {
            if (source == null || Key == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();
            Enumerator.MoveNext();

            int MaxValue = Key(Enumerator.Current);

            while (Enumerator.MoveNext())
            {
                var CurrentValue = Key(Enumerator.Current);

                if (CurrentValue > MaxValue)
                    MaxValue = CurrentValue;
            }
            return MaxValue;
        }

        public static TSource MyMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> Key, IComparer<TKey> comparer = null)
        {
            if (source == null || Key == null)
                throw new InvalidDataException();

            if (comparer == null) comparer = Comparer<TKey>.Default;

            var Enumerator = source.GetEnumerator();
            Enumerator.MoveNext();
            TSource MaxValue = Enumerator.Current;

            while (Enumerator.MoveNext())
            {
                var CurrentValue = Key(Enumerator.Current);

                if (comparer.Compare(Key(MaxValue), CurrentValue) < 0)
                    MaxValue = Enumerator.Current;
            }
            return MaxValue;
        }

        public static int MyMin<TSource>(this IEnumerable<TSource> source, Func<TSource, int> Key)
        {
            if (source == null || Key == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();
            Enumerator.MoveNext();

            int MinValue = Key(Enumerator.Current);

            while (Enumerator.MoveNext())
            {
                var CurrentValue = Key(Enumerator.Current);

                if (CurrentValue < MinValue)
                    MinValue = CurrentValue;
            }
            return MinValue;
        }

        public static TSource MyMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> Key, IComparer<TKey> comparer = null)
        {
            if (source == null || Key == null)
                throw new InvalidDataException();

            if (comparer == null) comparer = Comparer<TKey>.Default;

            var Enumerator = source.GetEnumerator();
            Enumerator.MoveNext();
            TSource MinValue = Enumerator.Current;

            while (Enumerator.MoveNext())
            {
                var CurrentValue = Key(Enumerator.Current);

                if (comparer.Compare(Key(MinValue), CurrentValue) > 0)
                    MinValue = Enumerator.Current;
            }
            return MinValue;
        }

        public static int MySum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> Key)
        {
            if (source == null || Key == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();
            Enumerator.MoveNext();
            int Total = Key(Enumerator.Current);

            while (Enumerator.MoveNext())
            {
                Total += Key(Enumerator.Current);
            }

            return Total;
        }

        public static double MyAvg<TSource>(this IEnumerable<TSource> source, Func<TSource, int> key)
        {
            if (source == null || key == null)
                throw new InvalidDataException();

            long total = 0;
            long count = 0;

            foreach (var item in source)
            {
                total += key(item);
                count++;
            }

            if (count == 0)
                throw new InvalidDataException();

            return (double)total / count;
        }

        public static IEnumerable<TSource> MyAsEnumerabl<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            var Enumerator = source.GetEnumerator();

            while (Enumerator.MoveNext())
                yield return Enumerator.Current;
        }

        public static IEnumerable<TCastType> MyCast<TSource, TCastType>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            foreach (object obj in source) yield return (TCastType)obj;
        }

        public static IEnumerable<TCastType> MyTypeOf<TSource, TCastType>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            foreach (object obj in source)
            {
                if (obj is TCastType)
                    yield return (TCastType)obj;
            }
        }

        public static List<TSource> MyToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            var result = new List<TSource>();

            foreach (var obj in source) result.Add(obj);

            return result;
        }

        public static TSource[] MyToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            List<TSource> result = new List<TSource>();
            var i = 0;
            foreach (var item in source)
                result.Add(item);


            return result.ToArray(); ;
        }

        public static HashSet<TSource> MyToHashset<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new InvalidDataException();

            var result = new HashSet<TSource>();

            foreach (var item in source)
                result.Add(item);

            return result;
        }

        public static Dictionary<TKey, TSource> MyToDictionary<TKey, TSource>(this IEnumerable<TSource> source, Func<TSource, TKey> Key)
        {
            if (source == null)
                throw new InvalidDataException();

            var result = new Dictionary<TKey, TSource>();

            foreach (var item in source)
            {
                TKey keyValue = Key(item);

                if (!result.ContainsKey(keyValue))
                    result.Add(keyValue, item);
            }

            return result;
        }

        public static TSource Random<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null || predicate == null)
                throw new InvalidDataException();

            var list = new List<TSource>();
            var rnd = new Random();


            foreach (var item in source)
            {

                if (predicate(item))
                    list.Add(item);
            }


            if (list.Count == 0)
                throw new InvalidDataException();


            return list[rnd.Next(list.Count)];
        }

        public static MySorter<TSource> MyOrder<TSource>(this IEnumerable<TSource> source, bool descending = false)
        {
            if (source == null || !source.MyAny())
                throw new InvalidDataException();

            return new MySorter<TSource>(source, descending);
        }

        public static MySorter<TSource> MyOrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> KeySelector, bool descending = false)
        {
            if (source == null || !source.MyAny())
                throw new InvalidDataException();

            return new MySorter<TSource, TKey>(source, KeySelector, descending);
        }

        public static MySorter<TSource, TKey> MyThenBy<TSource, TKey>(this MySorter<TSource> source, Func<TSource, TKey> KeySelector, bool descending = false)
        {
            if (source == null || !source.MyAny())
                throw new InvalidDataException();

            return new MySorter<TSource, TKey>(KeySelector, source, descending);
        }


        public static void Print<TSource>(this IEnumerable<TSource> source, string title)
        {
            if (source == null)
                return;
            Console.WriteLine();
            Console.WriteLine("┌───────────────────────────────────────────────────────┐");
            Console.WriteLine($"│   {title.PadRight(52, ' ')}│");
            Console.WriteLine("└───────────────────────────────────────────────────────┘");
            Console.WriteLine();
            foreach (var item in source)
            {
                if (typeof(TSource).IsValueType)
                    Console.Write($" {item} ");
                else
                    Console.WriteLine(item?.ToString());
            }
        }


        public static IEnumerable<TSource> MyConcat<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            if (source1 == null)
                throw new ArgumentNullException(nameof(source1));

            if (source2 == null)
                throw new ArgumentNullException(nameof(source2));


            var Enumerator1 = source1.GetEnumerator();
            var Enumerator2 = source2.GetEnumerator();

            while (Enumerator1.MoveNext())
                yield return Enumerator1.Current;

            while (Enumerator2.MoveNext())
                yield return Enumerator2.Current;
        }
    }
}