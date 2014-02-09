using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiLanguage
{
    public class Group
    {
        private String name;
        public List<Word> words;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public static Group getFromID(long id) {

            return new Group("", id);
        
        }

        public Group(String name, long id)
        {
            this.name = name;
            this.id = id;
            words = new List<Word>();
        }

        public override string ToString() {

            return name;

        }


        public override bool Equals(object obj)
        {
      
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((Group)obj).Id==this.Id;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
