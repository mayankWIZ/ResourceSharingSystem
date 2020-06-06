using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataManagement.Models;

namespace DataManagement.Controllers
{
    public class TokenController : ApiController
    {
        private RSS_DB_EF db = new RSS_DB_EF();

        [HttpPost]
        public IQueryable<TokenUsersModel> GetTokens([FromBody]string token)
        {
            if (!isValidateApiUser(token))
                return null;
            try
            {
                return db.Tokens;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpPost]
        [ResponseType(typeof(TokenUsersModel))]
        public IHttpActionResult GetTokenUsersModel([FromUri]string id,[FromBody] string token)
        {
            TokenUsersModel tokenUsersModel = db.Tokens.Find(id);
            if (tokenUsersModel == null)
            {
                return NotFound();
            }

            return Ok(tokenUsersModel);
        }

        // PUT: api/Token/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTokenUsersModel(string id, TokenUsersModel tokenUsersModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tokenUsersModel.token)
            {
                return BadRequest();
            }

            db.Entry(tokenUsersModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenUsersModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Token
        [HttpPost]
        [ResponseType(typeof(TokenUsersModel))]
        public IHttpActionResult PostTokenUsersModel(TokenUsersModel tokenUsersModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tokens.Add(tokenUsersModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TokenUsersModelExists(tokenUsersModel.token))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tokenUsersModel.token }, tokenUsersModel);
        }

        // DELETE: api/Token/5
        [HttpDelete]
        [ResponseType(typeof(TokenUsersModel))]
        public IHttpActionResult DeleteTokenUsersModel(string id)
        {
            TokenUsersModel tokenUsersModel = db.Tokens.Find(id);
            if (tokenUsersModel == null)
            {
                return NotFound();
            }

            db.Tokens.Remove(tokenUsersModel);
            db.SaveChanges();

            return Ok(tokenUsersModel);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool TokenUsersModelExists(string id)
        {
            return db.Tokens.Count(e => e.token == id) > 0;
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