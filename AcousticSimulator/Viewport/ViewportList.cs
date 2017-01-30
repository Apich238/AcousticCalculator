using System.Collections.Generic;
using ModelLib;

namespace Viewport
{
    public class ViewportList : IList<ViewportControl>
    {
        public ViewportList(Document owner)
        {
            vps = new List<ViewportControl>();
            ownDoc = owner;
        }
        public Document ownDoc;
        private List<ViewportControl> vps;
        public int IndexOf(ViewportControl item) { return vps.IndexOf(item); }
        public void Insert(int index, ViewportControl item)
        {
            if (vps.Contains(item))
            {
                item.OwnerList = null;
                Insert(index, item);
            }
            else
            {
                vps.Insert(index, item);
                if (item.OwnerList != this) item.OwnerList = this;
            }
        }
        public void RemoveAt(int index)
        {
            this[index].OwnerList = null;
        }
        public ViewportControl this[int index] { get { return vps[index]; } set { vps[index] = value; } }
        public void Add(ViewportControl item)
        {
            vps.Add(item);
            if (item.OwnerList != this) item.OwnerList = this;
        }
        public void Clear()
        {
            for(int i=Count-1;i>=0;i--) this[i].OwnerList = null;
        }
        public bool Contains(ViewportControl item) { return vps.Contains(item); }
        public void CopyTo(ViewportControl[] array, int arrayIndex) { vps.CopyTo(array, arrayIndex); }
        public int Count { get { return vps.Count; } }
        public bool Remove(ViewportControl item)
        {
            if (!Contains(item)) return false;
            vps.Remove(item);
            item.OwnerList = null;
            return true;
        }
        public void Redraw() { foreach (var v in this) v.Redraw(); }
        bool ICollection<ViewportControl>.IsReadOnly { get { return false; } }
        public IEnumerator<ViewportControl> GetEnumerator() { return vps.GetEnumerator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return vps.GetEnumerator(); }
    }
}