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
    [RoutePrefix("rssApi/files")]
    public class FileController : ApiController
    {
        //SqlConnection con = new SqlConnection();
        RSS_DB_EF db = new RSS_DB_EF();
        
        [HttpPost]
        [Route("upload")]
        public Boolean upload([FromBody] FileUploadModel file)
        {
            if (!isValidateApiUser(file.token))
                return false;
            try
            {
                db.Files.Add(file.file);
                IQueryable<UserPlanModel> up = db.UserPlans.Where(u => u.username == file.file.username).OrderBy(u => u.priority);
                int flag = 0;
                foreach(UserPlanModel u in up)
                {
                    if(u.storageRemaining>=file.file.size)
                    {
                        u.storageRemaining -= file.file.size;
                        db.Entry(u).State = EntityState.Modified;
                        flag = 1;
                        break;
                    }
                }
                if (flag == 0)
                    return false;
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        [HttpPost]
        [Route("{filetoken}")]
        public FileModel findFile(string filetoken,[FromBody] string token)
        {
            if (!isValidateApiUser(token))
                return null;
            /*con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RSSDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (con)
                {
                    string command = "Select * from File where token=@token";
                    SqlCommand cmd = new SqlCommand(command, con);
                    SqlParameter tok = new SqlParameter("@token", token);
                    cmd.Parameters.Add(tok);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    FileModel file = new FileModel();
                    if (rdr != null)
                    {
                        file.token = (string)rdr["token"];
                        file.fileName = (string)rdr["fileName"];
                        file.fileDuration = (DateTime)rdr["fileDuration"];
                        file.sharingDuration = (DateTime)rdr["sharingDuration"];
                        file.type = (string)rdr["type"];
                        file.url = (string)rdr["url"];
                        file.username = null;
                        rdr.Close();
                        con.Close();
                        return file; ;
                    }
                    else
                    {
                        rdr.Close();
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Errors: " + ex.Message);
                return null;
            }*/
            try
            {
                FileModel f;
                f = db.Files.Find(filetoken);
                return f;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        [HttpPost]
        [Route("all")]
        public IQueryable<FileModel> getFiles(string username,[FromBody] string token)
        {
            /*List<FileModel> files = new List<FileModel>();
            con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RSSDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (con)
                {
                    string command = "Select id from User where username=@uname";
                    SqlCommand cmd = new SqlCommand(command, con);
                    SqlParameter uname = new SqlParameter("@uname", username);
                    cmd.Parameters.Add(uname);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr == null)
                        return null;
                    command = "Select * from File where userid=@userid";
                    cmd.CommandText = command;
                    SqlParameter userid = new SqlParameter("@userid", (int)rdr["id"]);
                    cmd.Parameters.Add(userid);
                    rdr = cmd.ExecuteReader();
                    if (rdr == null)
                        return null;
                    FileModel file = new FileModel();
                    while(rdr.Read())
                    {
                        file.token = (string)rdr["token"];
                        file.fileName = (string)rdr["fileName"];
                        file.fileDuration = (DateTime)rdr["fileDuration"];
                        file.sharingDuration = (DateTime)rdr["sharingDuration"];
                        file.type = (string)rdr["type"];
                        file.url = (string)rdr["url"];
                        file.username = username;
                        files.Add(file);
                    }
                    rdr.Close();
                    return files;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Errors: " + ex.Message);
                return null;
            }*/
            if (!isValidateApiUser(token))
                return null;
            try
            {
                return db.Files.Where(f => f.username == username);
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpDelete]
        [Route("delete/{filetoken}")]
        public bool deleteFile([FromUri] string filetoken, [FromBody] string token)
        {
            if (!isValidateApiUser(token))
                return false;
            using (db)
            {
                try
                {
                    FileModel file;
                    file = db.Files.Find(filetoken);
                    if (file == null)
                        return false;
                    db.Files.Remove(file);
                    db.SaveChanges();
                    return true;
                }catch (Exception e)
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
