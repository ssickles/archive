//2006 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.Collections;

public static class Collection
{
   /// <summary>
   /// Returns true if collection contains the item
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="item"></param>
   /// <returns></returns>
   public static bool Contains<T>(IEnumerable<T> collection,T item) where T : IEquatable<T>
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return Contains(iterator,item);
      }
   }
   /// <summary>
   /// Returns true if iterator contains the item
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="item"></param>
   /// <returns></returns>
   public static bool Contains<T>(IEnumerator<T> iterator,T item) where T : IEquatable<T>
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      while(iterator.MoveNext())
      {
         if(iterator.Current.Equals(item))
         {
            return true;
         }
      }
      return false;
   }
   /// <summary>
   /// Converts all the items of type T in collection to a new collection of type U according to converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="U"></typeparam>
   /// <param name="collection"></param>
   /// <param name="converter"></param>
   /// <returns></returns>
   public static IEnumerable<U> ConvertAll<T,U>(IEnumerable<T> collection,Converter<T,U> converter)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(converter == null)
      {
         throw new ArgumentNullException("converter");
      }
      foreach(T item in collection)
      {
         yield return converter(item);
      }
   }
   /// <summary>
   /// Converts all the items of type T in iterator to a new iterator of type U according to converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="U"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="converter"></param>
   /// <returns></returns>
   public static IEnumerator<U> ConvertAll<T,U>(IEnumerator<T> iterator,Converter<T,U> converter)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(converter == null)
      {
         throw new ArgumentNullException("converter");
      }
      while(iterator.MoveNext())
      {
         yield return converter(iterator.Current);
      }
   }
   /// <summary>
   /// Returns true if collection contains an item that satisfies the predicate
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static bool Exists<T>(IEnumerable<T> collection,Predicate<T> match)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return Exists(iterator,match);
      }
   }
   /// <summary>
   /// Returns true if iterator contains an item that satisfies the predicate
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static bool Exists<T>(IEnumerator<T> iterator,Predicate<T> match)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      while(iterator.MoveNext())
      {
         if(match(iterator.Current))
         {
            return true;
         }
      }
      return false;
   }
   /// <summary>
   /// Finds the first occurrence of item in collection that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static T Find<T>(IEnumerable<T> collection,Predicate<T> match)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return Find(iterator,match);
      }
   }
   /// <summary>
   /// Finds the first occurrence of item in iterator that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static T Find<T>(IEnumerator<T> iterator,Predicate<T> match)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      while(iterator.MoveNext())
      {
         if(match(iterator.Current))
         {
            return iterator.Current;
         }
      }
      return default(T);
   }

   /// <summary>
   /// Finds all the items in collection that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static IEnumerable<T> FindAll<T>(IEnumerable<T> collection,Predicate<T> match)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      foreach(T item in collection)
      {
         if(match(item))
         {
            yield return item;
         }
      }
   }
   /// <summary>
   /// Finds all the items in iterator that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static IEnumerator<T> FindAll<T>(IEnumerator<T> iterator,Predicate<T> match)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }

      while(iterator.MoveNext())
      {
         if(match(iterator.Current))
         {
            yield return iterator.Current;
         }
      }
   }
   /// <summary>
   /// Finds all the items in iterator1 that are not in collection2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerable<T> FindComplement<T>(IEnumerable<T> collection1,IEnumerable<T> collection2) where T : IEquatable<T>
   {
      foreach(T item in collection1)
      {
         if(Contains(collection2,item) == false)
         {
            yield return item;
         }
      }
   }
   /// <summary>
   /// Finds all the items in iterator1 that are not in collection2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator1"></param>
   /// <param name="iterator2"></param>
   /// <returns></returns>
   public static IEnumerator<T> FindComplement<T>(IEnumerator<T> iterator1,IEnumerator<T> iterator2) where T : IEquatable<T>
   {
      while(iterator1.MoveNext())
      {
         T item = iterator1.Current;
         if(Contains(iterator2,item) == false)
         {
            yield return item;
         }
      }
   }
   /// <summary>
   /// Finds all the items that are not in the intersection of collection1 and collection2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerable<T> FindComplementIntersection<T>(IEnumerable<T> collection1,IEnumerable<T> collection2) where T : IEquatable<T>
   {
      IEnumerable<T> complement1 = FindComplement(collection1,collection2);
      IEnumerable<T> complement2 = FindComplement(collection2,collection1);
      return FindUnion(complement1,complement2);
   }
   /// <summary>
   /// Finds all the items that are not in the intersection of iterator1 and iterator2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerator<T> FindComplementIntersection<T>(IEnumerator<T> iterator1,IEnumerator<T> iterator2) where T : IEquatable<T>
   {
      IEnumerator<T> complement1 = FindComplement(iterator1,iterator2);
      IEnumerator<T> complement2 = FindComplement(iterator2,iterator1);
      return FindUnion(iterator1,iterator2);
   }
   /// <summary>
   /// Returns a collection of all the distinct items in collection (no duplicates)
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static IEnumerable<T> FindDistinct<T>(IEnumerable<T> collection)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      IList<T> list = new List<T>();
      foreach(T item in collection)
      {
         if(list.Contains(item) == false)
         {
            list.Add(item);
         }
      }
      foreach(T item in list)
      {
         yield return item;
      }
   }
   /// <summary>
   /// Returns a collection of all the distinct items in iterator (no duplicates)
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static IEnumerator<T> FindDistinct<T>(IEnumerator<T> iterator) where T : IComparable<T>
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      IList<T> list = new List<T>();
      while(iterator.MoveNext())
      {
         if(list.Contains(iterator.Current) == false)
         {
            list.Add(iterator.Current);
         }
      }
      foreach(T item in list)
      {
         yield return item;
      }
   }
   /// <summary>
   /// Find the index of the first occurrence of item in collection
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="value"></param>
   /// <returns></returns>
   public static int FindIndex<T>(IEnumerable<T> collection,T value) where T : IEquatable<T>
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return FindIndex(iterator,value);
      }
   }
   /// <summary>
   /// Find the index of the first occurrence of item in iterator
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="value"></param>
   /// <returns></returns>
   public static int FindIndex<T>(IEnumerator<T> iterator,T value) where T : IEquatable<T>
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      int index = 0;

      while(iterator.MoveNext())
      {
         if(iterator.Current.Equals(value) == false)
         {
            index++;
         }
         else
         {
            return index;
         }
      }
      return -1;
   }
   /// <summary>
   /// Finds the intersection of collection1 and collection2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerable<T> FindIntersection<T>(IEnumerable<T> collection1,IEnumerable<T> collection2) where T : IEquatable<T>
   {
      Predicate<T> existsInCollection2 =  delegate(T t)
                                          {
                                             Predicate<T> exist = delegate(T item)
                                                                  {
                                                                     return item.Equals(t);
                                                                  };
                                             return Exists(collection2,exist);
                                          };
      return FindAll(collection1,existsInCollection2);
   }
   /// <summary>
   /// Finds the intersection of iterator1 and iterator2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerator<T> FindIntersection<T>(IEnumerator<T> iterator1,IEnumerator<T> iterator2) where T : IEquatable<T>
   {
      Predicate<T> existsIniterator2 = delegate(T t)
                                      {
                                         Predicate<T> exist = delegate(T item)
                                         {
                                            return item.Equals(t);
                                         };
                                         return Exists(iterator2,exist);
                                      };
      return FindAll(iterator1,existsIniterator2);
   }

   /// <summary>
   /// Find the index of the last occurrence of item in collection that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static T FindLast<T>(IEnumerable<T> collection,Predicate<T> match)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return FindLast(iterator,match);
      }
   }
   /// <summary>
   /// Find the index of the last occurrence of item in iterator that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="value"></param>
   /// <returns></returns>
   public static T FindLast<T>(IEnumerator<T> iterator,Predicate<T> match)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      T last = default(T);

      while(iterator.MoveNext())
      {
         if(match(iterator.Current))
         {
            last = iterator.Current;
         }
      }

      return last;
   }
   /// <summary>
   /// Find the index of the last occurrence of item in collection
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="value"></param>
   /// <returns></returns>
   public static int FindLastIndex<T>(IEnumerable<T> collection,T value) where T : IEquatable<T>
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return FindLastIndex(iterator,value);
      }
   }
   /// <summary>
   /// Find the index of the last occurrence of item in iterator
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="value"></param>
   /// <returns></returns>
   public static int FindLastIndex<T>(IEnumerator<T> iterator,T value) where T : IEquatable<T>
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      int last = -1;

      while(iterator.MoveNext())
      {
         if(iterator.Current.Equals(value))
         {
            last++;
         }
      }

      if(last >= 0)
      {
         return last;
      }
      else
      {
         return -1;
      }
   }
   /// <summary>
   /// Finds the union of collection1 and collection2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerable<T> FindUnion<T>(IEnumerable<T> collection1,IEnumerable<T> collection2) where T : IEquatable<T>
   {
      LinkedList<T> union = new LinkedList<T>(FindDistinct(collection1));

      Action<T> action =   delegate(T t)
                           {
                              if(union.Find(t) == null)
                              {
                                 union.AddLast(t);
                              }
                           };
      ForEach(collection2,action);
      foreach(T item in union)
      {
         yield return item;
      }
   }
   /// <summary>
   /// Finds the union of iterator1 and iterator2
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection1"></param>
   /// <param name="collection2"></param>
   /// <returns></returns>
   public static IEnumerator<T> FindUnion<T>(IEnumerator<T> iterator1,IEnumerator<T> iterator2)
   {
      IList<T> union = ToList(iterator1);

      Action<T> action =   delegate(T t)
                           {
                              if(union.Contains(t) == false)
                              {
                                 union.Add(t);
                              }
                           };
      ForEach(iterator2,action);
      foreach(T item in union)
      {
         yield return item;
      }
   }

   /// <summary>
   ///  Performs the action on every item in collection
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <param name="action"></param>
   public static void ForEach<T>(IEnumerable<T> collection,Action<T> action)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(action == null)
      {
         throw new ArgumentNullException("action");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         ForEach(iterator,action);
      }
   }
   /// <summary>
   /// Performs the action on every item in iterator
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="action"></param>
   public static void ForEach<T>(IEnumerator<T> iterator,Action<T> action)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(action == null)
      {
         throw new ArgumentNullException("action");
      }
      while(iterator.MoveNext())
      {
         action(iterator.Current);
      }
   }
   /// <summary>
   /// Returns the number of items in collection
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static int GetLength<T>(IEnumerable<T> collection)
   {
      int length = 0;
      Action<T> add = delegate
                      {
                         length++;
                      };
      ForEach(collection,add);
      return length;
   }
   /// <summary>
   /// Returns the number of items in iterator
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static int GetLength<T>(IEnumerator<T> iterator)
   {
      int length = 0;
      Action<T> add = delegate
                      {
                         length++;
                      };
      ForEach(iterator,add);
      return length;
   }

   /// <summary>
   /// Returns a collection with a reverse order of items
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static IEnumerable<T> Reverse<T>(IEnumerable<T> collection)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      List<T> list = new List<T>(collection);
      list.Reverse();

      foreach(T item in list)
      {
         yield return item;
      }
   }
   /// <summary>
   /// Returns a collection with a reverse order of items
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static IEnumerator<T> Reverse<T>(IEnumerator<T> iterator)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      T[] items = Collection.ToArray(iterator);
      Array.Reverse(items);

      foreach(T item in items)
      {
         yield return item;
      }
   }

   /// <summary>
   /// Sorts the collection
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static IEnumerable<T> Sort<T>(IEnumerable<T> collection)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      List<T> list = new List<T>(collection);
      list.Sort();

      foreach(T item in list)
      {
         yield return item;
      }
   }
   /// <summary>
   /// Sorts the items under the iterator
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static IEnumerator<T> Sort<T>(IEnumerator<T> iterator)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      T[] items = Collection.ToArray(iterator);
      Array.Sort(items);
      foreach(T item in items)
      {
         yield return item;
      }
   }


   /// <summary>
   /// Converts collection to an array
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static T[] ToArray<T>(IEnumerable<T> collection)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return ToArray(iterator);
      }
   }
   
   /// <summary>
   /// Converts iterator to an array
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static T[] ToArray<T>(IEnumerator<T> iterator)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      List<T> list = new List<T>();

      while(iterator.MoveNext())
      {
         list.Add(iterator.Current);
      }
      return list.ToArray();
   }
   /// <summary>
   /// Converts the items in collection to an array
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="count">Initial size for optimization</param>
   /// <returns></returns>
   public static T[] ToArray<T>(IEnumerable<T> collection,int count)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("iterator");
      }
      return ToArray(collection.GetEnumerator(),count);
   }
   /// <summary>
   /// Converts the items in iterator to an array
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="count">Initial size for optimization</param>
   /// <returns></returns>
   public static T[] ToArray<T>(IEnumerator<T> iterator,int count)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      List<T> list = new List<T>(count);

      while(iterator.MoveNext())
      {
         list.Add(iterator.Current);
      }

      return list.ToArray();
   }
   /// <summary>
   /// Converts the items in collection to an array of type U according to the converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="U"></typeparam>
   /// <param name="collection"></param>
   /// <param name="converter"></param>
   /// <returns></returns>
   static U[] ToArray<T,U>(IEnumerable<T> collection,Converter<T,U> converter)
   {
      int count = GetLength(collection);
      return ToArray(collection,converter,count);
   }
   /// <summary>
   /// Converts the items in collection to an array  of type U according to the converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="count">Initial size for optimization</param>
   /// <returns></returns>
   static U[] ToArray<T,U>(IEnumerable<T> collection,Converter<T,U> converter,int count)
   {
      List<U> list = new List<U>(count);
      foreach(T t in collection)
      {
         list.Add(converter(t));
      }
      return list.ToArray();
   }
   /// <summary>
   /// Converts the items in iterator to an array of type U according to the converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="U"></typeparam>
   /// <param name="collection"></param>
   /// <param name="converter"></param>
   /// <returns></returns>
   static U[] ToArray<T,U>(IEnumerator<T> iterator,Converter<T,U> converter)
   {
      int count = GetLength(iterator);
      return ToArray(iterator,converter,count);
   }
   /// <summary>
   /// Converts the items in iterator to an array of type U according to the converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="count">Initial size for optimization</param>
   /// <returns></returns>
   static U[] ToArray<T,U>(IEnumerator<T> iterator,Converter<T,U> converter,int count)
   {
      List<U> list = new List<U>(GetLength(iterator));
      while(iterator.MoveNext())
      {
         list.Add(converter(iterator.Current));
      }
      return list.ToArray();
   }
   /// <summary>
   /// Returns a list out of collection
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static IList<T> ToList<T>(IEnumerable<T> collection)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return ToList(iterator);
      }
   }
   /// <summary>
   /// Returns a list out of iterator
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static IList<T> ToList<T>(IEnumerator<T> iterator)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("collection");
      }
      IList<T> list = new List<T>();
      while(iterator.MoveNext())
      {
         list.Add(iterator.Current);
      }
      return list;
   }
  
   
   /// <summary>
   /// Returns all the items in collection that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static bool TrueForAll<T>(IEnumerable<T> collection,Predicate<T> match)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }
      using(IEnumerator<T> iterator = collection.GetEnumerator())
      {
         return TrueForAll(iterator,match);
      }
   }
   /// <summary>
   /// Returns all the items in iterator that satisfy the predicate match
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <param name="match"></param>
   /// <returns></returns>
   public static bool TrueForAll<T>(IEnumerator<T> iterator,Predicate<T> match)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(match == null)
      {
         throw new ArgumentNullException("match");
      }

      while(iterator.MoveNext())
      {
         if(match(iterator.Current) == false)
         {
            return false;
         }
      }
      return true;
   }

   /// <summary>
   /// Converts all the items in the object-based collection of the type T to a new array of type U according to converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static U[] UnsafeToArray<T,U>(IEnumerable collection,Converter<T,U> converter)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(converter == null)
      {
         throw new ArgumentNullException("converter");
      }
      IEnumerable<U> newCollection = UnsafeConvertAll(collection,converter);
      return ToArray(newCollection);
   }
   
   /// <summary>
   /// Converts all the items in the object-based iterator of the type T to a new array of type U according to converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static U[] UnsafeToArray<T,U>(IEnumerator iterator,Converter<T,U> converter)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      if(converter == null)
      {
         throw new ArgumentNullException("converter");
      }
      IEnumerator<U> newIterator = UnsafeConvertAll(iterator,converter);
      return ToArray(newIterator);
   }











   
   
   
   
   
   
   
   
   
   /// <summary>
   /// Converts collection to an array
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="collection"></param>
   /// <returns></returns>
   public static T[] UnsafeToArray<T>(IEnumerable collection)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      IEnumerator iterator = collection.GetEnumerator();

      using(iterator as IDisposable)
      {
         return UnsafeToArray<T>(iterator);
      }
   }
   /// <summary>
   /// Converts an iterator to an array
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="iterator"></param>
   /// <returns></returns>
   public static T[] UnsafeToArray<T>(IEnumerator iterator)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("iterator");
      }
      Converter<object,T> innerConverter = delegate(object item)
                                           {
                                              return (T)item;
                                           };
      return UnsafeToArray(iterator,innerConverter);
   }
   /// <summary>
   /// Converts all the items of type T in the object-based collection to a new collection of type U according to converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="U"></typeparam>
   /// <param name="collection"></param>
   /// <param name="converter"></param>
   /// <returns></returns>
   public static IEnumerable<U> UnsafeConvertAll<T,U>(IEnumerable collection,Converter<T,U> converter)
   {
      if(collection == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(converter == null)
      {
         throw new ArgumentNullException("converter");
      }
      foreach(object item in collection)
      {
         yield return converter((T)item);
      }
   }
   /// <summary>
   /// Converts all the items of type T in the object-based IEnumerator to a new collection of type U according to converter
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <typeparam name="U"></typeparam>
   /// <param name="collection"></param>
   /// <param name="converter"></param>
   /// <returns></returns>
   public static IEnumerator<U> UnsafeConvertAll<T,U>(IEnumerator iterator,Converter<T,U> converter)
   {
      if(iterator == null)
      {
         throw new ArgumentNullException("collection");
      }
      if(converter == null)
      {
         throw new ArgumentNullException("converter");
      }
      while(iterator.MoveNext())
      {
         yield return converter((T)iterator.Current);
      }
   }
}
