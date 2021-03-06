﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CryptoGeeks.API;

namespace CryptoGeeks.API.Controllers
{
    public class UsersController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5
        [ResponseType(typeof(bool))]
        [Route("api/Users/GetUserByName")]
        public HttpResponseMessage GetUserByName(string displayName)
        {
            bool userExist = db.Users.Where(x => x.DisplayName.ToLower() == displayName.ToLower()).Count() > 0;

            return Request.CreateResponse(HttpStatusCode.OK, userExist, Configuration.Formatters.JsonFormatter);
        }

        [ResponseType(typeof(User))]
        [Route("api/Users/GetUserDetailsByName")]
        public IHttpActionResult GetUserDetailsByName(string displayName)
        {
            User user = db.Users.Where(x => x.DisplayName.ToLower() == displayName.ToLower()).FirstOrDefault();

            return Ok(user);
        }


        // GET: api/Users/5
        [Route("api/Users/GetNewUserName")]
        public void GetNewUserName(int userId, string displayName)
        {
            User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            user.DisplayName = displayName;

            db.SaveChanges();

        }

        [Route("api/Users/GetDashboard")]
        [ResponseType(typeof(GetDashboard_Result))]
        public GetDashboard_Result GetDashboard(int userId)
        {
            List<GetDashboard_Result> dashboard = db.GetDashboard(userId).ToList();

            return dashboard[0];

        }


        [Route("api/Users/GetNewUser")]
        [ResponseType(typeof(int))]
        public int GetNewUser(string displayName)
        {
            User u = new User();
            u.DisplayName = displayName;

            db.Users.Add(u);
            db.SaveChanges();


            return u.Id;

        }


        // PUT: api/Users/5
        [ResponseType(typeof(int))]
        [Route("api/Users/AddDisplayName")]
        public async Task<HttpResponseMessage> GetNewDisplayName(string displayName)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, -1, Configuration.Formatters.JsonFormatter);
            }


            User user = new User() { DisplayName = displayName };

            db.Entry(user).State = EntityState.Added;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK, user.Id, Configuration.Formatters.JsonFormatter);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<User> PostUser(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();


            return user;
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}