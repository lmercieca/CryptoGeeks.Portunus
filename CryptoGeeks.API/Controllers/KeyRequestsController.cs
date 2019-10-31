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
    public class KeyRequestsController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/KeyRequests
        public IQueryable<KeyRequest> GetKeyRequests()
        {
            return db.KeyRequests;
        }

        [ResponseType(typeof(List<GetKeyRequests_Result>))]
        public List<GetKeyRequests_Result> GetKeyRequestsForUser(Nullable<int> userId)
        {
            List<GetKeyRequests_Result> keyRequest = db.GetKeyRequests(userId).ToList();

            return keyRequest;
        }

        public IQueryable<KeyRequest> GetKeyRequestsForKey(int KeyId)
        {
            return db.KeyRequests.Where(x=>x.KeyID == KeyId);
        }


        public void GetMarkReqyestComplete(int KeyId)
        {
            KeyRequest kr =  db.KeyRequests.Where(x => x.KeyID == KeyId).FirstOrDefault();

            db.Entry(kr).State = EntityState.Modified;
            kr.Completed = true;

            db.SaveChanges();

        }

        public IHttpActionResult GetFragmentAsSent(int fragmentId)
        {
            db.MarkFragmentAsSent(fragmentId);
            return Ok();
        }

        [ResponseType(typeof(List<GetKeyFragmentRequests_Result>))]
        public List<GetKeyFragmentRequests_Result> GetFragmentsForUser(int UserId)
        {
            List<GetKeyFragmentRequests_Result> keyRequest = db.GetKeyFragmentRequests(UserId).ToList();

            return keyRequest;
        }


        // GET: api/KeyRequests/5
        [ResponseType(typeof(KeyRequest))]
        public async Task<IHttpActionResult> GetKeyRequest(int id)
        {
            KeyRequest keyRequest = await db.KeyRequests.FindAsync(id);
            if (keyRequest == null)
            {
                return NotFound();
            }

            return Ok(keyRequest);
        }

        // PUT: api/KeyRequests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutKeyRequest(int id, KeyRequest keyRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != keyRequest.Id)
            {
                return BadRequest();
            }

            db.Entry(keyRequest).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeyRequestExists(id))
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

        // POST: api/KeyRequests
        [ResponseType(typeof(KeyRequest))]
        public async Task<IHttpActionResult> PostKeyRequest(KeyRequest keyRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.KeyRequests.Add(keyRequest);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = keyRequest.Id }, keyRequest);
        }

        // DELETE: api/KeyRequests/5
        [ResponseType(typeof(KeyRequest))]
        public async Task<IHttpActionResult> DeleteKeyRequest(int id)
        {
            KeyRequest keyRequest = await db.KeyRequests.FindAsync(id);
            if (keyRequest == null)
            {
                return NotFound();
            }

            db.KeyRequests.Remove(keyRequest);
            await db.SaveChangesAsync();

            return Ok(keyRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KeyRequestExists(int id)
        {
            return db.KeyRequests.Count(e => e.Id == id) > 0;
        }
    }
}