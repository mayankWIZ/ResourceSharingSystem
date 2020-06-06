using DataManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataManagement.Controllers
{
    [RoutePrefix("rssApi/groups")]
    public class GroupController : ApiController
    {
        //SqlConnection con = new SqlConnection();
        RSS_DB_EF db = new RSS_DB_EF();
        [HttpPost]
        [Route("create")]
        public Boolean createGroup(GroupCreateModel group)
        {
            if (!isValidateApiUser(group.token))
                return false;
            try
            {
                db.Groups.Add(group.group);
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        [HttpPost]
        [Route("add")]
        public Boolean addMembers(GroupMemberCreateModel group)
        {
            if (!isValidateApiUser(group.token))
                return false;

            /*con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RSSDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            int[] userid = new int[users.Length];
            try
            {
                using (con)
                {
                    string command = "";
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader rdr;
                    cmd.Connection = con;
                    SqlParameter gid = new SqlParameter("@gid", id);
                    command = "Select id from Group where Id=@gid";
                    cmd.CommandText = command;
                    cmd.Parameters.Add(gid);
                    con.Open();
                    rdr = cmd.ExecuteReader();
                    if (rdr == null)
                    {
                        return false;
                    }
                    command = "Select id from User where username=@uname";
                    cmd.CommandText = command;
                    for (int i = 0; i < users.Length; i++)
                    {
                        SqlParameter uname = new SqlParameter("@uname", users[i]);
                        cmd.Parameters.Add(uname);
                        rdr = cmd.ExecuteReader();
                        if (rdr != null)
                        {
                            userid[i] = (int)rdr["id"];
                            rdr.Close();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    command = "INSERT INTO Group_Member(Id,userid,reqStatus) VALUES(@gid,@uid,0)";
                    cmd.CommandText = command;
                    cmd.Parameters.Add(gid);
                    SqlParameter uid = new SqlParameter();
                    uid.ParameterName = "@uid";
                    for (int i = 0; i < users.Length; i++)
                    {
                        uid.Value = userid[i];
                        cmd.Parameters.Add(uid);
                        if (cmd.ExecuteNonQuery() <= 0)
                        {
                            return false;
                        }
                    }
                    con.Close();
                    return true;

                }
            }
            catch (Exception ex)
            {
                Console.Write("Errors: " + ex.Message);
                return false;
            }*/
            try
            {
                if (db.Groups.Find(group.id) == null)
                    return false;
                GroupMemberModel gm = new GroupMemberModel();
                gm.groupId = group.id;
                foreach (string uname in group.users)
                {
                    gm.username = uname;
                    gm.reqStatus = false;
                    db.GroupMembers.Add(gm);
                }
                db.SaveChanges();
                return true;
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }
        [HttpPost]
        [Route("ack")]
        public Boolean ack(GroupMemberCreateModel group,[FromBody] bool st)
        {
            if (!isValidateApiUser(group.token))
                return false;
            /*con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RSSDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            int reqP = 0, userid;
            try
            {
                using (con)
                {
                    string command = "Select * from Group where Id=@gid";
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader rdr;
                    cmd.CommandText = command;
                    cmd.Connection = con;
                    SqlParameter id = new SqlParameter("@gid", gid);
                    cmd.Parameters.Add(id);
                    con.Open();
                    rdr = cmd.ExecuteReader();
                    if (rdr == null)
                    {
                        return false;
                    }
                    reqP = (int)rdr["reqPending"];
                    if (st == true)
                        reqP--;
                    rdr.Close();
                    command = "Select id from User where username=@uname";
                    SqlParameter uname = new SqlParameter("@uname", username);
                    cmd.Parameters.Add(uname);
                    rdr = cmd.ExecuteReader();
                    if (rdr == null)
                        return false;
                    userid = (int)rdr["id"];
                    rdr.Close();
                    command = "UPDATE Group_Member SET reqStatus = @reqst where Id=@gid and userid=@userid";
                    cmd.CommandText = command;
                    cmd.Parameters.Add(id);
                    SqlParameter reqst = new SqlParameter("@reqst", st);
                    cmd.Parameters.Add(reqst);
                    SqlParameter user = new SqlParameter("@userid", userid);
                    cmd.Parameters.Add(user);
                    if (cmd.ExecuteNonQuery() <= 0)
                        return false;

                    command = "UPDATE Group SET reqPending = @reqp where Id=@gid";
                    cmd.CommandText = command;
                    cmd.Parameters.Add(id);
                    SqlParameter reqp = new SqlParameter("@reqp", reqP);
                    cmd.Parameters.Add(reqp);
                    if (cmd.ExecuteNonQuery() <= 0)
                        return false;
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Errors: " + ex.Message);
                return false;
            }*/
            try
            {
                IQueryable<GroupMemberModel> gm;
                gm = db.GroupMembers.Where(g => g.groupId == group.id && g.username == group.users.First());
                foreach (GroupMemberModel g in gm)
                {
                    g.reqStatus = st;
                    db.Entry(g).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        public bool isValidateApiUser(string token)
        {

            if (db.Tokens.Find(token) == null)
                return false;
            return true;
        }

    }
}
