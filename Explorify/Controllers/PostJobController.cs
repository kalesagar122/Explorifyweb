using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Explorify.Models;
using Explorify.Web.Models;
using Quartz;
using Quartz.Impl;

namespace Explorify.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostJobController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        //
        // GET: /PostNews/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public JsonResult GetAjaxJobswData(JQueryDataTableParamModel param)
        {
            //string userId = User.Identity.GetUserId();
            int totalRowsCount;
            int filteredRowsCount;
            var data = GetAllJobs(param.sSearch,
                                Convert.ToInt32(Request["iSortCol_0"]),
                                Request["sSortDir_0"],
                                param.iDisplayStart,
                                param.iDisplayStart + param.iDisplayLength,
                                out totalRowsCount,
                                out filteredRowsCount).AsEnumerable().ToList();

            var aaData = data.Select(d => new[] { d["PostedDate"].ToString(), d["ExpireDate"].ToString(), d["JobTitle"].ToString(), d["CompanyName"].ToString(), d["CompanyJobId"].ToString(), d["JobId"].ToString() }).ToArray();

            return Json(new
            {
                param.sEcho,
                aaData,
                iTotalRecords = Convert.ToInt32(totalRowsCount),
                iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount)
            }, JsonRequestBehavior.AllowGet);

        }

        private DataTable GetAllJobs(string searchterm, int sortindex, string sortdirection, int startrow, int endrow, out int totalrowcount, out int filterrowcount)
        {
            DataTable dt = new DataTable();
            var parameter1 = new SqlParameter("@FilterTerm", searchterm);
            var parameter2 = new SqlParameter("@SortIndex", sortindex);
            var parameter3 = new SqlParameter("@SortDirection", sortdirection);
            var parameter4 = new SqlParameter("@StartRowNum", startrow);
            var parameter5 = new SqlParameter("@EndRowNum", endrow);
            var parameter6 = new SqlParameter("@TotalRowsCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var parameter7 = new SqlParameter("@FilteredRowsCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            //using (var conn = Database.Connection) 
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (var cmd = (SqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = "SpGetAllJobs";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parameter1);
                    cmd.Parameters.Add(parameter2);
                    cmd.Parameters.Add(parameter3);
                    cmd.Parameters.Add(parameter4);
                    cmd.Parameters.Add(parameter5);
                    cmd.Parameters.Add(parameter6);
                    cmd.Parameters.Add(parameter7);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    totalrowcount = int.Parse(parameter6.Value.ToString());
                    filterrowcount = int.Parse(parameter7.Value.ToString());
                }
                conn.Close();
            }
            return dt;
        }

        [HttpGet]
        public ActionResult ViewJobDetail(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    JobDetailModel vrm = GetJobById(id).ToList<JobDetailModel>().FirstOrDefault();
                    if (vrm != null)
                    {
                        return View(vrm);
                    }
                    TempData["Error"] = "Job does not exits.";
                    return Redirect("/Error/Index");
                }
                TempData["Error"] = "Job does not exits.";
                return Redirect("/Error/Index");
            }
            catch (Exception ex)
            {
                // Exception
                TempData["Error"] = ex.Message;
                return Redirect("/Error/Index");
            }
        }

        public DataTable GetJobById(Guid jobId)
        {
            DataTable dt = new DataTable();
            var parameter1 = new SqlParameter("@Id", jobId);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();
                using (var cmd = (SqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = "SpGetJobDetailsById";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(parameter1);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                }
                conn.Close();
            }
            return dt;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteJob(Guid id)
        {
            try
            {
                // Find Strain Type exits or not
                var oldjob = _db.Jobs.FirstOrDefault(j=> j.Id == id);
                if (oldjob == null) return Json(new { success = false, message = "Jobs does not found." });

                _db.Jobs.Remove(oldjob);

                //Save details
                _db.SaveChanges();

                return Json(new { success = true, message = "Job deleted successfully." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[Authorize(Roles = "Admin")]
        //public JsonResult GetAjaxLocalNewsData(JQueryDataTableParamModel param)
        //{

        //    int totalRowsCount;
        //    int filteredRowsCount;
        //    var data = GetPostedNews(param.sSearch,
        //                        Convert.ToInt32(Request["iSortCol_0"]),
        //                        Request["sSortDir_0"],
        //                        param.iDisplayStart,
        //                        param.iDisplayStart + param.iDisplayLength,
        //                        out totalRowsCount,
        //                        out filteredRowsCount).AsEnumerable().ToList();

        //    var aaData = data.Select(d => new[] { d["RowNum"].ToString(), d["CompanyName"].ToString(), d["ContactName"].ToString(), d["CallDate"].ToString(), d["CallDesc"].ToString(), d["IsCallAns"].ToString(), d["IsDelete"].ToString(), d["ID"].ToString(), d["CompanyID"].ToString(), d["CompanyContactID"].ToString(), d["Message"].ToString() }).ToArray();

        //    return Json(new
        //    {
        //        sEcho = param.sEcho,
        //        aaData = aaData,
        //        iTotalRecords = Convert.ToInt32(totalRowsCount),
        //        iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount)
        //    }, JsonRequestBehavior.AllowGet);

        //}

        //private static DataTable GetPostedNews(string searchterm, int sortindex, string sortdirection, int startrow, int endrow, out int totalrowcount, out int filterrowcount)
        //{
        //    var dt = new DataTable();
        //    var parameter1 = new SqlParameter("@FilterTerm", searchterm);
        //    var parameter2 = new SqlParameter("@SortIndex", sortindex);
        //    var parameter3 = new SqlParameter("@SortDirection", sortdirection);
        //    var parameter4 = new SqlParameter("@StartRowNum", startrow);
        //    var parameter5 = new SqlParameter("@EndRowNum", endrow);
        //    var parameter6 = new SqlParameter("@TotalRowsCount", SqlDbType.Int)
        //    {
        //        Direction = ParameterDirection.Output
        //    };
        //    var parameter7 = new SqlParameter("@FilteredRowsCount", SqlDbType.Int)
        //    {
        //        Direction = ParameterDirection.Output
        //    };

        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = "SpGetDagbonnen";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(parameter1);
        //            cmd.Parameters.Add(parameter2);
        //            cmd.Parameters.Add(parameter3);
        //            cmd.Parameters.Add(parameter4);
        //            cmd.Parameters.Add(parameter5);
        //            cmd.Parameters.Add(parameter6);
        //            cmd.Parameters.Add(parameter7);

        //            using (var adapter = new SqlDataAdapter(cmd))
        //            {
        //                adapter.Fill(dt);
        //            }

        //            totalrowcount = int.Parse(parameter6.Value.ToString());
        //            filterrowcount = int.Parse(parameter7.Value.ToString());
        //        }
        //    }
        //    return dt;
        //}


        //
        // GET: /PostNews/Details/5
        //public async Task<ActionResult> Details(Guid id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }


        //    var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

        //    return System.Web.UI.WebControls.View(category);
        //}

        //
        // GET: /PostNews/Create
        public async Task<ActionResult> Create()
        {
            var obj = new PostJobViewModel();
            var categorylist = await _db.Categories.Where(nc => nc.IsActive).ToListAsync();
            obj.CategoryList = new SelectList(categorylist, "Id", "CategoryName");

            var skilllist = await _db.Skills.Where(nc => nc.IsActive).ToListAsync();
            obj.SkillList = new SelectList(skilllist, "SkillId", "SkillName");

            var years = new List<System.Web.UI.WebControls.ListItem> 
                    { 
                          new System.Web.UI.WebControls.ListItem { Text = "0 Year", Value = "0" }, 
                          new System.Web.UI.WebControls.ListItem { Text = "1 Year", Value = "1" },
                          new System.Web.UI.WebControls.ListItem { Text = "2 Year", Value = "2" },
                          new System.Web.UI.WebControls.ListItem { Text = "3 Year", Value = "3" },
                          new System.Web.UI.WebControls.ListItem { Text = "4 Year", Value = "4" },
                          new System.Web.UI.WebControls.ListItem { Text = "5 Year", Value = "5" },
                          new System.Web.UI.WebControls.ListItem { Text = "6 Year", Value = "6" },
                          new System.Web.UI.WebControls.ListItem { Text = "7 Year", Value = "7" },
                          new System.Web.UI.WebControls.ListItem { Text = "8 Year", Value = "8" },
                          new System.Web.UI.WebControls.ListItem { Text = "9 Year", Value = "9" }
                    };
            obj.YearList = new SelectList(years, "Value", "Text");

            var months = new List<System.Web.UI.WebControls.ListItem> 
                    { 
                          new System.Web.UI.WebControls.ListItem { Text = "0 Month", Value = "0" }, 
                          new System.Web.UI.WebControls.ListItem { Text = "1 Month", Value = "1" },
                          new System.Web.UI.WebControls.ListItem { Text = "2 Month", Value = "2" },
                          new System.Web.UI.WebControls.ListItem { Text = "3 Month", Value = "3" },
                          new System.Web.UI.WebControls.ListItem { Text = "4 Month", Value = "4" },
                          new System.Web.UI.WebControls.ListItem { Text = "5 Month", Value = "5" },
                          new System.Web.UI.WebControls.ListItem { Text = "6 Month", Value = "6" },
                          new System.Web.UI.WebControls.ListItem { Text = "7 Month", Value = "7" },
                          new System.Web.UI.WebControls.ListItem { Text = "8 Month", Value = "8" },
                          new System.Web.UI.WebControls.ListItem { Text = "9 Month", Value = "9" },
                          new System.Web.UI.WebControls.ListItem { Text = "10 Month", Value = "10" },
                          new System.Web.UI.WebControls.ListItem { Text = "11 Month", Value = "11" }
                    };
            obj.MonthList = new SelectList(months, "Value", "Text");

            obj.Radius = "0.10";
            //TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //obj.PostedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
            //obj.ExpireDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);

            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            obj.PostedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
            obj.ExpireDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);

            //Get the list of Roles
            //ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View(obj);
        }

        //
        // POST: /PostNews/Create
        [HttpPost]
        public async Task<ActionResult> Create(PostJobViewModel obj, HttpPostedFileBase file)
        {
            string categorypass = null;
            string skillpass = null;

            if (!ModelState.IsValid) return View(obj);
            var pn = new Job
            {
                JobDetails = obj.JobDetails,
                ExpireDate = obj.ExpireDate,
                Id = SqlGuidUtil.NewSequentialId(),
                IsNotification = obj.IsNotification,
                Lan = obj.Lan,
                Lat = obj.Lat,
                PostedDate = obj.PostedDate,
                Radius = obj.Radius,
                JobTitle = obj.JobTitle,
                CompanyAddress = obj.CompanyAddress,
                CompanyJobId = obj.CompanyJobId,
                CompanyName = obj.CompanyName,
                MonthExperience = obj.MonthExperience,
                Website = obj.Website,
                YearExpereince = obj.YearExpereince,
                IsApplicableforWholeCity = obj.IsApplicableforWholeCity
            };

            if (obj.SelectedCategory != null)
            {
                // Category
                foreach (string equipme in obj.SelectedCategory)
                {
                    categorypass = categorypass + "'" + equipme + "',";
                    var postcategory = new JobCategory();
                    postcategory.Id = SqlGuidUtil.NewSequentialId();
                    postcategory.CategoryId = Guid.Parse(equipme);
                    pn.JobCategories.Add(postcategory);
                }
            }

            if (obj.SelectedSkill != null)
            {
                // Category
                foreach (string skill in obj.SelectedSkill)
                {
                    skillpass = skillpass + "'" + skill + "',";
                    var postskill = new JobSkill();
                    postskill.JobSkillId = SqlGuidUtil.NewSequentialId();
                    postskill.SkillId = Guid.Parse(skill);
                    pn.JobSkills.Add(postskill);
                }
            }

            if (file != null && file.ContentLength > 0)
            {
                var ni = new JobImage {Id = SqlGuidUtil.NewSequentialId()};

                // Initialize variables
                string sSavePath;
                // Set constant values
                sSavePath = "Images/";
                if (!Directory.Exists(Server.MapPath("~/" + sSavePath)))
                {
                    // if not created then it will create it.
                    Directory.CreateDirectory(Server.MapPath("~/" + sSavePath));
                }

                // Make sure a duplicate file doesn’t exist.  If it does, keep on appending an 
                // incremental numeric until it is unique
                var sFilename = Path.GetFileName(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var fileAppend = 0;
                if (!string.IsNullOrWhiteSpace(sFilename))
                {
                    while (System.IO.File.Exists(Server.MapPath(sSavePath + sFilename)))
                    {
                        fileAppend++;
                        sFilename = Path.GetFileNameWithoutExtension(file.FileName)
                                    + fileAppend.ToString() + fileExtension;
                    }

                    // Save document
                    file.SaveAs(Server.MapPath("~/" + sSavePath + sFilename));

                    ni.ImagePath = "/" + sSavePath + sFilename;

                    pn.JobImages.Add(ni);
                }
            }

            // Then create:
            _db.Jobs.Add(pn);
            await _db.SaveChangesAsync();

            // If is Notification, then only send push notification
            if (!pn.IsNotification) return RedirectToAction("Index");
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            decimal radiust = Convert.ToDecimal(pn.Radius) * 1000;
            TimeSpan span = pn.ExpireDate - pn.PostedDate;

            //JobDataMap jdm = new JobDataMap();
            if (categorypass != null)
            {
                categorypass = categorypass.Remove(categorypass.Length - 1);
                //jdm.Add("category", categorypass);
            }
            //jdm.Add("idd", pn.Id);
            //jdm.Add("lat", pn.Lat);
            //jdm.Add("lng", pn.Lan);
            //jdm.Add("title", pn.Title);
            //jdm.Add("description", pn.Description);
            //jdm.Add("expirationdate", pn.ExpireDate.ToString(CultureInfo.InvariantCulture));
            //jdm.Add("radius", radiust.ToString("#.##"));
            //jdm.Add("millisecond", span.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            IJobDetail job = JobBuilder.Create<PushNewsScheduleJob>()
                .UsingJobData("category", categorypass)
                .UsingJobData("skills", skillpass)
                .UsingJobData("idd", pn.Id.ToString())
                .UsingJobData("lat", pn.Lat)
                .UsingJobData("lng", pn.Lan)
                .UsingJobData("title", pn.JobTitle)
                .UsingJobData("description", pn.JobDetails)
                .UsingJobData("years", pn.YearExpereince)
                .UsingJobData("months", pn.MonthExperience)
                .UsingJobData("expirationdate", pn.ExpireDate.ToString())
                .UsingJobData("radius", radiust.ToString("#.##"))
                .UsingJobData("millisecond", span.TotalMilliseconds.ToString())
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
            
            

            //List<UserRegId> urlist = await _db.UserRegIds.Where(ur => ur.UserId = ).ToListAsync();
 
            //foreach(UserRegId u in urlist)
            //{
            //    PushNewsModel pnm = new PushNewsModel();
            //    pnm.id = pn.Id.ToString();
            //    pnm.lat = pn.Lat;
            //    pnm.lng = pn.Lan;
            //    pnm.title = pn.Title;
            //    pnm.description = pn.Description;
            //    pnm.expirationdate = pn.ExpireDate.ToString();
            //    decimal radiust = pn.Radius * 1000;
            //    pnm.radius = radiust.ToString("#.##");
            //    TimeSpan span = pn.ExpireDate - pn.PostedDate;
            //    pnm.millisecond = span.TotalMilliseconds.ToString();
            //    SendNotification(u.RegId,pnm);
            //}

            return RedirectToAction("Index");
        }

        
        
	}

}