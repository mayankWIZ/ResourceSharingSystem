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
    [RoutePrefix("rssApi/users")]
    public class UserController : ApiController
    {
        //SqlConnection con = new SqlConnection();
        RSS_DB_EF db = new RSS_DB_EF();
        
        [HttpPost]
        [Route("validate")]
        public Boolean validate([FromBody]ValidationModel v)
        {
            if (!isValidateApiUser(v.token))
                return false;
            /*con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RSSDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (con)
                {
                    string command = "Select * from User where username=@uname";
                    SqlCommand cmd = new SqlCommand(command, con);
                    SqlParameter uname = new SqlParameter("@uname", username);
                    cmd.Parameters.Add(uname);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while(rdr.Read())
                    {
                        if((string)rdr["password"]==pass)
                        {
                            rdr.Close();
                            con.Close();
                            return true;
                        }

                    }
                    rdr.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Errors: " + ex.Message);
                return false;
            }*/
            using (db)
            {
                UserModel u;
                u = db.Users.Find(v.username);
                if(u==null)
                {
                    return false;
                }
                if(u.isActive==true && u.isVerified==true && u.password==v.pass)
                {
                    return true;
                }
                return false;
            }
        }

        [HttpPost]
        [Route("register")]
        public Boolean register([FromBody] UserRegistrationModel user)
        {
            if (!isValidateApiUser(user.token))
                return false;
            try
            {
                if (db.Users.Find(user.user.username) != null)
                    return false;
                user.user.isVerified = false;
                user.user.isActive = false;
                db.Users.Add(user.user);
                VerifyAccountModel va = new VerifyAccountModel();
                va.username = user.user.username;
                int flag =0;
                while (flag==0)
                {
                    va.token = Guid.NewGuid().ToString();
                    foreach (VerifyAccountModel v in db.VerifyAccounts)
                    {
                        if (va.token == v.token)
                        {
                            flag = 0;
                            continue;
                        }
                    }
                    flag = 1;
                }
                va.gen_time = DateTime.Now;
                db.VerifyAccounts.Add(va);
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        [HttpPut]
        [Route("update/contact")]
        public Boolean updateContact([FromBody] ContactUpdateModel user)
        {
            if (!isValidateApiUser(user.token))
                return false;
            try
            {
                UserModel u;
                if ((u=db.Users.Find(user.username)) == null)
                    return false;
                u.email = user.email;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        [HttpPut]
        [Route("update/activation")]
        public Boolean updateActive([FromBody] ActivationUpdateModel user)
        {
            if (!isValidateApiUser(user.token))
                return false;
            try
            {
                UserModel u;
                if ((u = db.Users.Find(user.username)) == null)
                    return false;
                u.isActive = user.isActive;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        [HttpPut]
        [Route("update/password")]
        public Boolean updatePass([FromBody] PasswordUpdateModel user)
        {
            if (!isValidateApiUser(user.token))
                return false;
            try
            {
                UserModel u;
                if ((u = db.Users.Find(user.username)) == null)
                    return false;
                if (u.password == user.cpass)
                {
                    u.password = user.npass;
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        [HttpPut]
        [Route("update/Dp")]
        public Boolean updateDp([FromBody] DpUpdateModel user)
        {
            if (!isValidateApiUser(user.token))
                return false;
            try
            {
                UserModel u;
                if ((u = db.Users.Find(user.username)) == null)
                    return false;
                u.profilePic = user.url;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        [HttpPost]
        [Route("all")]
        public IQueryable<UserModel> getUsers([FromBody] string token)
        {
            if (!isValidateApiUser(token))
                return null;
            return db.Users;
        }

        [HttpPost]
        [Route("getUser/{uname}")]
        public UserModel getUsers([FromBody] string token,string uname)
        {
            if (!isValidateApiUser(token))
                return null;
            UserModel user;
            user = db.Users.Find(uname);
            return user;
        }

        
        [HttpGet]
        [Route("verify/{token}")]
        public bool verify(string token)
        {
            using (db)
            {
                try
                {
                    VerifyAccountModel v;
                    UserModel user;
                    v = db.VerifyAccounts.Find(token);
                    if (v == null)
                        return false;
                    // Active User
                    user = db.Users.Find(v.username);
                    user.isVerified = true;
                    user.isActive = true;
                    db.Entry(user).State = EntityState.Modified;

                    //Deleting Token
                    db.VerifyAccounts.Remove(v);
                    // Saving changes to DB
                    db.SaveChanges();
                    return true;

                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        [HttpDelete]
        [Route("delete/{uname}")]
        public bool DeleteUser([FromBody] string token, [FromUri] string uname)
        {
            if (!isValidateApiUser(token))
                return false;
            using (db)
            {
                try
                {
                    UserModel u;
                    u = db.Users.Find(uname);
                    if (u == null)
                        return false;
                    db.Users.Remove(u);
                    db.SaveChanges();
                    return true;
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        [HttpPost]
        [Route("forgotPass/gen/{uname}")]
        public string genToken([FromBody] string token, [FromUri] string uname)
        {
            if (!isValidateApiUser(token))
                return null;
            using (db)
            {
                try
                {
                    UserModel user = db.Users.Find(uname);
                    if (user == null)
                        return null;
                    ResetPasswordModel r = new ResetPasswordModel();
                    r.username = uname;
                    int flag = 1;
                    while(flag==1)
                    {
                        flag = 0;
                        r.token = Guid.NewGuid().ToString();
                        foreach(ResetPasswordModel c in db.ResetPasswords)
                        {
                            if (c.token == r.token)
                            {
                                flag = 1;
                                break;
                            }
                        }
                    }
                    r.gen_time = DateTime.Now;
                    db.ResetPasswords.Add(r);
                    db.SaveChanges();
                    return r.token;
                }catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        [HttpPost]
        [Route("reset/password/{token}")]
        public bool resetPass([FromBody] PasswordUpdateModel user, [FromUri] string token )
        {
            if (!isValidateApiUser(token))
                return false ;
            using (db)
            {
                try
                {
                    ResetPasswordModel r = db.ResetPasswords.Find(token);
                    if (r == null)
                        return false;
                    if(user.username == r.username)
                    {
                        UserModel u = db.Users.Find(user.username);
                        if (u == null)
                            return false;
                        u.password = user.npass;
                        db.Entry(u).State = EntityState.Modified;
                        db.ResetPasswords.Remove(r);
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
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
