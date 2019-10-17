using System;
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
    public class UserStatusCompactController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/UserStatusCompact
        public IQueryable<UserStatusCompact> GetUserStatusCompacts()
        {
            return db.UserStatusCompacts;
        }

        public IQueryable<UserStatusCompact> GetUserStatus(int userId)
        {
            return db.UserStatusCompacts.Where(x=>x.Id == userId);
        }

        // GET: api/UserStatusCompact/5
        [ResponseType(typeof(UserStatusCompact))]
        public async Task<IHttpActionResult> GetUserStatusCompact(int id)
        {
            UserStatusCompact userStatusCompact = await db.UserStatusCompacts.FindAsync(id);
            if (userStatusCompact == null)
            {
                return NotFound();
            }

            return Ok(userStatusCompact);
        }

        // PUT: api/UserStatusCompact/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserStatusCompact(int id, UserStatusCompact userStatusCompact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userStatusCompact.Id)
            {
                return BadRequest();
            }

            db.Entry(userStatusCompact).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserStatusCompactExists(id))
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

        // POST: api/UserStatusCompact
        [ResponseType(typeof(UserStatusCompact))]
        public async Task<IHttpActionResult> PostUserStatusCompact(UserStatusCompact userStatusCompact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserStatusCompacts.Add(userStatusCompact);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserStatusCompactExists(userStatusCompact.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userStatusCompact.Id }, userStatusCompact);
        }

        // DELETE: api/UserStatusCompact/5
        [ResponseType(typeof(UserStatusCompact))]
        public async Task<IHttpActionResult> DeleteUserStatusCompact(int id)
        {
            UserStatusCompact userStatusCompact = await db.UserStatusCompacts.FindAsync(id);
            if (userStatusCompact == null)
            {
                return NotFound();
            }

            db.UserStatusCompacts.Remove(userStatusCompact);
            await db.SaveChangesAsync();

            return Ok(userStatusCompact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserStatusCompactExists(int id)
        {
            return db.UserStatusCompacts.Count(e => e.Id == id) > 0;
        }
    }
}