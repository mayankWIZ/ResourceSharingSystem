using DataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataManagement.Controllers
{
    [RoutePrefix("rssApi/plans")]
    public class PlanController : ApiController
    {
        RSS_DB_EF db = new RSS_DB_EF();

        [HttpPost]
        [Route("all")]
        public IQueryable<PlanModel> displayPlans([FromBody] string token)
        {
            if (!isValidateApiUser(token))
                return null;
            using (db)
            {
                try
                {
                    return db.Plans;
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        [HttpPost]
        [Route("{uname}")]
        public IQueryable<UserPlanModel> getPlan([FromBody] string token, [FromUri] string uname)
        {
            if (!isValidateApiUser(token))
                return null;
            using (db)
            {
                try
                {
                    return db.UserPlans.Where(u => u.username == uname);

                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        [HttpPost]
        [Route("add")]
        public bool addPlan(AddPlanModel user)
        {
            if (!isValidateApiUser(user.token))
                return false;
            using (db)
            {
                try
                {
                    PlanModel p = db.Plans.Find(user.id);
                    if (p == null)
                        return false;
                    UserPlanModel up = new UserPlanModel();
                    int max = 0;
                    foreach (UserPlanModel t in db.UserPlans.Where(u => u.username == user.username))
                    {
                        if (t != null && t.priority > max)
                            max = t.priority;
                    }
                    up.id = user.id;
                    up.username = user.username;
                    up.expiryTime = DateTime.Now.AddDays(p.validity);
                    up.storageRemaining = p.storageBenefit;
                    up.subTime = DateTime.Now;
                    up.priority = max+1;
                    db.UserPlans.Add(up);
                    db.SaveChanges();
                    return true;

                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        [NonAction]
        public bool isValidateApiUser(string token)
        {
            if (db.Tokens.Find(token) == null)
                return false;
            return true;
        }
    }
}
