using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmazingCloudSearch.Contract.Facet
{
    public class Buckets
    {
        public string Name { get; set; }
        public List<Bucket> buckets { get; set; }
    }
}
