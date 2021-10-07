using MvvmCross.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Core.Utils
{
    public class ThreadSafeObservableCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        private readonly MvxObservableCollection<T> threadUnsafeObservableCollection;

        private IList<T> items;

        public ThreadSafeObservableCollection() : this(new MvxObservableCollection<T>())
        {
        }

        public ThreadSafeObservableCollection(MvxObservableCollection<T> threadUnsafeObservableCollection)
        {
            this.threadUnsafeObservableCollection = threadUnsafeObservableCollection;
            this.threadUnsafeObservableCollection.CollectionChanged += ThreadUnsafeObservableCollectionChanged;

            items = new List<T>(this.threadUnsafeObservableCollection);
        }

        private void ThreadUnsafeObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            items = new List<T>((IEnumerable<T>)sender);
            CollectionChanged?.Invoke(items, e);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Count => items.Count;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => items[index];
            set => threadUnsafeObservableCollection[index] = value;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        #region Reading only

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        #endregion

        #region Modificating only

        public void Add(T item)
        {
            threadUnsafeObservableCollection.Add(item);
        }

        public void Clear()
        {
            threadUnsafeObservableCollection.Clear();
        }

        public bool Remove(T item)
        {
            return threadUnsafeObservableCollection.Remove(item);
        }

        public void Insert(int index, T item)
        {
            threadUnsafeObservableCollection.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            threadUnsafeObservableCollection.RemoveAt(index);
        }

        public void ReplaceWith(IEnumerable<T> items)
        {
            threadUnsafeObservableCollection.ReplaceWith(items);
        }

        public void AddRange(IEnumerable<T> items)
        {
            threadUnsafeObservableCollection.AddRange(items);
        }

        #endregion

        public void Dispose()
        {
            threadUnsafeObservableCollection.CollectionChanged -= ThreadUnsafeObservableCollectionChanged;
        }
    }
}
