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
    public class KeysController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/Keys
        public IQueryable<Key> GetKeys()
        {
            return db.Keys;
        }


        [Route("api/Keys/GetKeysForUser")]
        public List<GetKeysForUser_Result> GetKeysForUser(int userId)
        {
            List<GetKeysForUser_Result> keys = db.GetKeysForUser(userId).ToList();
            return keys;
        }


        [Route("api/Keys/GetKeysCountForUser")]
        public int GetKeysCountForUser(int userId)
        {
            return db.Keys.Where(x => x.User == userId).Count();
        }




        // GET: api/Keys/5
        
        public Key GetKey(int id)
        {
            Key key = db.Keys.Where(x=>x.Id ==id).FirstOrDefault();
           
            return key;
        }

        // PUT: api/Keys/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutKey(int id, Key key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != key.Id)
            {
                return BadRequest();
            }

            db.Entry(key).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KeyExists(id))
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

        // POST: api/Keys
        [ResponseType(typeof(Key))]
        public async Task<IHttpActionResult> PostKey(Key key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Keys.Add(key);
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CreatedAtRoute("DefaultApi", new { id = key.Id }, key);
        }

        // DELETE: api/Keys/5
        [ResponseType(typeof(Key))]
        public async Task<IHttpActionResult> DeleteKey(int id)
        {
            Key key = await db.Keys.FindAsync(id);
            if (key == null)
            {
                return NotFound();
            }

            db.Keys.Remove(key);
            await db.SaveChangesAsync();

            return Ok(key);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KeyExists(int id)
        {
            return db.Keys.Count(e => e.Id == id) > 0;
        }
    }
}