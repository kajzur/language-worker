using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiLanguage
{
    public class Group
    {
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public Group(String name, int id)
        {
            this.name = name;
            this.id = id;
        }

        public override string ToString() {

            return name;

        }
    }
}
