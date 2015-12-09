using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AmazingCloudSearch.Contract;
using AmazingCloudSearch.Query;
using AmazingCloudSearch.Query.Boolean;
using AmazingCloudSearch.Query.Facets;

namespace AmazingCloudSearch.Builder
{
    /*
     * 
     * The rules when building the URL string it that ONLY function who write
     * in the string add the & at the end of it.
     * 
     */

    public class QueryBuilder<T> where T : SearchDocument, new()
    {
		private string _searchUri;
		private static readonly Regex plusRegex = new Regex(@"\++", RegexOptions.Compiled);
		private static readonly Regex urlEncodedSpaceRegex = new Regex(@"%20+", RegexOptions.Compiled);

        public QueryBuilder(string searchUri)
        {
            _searchUri = searchUri;
        }

		public string BuildSearchQuery(SearchQuery<T> query)
		{
			if (!string.IsNullOrEmpty(query.PublicSearchQueryString))
			{
				return BuildFromPublicSearchQuery(query.PublicSearchQueryString, query);
			}

			var url = new StringBuilder(_searchUri);
			url.Append("?");

			FeedKeyword(query.Keyword, url);

			FeedBooleanCritera(query.BooleanQuery, url);

			FeedFacet(query.Facets, url);

            FeedFacetFilter(query.FacetFilters, url);

			FeedReturnFields(query.Fields, url);

			FeedMaxResults(query.Size, url);

			FeedStartResultFrom(query.Start, url);

            FeedHighlight(query.Highlight, url);

			return url.ToString();
		}

        private void FeedFacetFilter(List<string> list, StringBuilder url)
        {

            if(list.Any())
            {
                url.Append("&");
                url.Append("fq=");
                url.Append("(");
                url.Append(" and ");
                foreach (string s in list)
                {

                    url.Append( s +' ');
                }
                url.Append(")");
            }
        }

        private void FeedHighlight(string highlight, StringBuilder url)
        {
            if (!string.IsNullOrEmpty(highlight))
            {
                url.Append("&");
                url.Append("highlight.");
                url.Append(highlight);
                //url.Append("={pre_tag:'<strong>', post_tag:'</strong>'}");
                url.Append("={format:'text'}");
            }
        }

		public string BuildFromPublicSearchQuery(string publicSearchQueryString, SearchQuery<T> query)
		{
			var url = new StringBuilder(_searchUri);
			url.Append("?");

			url.Append(publicSearchQueryString);

			FeedFacet(query.Facets, url);

			FeedReturnFields(query.Fields, url);

			FeedMaxResults(query.Size, url);

			FeedStartResultFrom(query.Start, url);

			return url.ToString();
		}

		public string BuildPublicSearchQuery(SearchQuery<T> query)
		{
			var url = new StringBuilder();

			FeedKeyword(query.Keyword, url);

			FeedBooleanCritera(query.BooleanQuery, url);

			FeedFacet(query.Facets, url);

			return url.ToString();
		}

		private void FeedKeyword(string keyword, StringBuilder url)
		{
			if (!string.IsNullOrEmpty(keyword))
			{
				keyword = plusRegex.Replace(keyword, " ");
				keyword = Uri.EscapeDataString(keyword);

				url.Append("q=");
				url.Append(urlEncodedSpaceRegex.Replace(keyword, "+"));				
			}
		}

