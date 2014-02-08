using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiLanguage
{
    public class Word
    {
        private long id_group;

        public long Id_group
        {
            get { return id_group; }
            set { id_group = value; }
        }
        private string foregin;

        public string Foregin
        {
            get {


                return foregin;
            }
            set { foregin = value; }
        }
        private string basic;

        public string Basic
        {
            get {

                return basic;
            }
            set { basic = value; }
        }
        public Word(long id, string foregin, string basic)
        {
            this.id_group = id;
            this.foregin = foregin;
            this.basic = basic;
        }

        public override string ToString(){


            return basic+" - "+foregin;
        

        }
    }
}
