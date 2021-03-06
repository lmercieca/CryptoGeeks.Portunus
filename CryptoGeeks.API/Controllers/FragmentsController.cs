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
    public class FragmentsController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/Fragments
        public IQueryable<Fragment> GetFragments()
        {
            return db.Fragments;
        }

        public List<GetFragmentsForKey_Result> GetFragmentsForKey(int keyId)
        {

            List<GetFragmentsForKey_Result> result = db.GetFragmentsForKey(keyId).ToList();

            return result;
        }


        public List<Fragment> GetPendingFragmentsForUser(int userId)
        {
            return db.GetPendingFragmentsForUser(userId).ToList();
        }


        public List<GetFragmentsForUser_Result> GetFragmentsForUser(int userId)
        {
            List<GetFragmentsForUser_Result> result = db.GetFragmentsForUser(userId).ToList();

            return result;
        }


        // GET: api/Fragments/5
        [ResponseType(typeof(Fragment))]
        public async Task<IHttpActionResult> GetFragment(int id)
        {
            Fragment fragment = await db.Fragments.FindAsync(id);
            if (fragment == null)
            {
                return NotFound();
            }

            return Ok(fragment);
        }

        // PUT: api/Fragments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFragment(int id, Fragment fragment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fragment.Id)
            {
                return BadRequest();
            }

            db.Entry(fragment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FragmentExists(id))
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

        // POST: api/Fragments
        [ResponseType(typeof(Fragment))]
        public async Task<IHttpActionResult> PostFragment(Fragment fragment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Fragments.Add(fragment);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }

            return CreatedAtRoute("DefaultApi", new { id = fragment.Id }, fragment);
        }

        // DELETE: api/Fragments/5
        [ResponseType(typeof(Fragment))]
        public async Task<IHttpActionResult> DeleteFragment(int id)
        {
            Fragment fragment = await db.Fragments.FindAsync(id);
            if (fragment == null)
            {
                return NotFound();
            }

            db.Fragments.Remove(fragment);
            await db.SaveChangesAsync();

            return Ok(fragment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FragmentExists(int id)
        {
            return db.Fragments.Count(e => e.Id == id) > 0;
        }
    }
}