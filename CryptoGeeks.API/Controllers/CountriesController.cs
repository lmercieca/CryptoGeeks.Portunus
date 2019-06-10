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
using CryptoGeeks.API.Models;

namespace CryptoGeeks.API.Controllers
{
    public class CountriesController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/Countries
        public IQueryable<CountryView> GetCountries() 
        {
            return (from c in db.Countries select new CountryView { Name = c.Name }).AsNoTracking();
        }

        // GET: api/Countries/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> GetCountry(int id)
        {

            Country country = await db.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // PUT: api/Countries/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountry(int id, Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != country.Id)
            {
                return BadRequest();
            }

            db.Entry(country).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countries
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> PostCountry(Country country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Countries.Add(country);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> DeleteCountry(int id)
        {
            Country country = await db.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            db.Countries.Remove(country);
            await db.SaveChangesAsync();

            return Ok(country);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CountryExists(int id)
        {
            return db.Countries.Count(e => e.Id == id) > 0;
        }
    }
}