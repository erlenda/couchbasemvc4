using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;

namespace TestCouchbase
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new CouchbaseClient();
            var returnValue = client.Get("");
            System.Console.WriteLine("OUTPUT: - {0}", returnValue);
            System.Console.ReadLine();

        }
    }
}
