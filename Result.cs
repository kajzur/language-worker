using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiLanguage
{
    class Result
    {
        public List<Group> finishedGroups;
        public int positiveAnsw;
        public int negativeAnsw;
        public TimeSpan duration;
        public Result(List<Group> finishedGroups, int positiveAnsw, int negativeAnsw, TimeSpan duration)
        {

            this.finishedGroups = finishedGroups;
            this.positiveAnsw = positiveAnsw;
            this.negativeAnsw = negativeAnsw;
            this.duration = duration;
        
        }
    }
}
