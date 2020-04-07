using System;
using System.Collections.Generic;
using System.Text;

namespace smallDex.models
{


    public class PokeCallAll
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
    }
    public class Result
        {
            public string name { get; set; }
            public string url { get; set; }
        }

       
    }
