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
    public class PingsController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/Pings
        public IQueryable<Ping> GetPings()
        {
            return db.Pings;
        }

        // GET: api/Pings/5
        [ResponseType(typeof(Ping))]
        public async Task<IHttpActionResult> GetPing(int id)
        {
            Ping ping = await db.Pings.FindAsync(id);
            if (ping == null)
            {
                return NotFound();
            }

            return Ok(ping);
        }

        // PUT: api/Pings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPing(int id, Ping ping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ping.Id)
            {
                return BadRequest();
            }

            db.Entry(ping).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PingExists(id))
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

        // POST: api/Pings
        [ResponseType(typeof(Ping))]
        public async Task<IHttpActionResult> PostPing(Ping ping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CleanPingsForUser(ping.User_Fk);

            db.Pings.Add(ping);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ping.Id }, ping);
        }

        // DELETE: api/Pings/5
        [ResponseType(typeof(Ping))]
        public async Task<IHttpActionResult> DeletePing(int id)
        {
            Ping ping = await db.Pings.FindAsync(id);
            if (ping == null)
            {
                return NotFound();
            }

            db.Pings.Remove(ping);
            await db.SaveChangesAsync();

            return Ok(ping);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PingExists(int id)
        {
            return db.Pings.Count(e => e.Id == id) > 0;
        }
    }
}