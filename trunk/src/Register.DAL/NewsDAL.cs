using CSP.Lib.Diagnostic;
using Register.Database;
using Register.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.DAL
{
    public class NewsDAL : BaseDAL
    {
        //public IEnumerable<dynamic> NewsSelectType()
        //{
        //    List<ListNewsModel> news = new List<ListNewsModel>
        //    {
        //        new ListNewsModel { Detail = "1. ประกาศกรมส่งเสริมการปกครองท้องถิ่น เรื่อง รับสมัครสอบแข่งขันเพื่อบรรจุและแต่งตั้งบุคคลเข้ารับราชการเป็นข้าราชการพลเรือนสามัญ ในตำแหน่งนักส่งเสริมการปกครองท้องถิ่นปฏิบัติการ ตำแหน่งนิติกรปฏิบัติการ ตำแหน่งนักวิชาการเงินและบัญชีปฏิบัติการ ตำแหน่งเจ้าพนักงานส่งเสริมการปกครองท้องถิ่นปฏิบัติงาน และตำแหน่งเจ้าพนักงานการเงินและบัญชีปฏิบัติงาน" },
        //        new ListNewsModel { Detail = "2. ประกาศกรมส่งเสริมการปกครองท้องถิ่น เรื่อง รายชื่อผู้สมัครสอบแข่งขันเพื่อบรรจุและแต่งตั้งบุคคลเข้ารับราชการเป็นข้าราชการพลเรือนสามัญในตำแหน่งนักส่งเสริมการปกครองท้องถิ่นปฏิบัติการ ตำแหน่งนิติกรปฏิบัติการ ตำแหน่งนักวิชาการเงินและบัญชีปฏิบัติการ ตำแหน่งเจ้าพนักงานส่งเสริมการปกครองท้องถิ่นปฏิบัติงาน และตำแหน่งเจ้าพนักงานการเงินและบัญชีปฏิบัติงาน"},
        //        new ListNewsModel { Detail = "3. ประกาศกรมส่งเสริมการปกครองท้องถิ่น เรื่อง แก้ไขประกาศรับสมัครสอบแข่งขันเพื่อบรรจุและแต่งตั้งบุคคลเข้ารับราชการเป็นข้าราชการพลเรือนสามัญในตำแหน่งนักส่งเสริมการปกครองท้องถิ่นปฏิบัติการ ตำแหน่งนิติกรปฏิบัติการ ตำแหน่งนักวิชาการเงินและบัญชีปฏิบัติการ ตำแหน่งเจ้าพนักงานส่งเสริมการปกครองท้องถิ่นปฏิบัติงาน และตำแหน่งเจ้าพนักงานการเงินและบัญชีปฏิบัติงาน" }
        //    };
        //    return news.ToList();
        //}

        public async Task<List<NewsSlideModel>> NewsSlides(int testTypeID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    return await db.ExecuteStored<NewsSlideModel>("ENR_SP_GET_NEWS_SLIDE @TEST_TYPE_ID, @IPADDRESS, @PROCESSCD", prms).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<List<NewsModel>> News(int testTypeID, string ipAddress, string processCD)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("IPADDRESS", ipAddress));
                    prms.Add(new CommonParameter("PROCESSCD", processCD));
                    return await db.ExecuteStored<NewsModel>("ENR_SP_GET_NEWS @TEST_TYPE_ID, @IPADDRESS, @PROCESSCD", prms).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        public async Task<List<SubNewsModel>> NewsDetail(int testTypeID, int newsID)
        {
            try
            {
                using (var db = new RegisterDB())
                {
                    var prms = new List<CommonParameter>();
                    prms.Add(new CommonParameter("TEST_TYPE_ID", testTypeID));
                    prms.Add(new CommonParameter("NEWS_ID", (newsID == -1 ? DBNull.Value : (object)newsID) ));
                    prms.Add(new CommonParameter("IPADDRESS", ""));
                    prms.Add(new CommonParameter("PROCESSCD", ""));
                    return await db.ExecuteStored<SubNewsModel>("ENR_SP_GET_NEWS_DETAIL @TEST_TYPE_ID, @NEWS_ID, @IPADDRESS, @PROCESSCD", prms).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
            }
            return null;
        }

        //public IEnumerable<dynamic> LocationSchool()
        //{
        //    List<LocationModel> news = new List<LocationModel>
        //    {
        //        new LocationModel { UrlLocation = "https://goo.gl/maps/3qgAb1mhaYHpQHCV7", Detail = "แผนที่ มหาวิทยาลัยอุบลราชธานี จ.อุบลราชธานี" },
        //        new LocationModel { UrlLocation = "https://goo.gl/maps/FTkh4XCiXnzF5wFF7", Detail = "แผนที่ มหาวิทยาลัยขอนแก่น จ.ขอนแก่น"},
        //        new LocationModel { UrlLocation = "https://goo.gl/maps/27p5yDrEcdeL4unS6", Detail = "แผนที่ มหาวิทยาลัยราชภัฏสวนสุนันทา จ.กรุงเทพฯ" }
        //    };
        //    using (var db = new OCSDB())
        //    {
        //        //    var prms = new Dictionary<string, object>();
        //        //    prms.Add("@effDate", effectiveDate);
        //        //    return db.DynamicListFromSql("exec DashProjectSummary @effDate", prms).ToList();
        //        return news.ToList();
        //    }
        //}
    }
}
