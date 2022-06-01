using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Models
{
    public class ListNewsModel
    {
        public string Detail { get; set; }
        
    }

    public class NewsSlideModel
    {
        public int NEWS_ID { get; set; }
        public string NEWS_TYPE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string ATTACHMENT { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public int TEST_TYPE_ID { get; set; }
    }

    public class LocationModel
    {
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string UrlLocation { get; set; }
        public string Detail { get; set; }
    }

    public class NewsViewModel
    {
        public List<NewsModel> News { get; set; }
        public List<SubNewsModel> SubNews { get; set; }
    }
    public class NewsModel
    {
        //public string NewsID { get; set; }
        //public string CollapseID { get; set; }
        //public string Header { get; set; }
        //public string Header2 { get; set; }
        //public string Image { get; set; }
        //public List<SubNewsModel> SubNews { get; set; }
        public int NEWS_ID { get; set; }
        public string HEADLINE { get; set; }
        public string DETAILS { get; set; }
        public int? PARENT_NEWS_ID { get; set; }
        public string NEWS_TYPE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string ATTACHMENT { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public int TEST_TYPE_ID { get; set; }
        public string NEW_FLAG { get; set; }
        public bool IS_PINNED { get; set; }
        public int? PRIORITY_LEVEL { get; set; }

        public string CONTAINER_CSS { get; set; }
    }

    public class SubNewsModel
    {
        //public string NewsID { get; set; }
        //public string AnnFilePath { get; set; }
        //public string TextClick { get; set; }
        //public string Detail { get; set; }
        public string DETAILS { get; set; }
        public int NEWS_ID { get; set; }
        public string HEADLINE { get; set; }
        public int PARENT_NEWS_ID { get; set; }
        public string NEWS_TYPE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string ATTACHMENT { get; set; }
        public string ACTIVE_FLAG { get; set; }
        public int TEST_TYPE_ID { get; set; }
        public string NEW_FLAG { get; set; }

    }
}
