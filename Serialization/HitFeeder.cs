﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using AmazingCloudSearch.Contract;
using AmazingCloudSearch.Contract.Result;
using AmazingCloudSearch.Helper;

namespace AmazingCloudSearch.Serialization
{
    class HitFeeder<T> where T : SearchDocument, new()
    {
        private static int MAX_TRY_HIT = 20;
        private JavaScriptSerializer _serializer;
        private ConvertArray _convertArray;
        private ListProperties<T> _listProperties;

        public HitFeeder()
        {
            _convertArray = new ConvertArray();
            _serializer = new JavaScriptSerializer();
            _listProperties = new ListProperties<T>();
        }

        public void Feed(SearchResult<T> searchResult, dynamic dyHit)
        {
            searchResult.hits.hit = new List<SearchResult<T>.Hit<T>>();

            foreach(var hitDocument in dyHit)
            {
                
               //  Dictionary<string, List<string>> jsonHitField = _serializer.Deserialize<Dictionary<string, List<string>>>(hitDocument.data.ToString());
               // Dictionary<string, List<string>> jsonHitField = _serializer.Deserialize<Dictionary<string, List<string>>>( hitDocument );
                Newtonsoft.Json.Linq.JObject jO = (Newtonsoft.Json.Linq.JObject)hitDocument;
                jO.Last.ToString();

                string jz = jO.Last.ToString();
                string jzw = jO["fields"].ToString();


                Newtonsoft.Json.Linq.JObject jHighlights = (Newtonsoft.Json.Linq.JObject)jO["highlights"];

                jO["fields"]["highlight"] = jO["highlights"]["content"].ToString();

                T hit = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jO["fields"].ToString());
                hit.id = hitDocument.id;
                jO["highlights"]["content"].ToString();
                
                searchResult.hits.hit.Add(new SearchResult<T>.Hit<T> { id = hitDocument.id, data = hit, highlights = jO["highlights"]["content"].ToString() });
            }
        }

        private T Map(Dictionary<string, string> data)
        {
            var hit = new T();

            foreach (var p in _listProperties.GetProperties())
            {
                
                List<string> newValues = FindField(p.Name, data);

                if (newValues==null)
                    continue;

                if (p.PropertyType == typeof(List<string>))
                    p.SetValue(hit, newValues, null);

                else if (p.PropertyType == typeof(List<int?>))
                    p.SetValue(hit, _convertArray.StringToIntNull(newValues), null);

                else if(p.PropertyType == typeof(List<int>))
                    p.SetValue(hit, _convertArray.StringToInt(newValues), null);

                else if (p.PropertyType == typeof(List<DateTime>))
                    p.SetValue(hit, _convertArray.StringToDate(newValues), null);



                else if (p.PropertyType == typeof(string))
                    p.SetValue(hit, newValues.FirstOrDefault(), null);

                else if (p.PropertyType == typeof(int?))
                    p.SetValue(hit, _convertArray.StringToIntNull(newValues).FirstOrDefault(), null);

                else if (p.PropertyType == typeof(int))
                    p.SetValue(hit, _convertArray.StringToInt(newValues).FirstOrDefault(), null);

                else if (p.PropertyType == typeof(DateTime))
                    p.SetValue(hit, _convertArray.StringToDate(newValues).FirstOrDefault(), null);
            }

            return hit;
        }

        private List<string> FindField(string propertyName, Dictionary<string, string> data)
        {
            //return data.FirstOrDefault(d => d.Key == propertyName).Value;
            List<string> l = new List<string>();


            if (data.ContainsKey(propertyName))
            {
                l.Add(data[propertyName]);

            }

            return l;
        }
    }
}