        private void FeedBooleanCritera(BooleanQuery booleanQuery, StringBuilder url)
        {
            if (booleanQuery == null) return;
            if(booleanQuery.Conditions == null || booleanQuery.Conditions.Count == 0)
                return;

			bool hasParameters = (url.Length > 0);

			StringBuilder andConditions = new StringBuilder();
			StringBuilder orConditions = new StringBuilder();
			List<string> listOrCondintions = new List<string>();

			foreach (var condition in booleanQuery.Conditions)
			{
				if (condition.IsOrCondition())
				{
					listOrCondintions.Add(condition.GetCondictionParam());
				}
				else
				{
					andConditions.Append(condition.GetCondictionParam());
					andConditions.Append("+");
				}
			}

			List<string> booleanConditions = new List<string>();

			if (andConditions.Length > 0)
			{
				andConditions.Remove(andConditions.Length - 1, 1);
				booleanConditions.Add("and+" + andConditions);
			}

			if (orConditions.Length > 0)
			{
				orConditions.Remove(orConditions.Length - 1, 1);
				listOrCondintions.Add(orConditions.ToString());
			}
			
			if (listOrCondintions.Count == 1)
			{
				booleanConditions.Add("or+" + listOrCondintions[0]);
			}
			else if (listOrCondintions.Count > 1)
			{
				orConditions.Clear();
				orConditions.Append("and");

				foreach (string listOrCondintion in listOrCondintions)
				{
					orConditions.Append("+(or+");
					orConditions.Append(listOrCondintion);
					orConditions.Append(")");
				}

				booleanConditions.Add(orConditions.ToString());
			}

			if (hasParameters)
			{
				url.Append("&");
			}

			url.Append("bq=");

			string postpendBooleanCondition = null;
			int count = 0;
			foreach (string booleanCondition in booleanConditions)
			{
				if (count > 0)
				{
					url.Append("+");
				}

				url.Append("(");
				url.Append(booleanCondition);
				postpendBooleanCondition += ")";
				
				count++;
			}

			url.Append(postpendBooleanCondition);
        }

        private void FeedStartResultFrom(int? start, StringBuilder url)
        {
			if (start != null)
			{
				url.Append("&");
				url.Append("start=");
				url.Append(start);
			}
        }

        private void FeedMaxResults(int? size, StringBuilder url)
        {
            if (size != null)
			{
				url.Append("&");
                url.Append("size=");
                url.Append(size);
            }
        }

        private void FeedFacet(List<Facet> facets, StringBuilder url)
        {
            FeedFacetList(facets, url);

           // FeedFacetConstraints(facets, url);
        }

        private void FeedFacetList(List<Facet> facets, StringBuilder url)
        {
            if (facets == null || facets.Count==0)
                return;

			bool hasParameters = (url.Length > 0);

			if (hasParameters)
			{
				url.Append("&");
			}

            

            Facet lastItem = facets.Last();
            foreach (var facet in facets)
            {
                url.Append("facet." + facet.Name + "={}"); // default behaviour for now.
                //url.Append(facet.Name);

                if (!object.ReferenceEquals(lastItem, facet))
                    url.Append("&");
            }

        }

        private void FeedFacetConstraints(List<Facet> facets, StringBuilder url)
        {
            return; // FacetConstraints no longer exist in 2013-01-01 API as such.
            // This will be rewritten later to handle sort, index, bucket, whatever...

			if (facets == null || facets.Count == 0)
				return;

            foreach (var facet in facets)
            {
                FeedFacet(facet, url);
            }
        }

        private void FeedFacet(Facet facet, StringBuilder url)
        {
            if (string.IsNullOrEmpty(facet.Name))
                return;

            if(facet.TopResult != null)
			{
				url.Append("&");
                url.Append("facet-");
                url.Append(facet.Name);
                url.Append("-top-n=");
                url.Append(facet.TopResult);
            }

            if (facet.FacetContraint != null)
            {
                var param = facet.FacetContraint.GetRequestParam();
				if (param != null)
				{
					url.Append("&");
                    url.Append("facet-");
                    url.Append(facet.Name);
                    url.Append("-constraints=");
                    url.Append(param);
                }
            }
        }

        private void FeedReturnFields(List<string> fields, StringBuilder url)
		{
			if (fields == null || fields.Count == 0)
				return;
			
			bool hasParameters = (url.Length > 0);

			if (hasParameters)
			{
				url.Append("&");
			}

            url.Append("return=");

            foreach (var field in fields)
            {
                url.Append(field);
                url.Append(",");
			}

			if (url.Length > 0)
			{
				url.Remove(url.Length - 1, 1);
			}
        }
    }
}