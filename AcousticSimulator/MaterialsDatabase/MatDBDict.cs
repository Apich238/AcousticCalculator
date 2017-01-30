using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace MaterialsDatabase
{
    public class MatDBDict : IMatDBDict
    {
        public MatDBDict(string username, string pwd, string host, uint port)
        {
            cl = new MaterialsDBClient(username, pwd, host, port);
            cl.Reload();
        }
        private MaterialsDBClient cl;
        public void Reload() { cl.Reload(); }
        /// <summary>
        /// add item to database
        /// </summary>
        /// <param name="key">have not effect, key creating automatically in Mysql db in stored procedure</param>
        /// <param name="value"></param>
        public void Add(int key, Material value)
        {
            if (0 == cl.AddMaterial(value))
                throw new Exception("Element not added");
        }
        public bool ContainsKey(int key)
        {
            foreach (DataRow r in cl.Materials.Rows)
                if (r.Field<int>(cl.Materials.Columns["ID"]) == key) return true;
            return false;
        }
        public ICollection<int> Keys
        {
            get
            {
                List<int> l = new List<int>();
                foreach (DataRow r in cl.Materials.Rows)
                    l.Add(r.Field<int>(cl.Materials.Columns["ID"]));
                return l;
            }
        }
        /// <summary>
        /// sets material by id outdated
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(int key)
        {
            cl.SetMaterialOutDated(key, true);
            return true;
        }

        public bool TryGetValue(int key, out Material value)
        {
            value = null;
            try
            {
                value = cl.GetMaterialByID(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ICollection<Material> Values
        {
            get
            {
                List<Material> l = new List<Material>();
                foreach (var i in Keys)
                {
                    l.Add(cl.GetMaterialByID(i));
                }
                return l;
            }
        }

        public Material this[int key]
        {
            get
            {
                Material m = null;
                if (!TryGetValue(key, out m)) throw new KeyNotFoundException();
                return m;
            }
            set
            {
                if (!ContainsKey(key)) throw new KeyNotFoundException();
                else
                {
                    cl.UpdMaterial(key, value);
                }
            }
        }
        
        public void Add(KeyValuePair<int, Material> item)
        {
            cl.AddMaterial(item.Value);
        }

        /// <summary>
        /// removes all outdated materials
        /// </summary>
        public void Clear()
        {
            cl.RemoveOutdatedMaterials();
        }

        public bool Contains(KeyValuePair<int, Material> item)
        {
            if (!ContainsKey(item.Key)) return false;
            return (this[item.Key].Equals(item.Value));
        }

        public void CopyTo(KeyValuePair<int, Material>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return Keys.Count; }
        }

        public bool IsReadOnly
        {
            get { return cl.UserType == MaterialsDBClient.UserMode.Viewer; }
        }

        /// <summary>
        /// only key matter in this method. same as call Remove(item.Key)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<int, Material> item)
        {
            return Remove(item.Key);
        }
        //---------------------------------------------------------------------
        public IEnumerator<KeyValuePair<int, Material>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
