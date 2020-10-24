using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //Generell klass för skapande & hantering av objekt-arrayer
    class ObjectArray
    {
        private static int m_arrayCount;
        
        public ObjectArray(int a_arrayCount)
        {
            m_arrayCount = a_arrayCount;
        }

        private List<Object> m_particleArray = new List<Object>(m_arrayCount);
       
        public void Add(Object a_particle)
        {
            m_particleArray.Add(a_particle);
        }

        public void Remove(Object a_particle)
        {
            m_particleArray.Remove(a_particle);
        }

        public Array Get()
        {
            return m_particleArray.ToArray();
        }
    }
}
