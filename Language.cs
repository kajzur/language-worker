using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiLanguage
{
    class Language
    {
        private int ID1;

        public int ID
        {
            get { return ID1; }
            set { ID1 = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Language(int id, string name)
        {
            this.ID = id;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            return ((Language)obj).Name.Equals(this.Name) && ((Language)obj).ID == (this.ID);
 
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    
}
