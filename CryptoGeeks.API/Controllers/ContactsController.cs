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
    public class ContactsController : ApiController
    {
        private PortunusEntitiesConnString db = new PortunusEntitiesConnString();

        // GET: api/Contacts
        public IQueryable<Contact> GetContacts()
        {
            return db.Contacts;
        }

        public IQueryable<GetContactsForUser_Result> GetContactsForUser(int userId)
        {
            IQueryable<GetContactsForUser_Result> contacts = db.GetContactsForUser(userId).AsQueryable();

            return contacts;
        }

        public int GetContactsCountForUser(int userId)
        {
            return db.GetContactsForUser(userId).Count();

        }

        public IQueryable<GetAvailableContactsForUser_Result> GetAvailableContactsForUser(int userId)
        {
            IQueryable<GetAvailableContactsForUser_Result> contacts = db.GetAvailableContactsForUser(userId).AsQueryable();

            return contacts;
        }


        // GET: api/Contacts/5
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> GetContact(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContact(int id, Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.ID)
            {
                return BadRequest();
            }

            db.Entry(contact).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            bool exist = db.Contacts.Where(c => c.ContactID == contact.ContactID && c.UserID == contact.UserID).Count() > 0 ;

            if (!exist)
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
            }

            return CreatedAtRoute("DefaultApi", new { id = contact.ID }, contact);
        }

        // DELETE: api/Contacts/5
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> DeleteContact(int id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            await db.SaveChangesAsync();

            return Ok(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.ID == id) > 0;
        }
    }
}