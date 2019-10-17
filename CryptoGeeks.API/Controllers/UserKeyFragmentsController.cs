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
    public class UserKeyFragmentsController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/UserKeyFragments
        public IQueryable<UserKeyFragment> GetUserKeyFragments()
        {
            return db.UserKeyFragments;
        }

        public IQueryable<UserKeyFragment> GetUserKeyFragments(int keyId)
        {
            return db.UserKeyFragments.Where(x=>x.KeyID == keyId);
        }

        public IQueryable<UserKeyFragment> GetUserOwnerFragments(int ownerId)
        {
            return db.UserKeyFragments.Where(x => x.OwnerID == ownerId);
        }

        public IQueryable<UserKeyFragment> GetUserRecepientFragments(int recepientID)
        {
            return db.UserKeyFragments.Where(x => x.RecepientID == recepientID);
        }


        // GET: api/UserKeyFragments/5
        [ResponseType(typeof(UserKeyFragment))]
        public async Task<IHttpActionResult> GetUserKeyFragment(int id)
        {
            UserKeyFragment userKeyFragment = await db.UserKeyFragments.FindAsync(id);
            if (userKeyFragment == null)
            {
                return NotFound();
            }

            return Ok(userKeyFragment);
        }

        // PUT: api/UserKeyFragments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserKeyFragment(int id, UserKeyFragment userKeyFragment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userKeyFragment.ID)
            {
                return BadRequest();
            }

            db.Entry(userKeyFragment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserKeyFragmentExists(id))
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

        // POST: api/UserKeyFragments
        [ResponseType(typeof(UserKeyFragment))]
        public async Task<IHttpActionResult> PostUserKeyFragment(UserKeyFragment userKeyFragment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserKeyFragments.Add(userKeyFragment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userKeyFragment.ID }, userKeyFragment);
        }

        // DELETE: api/UserKeyFragments/5
        [ResponseType(typeof(UserKeyFragment))]
        public async Task<IHttpActionResult> DeleteUserKeyFragment(int id)
        {
            UserKeyFragment userKeyFragment = await db.UserKeyFragments.FindAsync(id);
            if (userKeyFragment == null)
            {
                return NotFound();
            }

            db.UserKeyFragments.Remove(userKeyFragment);
            await db.SaveChangesAsync();

            return Ok(userKeyFragment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserKeyFragmentExists(int id)
        {
            return db.UserKeyFragments.Count(e => e.ID == id) > 0;
        }
    }
}