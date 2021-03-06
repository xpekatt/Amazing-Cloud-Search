using AmazingCloudSearch.Query;
using AmazingCloudSearch.Contract.Facet;
namespace AmazingCloudSearch.Query.Facets
{
    public class Facet
    {
        public string Name { get; set; }
        public IFacetContraint FacetContraint { get; set; }
        public int? TopResult { get; set; }
        public Buckets Buckets { get; set; }
    }
}