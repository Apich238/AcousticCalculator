//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using MaterialsDatabase;

//namespace ModelEditor
//{
//    public class XMLMatDB : IMatDBDict
//    {
//        public XMLMatDB(string fname)
//        {
//            doc = XDocument.Load(fname);
//            root = doc.Root;
//        }

//        private XDocument doc;
//        private XElement root;
//        public Material this[int key]
//        {
//            get
//            {
//                foreach(XElement el in root)
//                {
//                    if()
//                }
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public int Count
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public bool IsReadOnly
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public ICollection<int> Keys
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public ICollection<Material> Values
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public void Add(KeyValuePair<int, Material> item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Add(int key, Material value)
//        {
//            throw new NotImplementedException();
//        }

//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(KeyValuePair<int, Material> item)
//        {
//            throw new NotImplementedException();
//        }

//        public bool ContainsKey(int key)
//        {
//            throw new NotImplementedException();
//        }

//        public void CopyTo(KeyValuePair<int, Material>[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerator<KeyValuePair<int, Material>> GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Remove(KeyValuePair<int, Material> item)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Remove(int key)
//        {
//            throw new NotImplementedException();
//        }

//        public bool TryGetValue(int key, out Material value)
//        {
//            throw new NotImplementedException();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
