﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CryptoGeeks.API
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PortunusEntitiesConnString : DbContext
    {
        public PortunusEntitiesConnString()
            : base("name=PortunusEntitiesConnString")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Fragment> Fragments { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserStatusCompact> UserStatusCompacts { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<UserStatu> UserStatus { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<KeyRequest> KeyRequests { get; set; }
        public virtual DbSet<UserKeyFragment> UserKeyFragments { get; set; }
        public virtual DbSet<Ping> Pings { get; set; }
    
        public virtual int CleanPingsForUser(Nullable<int> userFk)
        {
            var userFkParameter = userFk.HasValue ?
                new ObjectParameter("UserFk", userFk) :
                new ObjectParameter("UserFk", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CleanPingsForUser", userFkParameter);
        }
    
        public virtual ObjectResult<GetContactsForUser_Result> GetContactsForUser(Nullable<int> user)
        {
            var userParameter = user.HasValue ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetContactsForUser_Result>("GetContactsForUser", userParameter);
        }
    
        public virtual ObjectResult<GetAvailableContactsForUser_Result> GetAvailableContactsForUser(Nullable<int> user)
        {
            var userParameter = user.HasValue ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAvailableContactsForUser_Result>("GetAvailableContactsForUser", userParameter);
        }
    
        public virtual ObjectResult<GetKeyFragmentRequests_Result> GetKeyFragmentRequests(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetKeyFragmentRequests_Result>("GetKeyFragmentRequests", userIDParameter);
        }
    
        public virtual int MarkFragmentAsSent(Nullable<int> fragmentId)
        {
            var fragmentIdParameter = fragmentId.HasValue ?
                new ObjectParameter("fragmentId", fragmentId) :
                new ObjectParameter("fragmentId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MarkFragmentAsSent", fragmentIdParameter);
        }
    
        public virtual ObjectResult<GetKeyRequests_Result> GetKeyRequests(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetKeyRequests_Result>("GetKeyRequests", userIdParameter);
        }
    
        public virtual ObjectResult<Fragment> GetPendingFragmentsForUser(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Fragment>("GetPendingFragmentsForUser", userIdParameter);
        }
    
        public virtual ObjectResult<Fragment> GetPendingFragmentsForUser(Nullable<int> userId, MergeOption mergeOption)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Fragment>("GetPendingFragmentsForUser", mergeOption, userIdParameter);
        }
    
        public virtual ObjectResult<GetDashboard_Result> GetDashboard(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDashboard_Result>("GetDashboard", userIDParameter);
        }
    
        public virtual ObjectResult<GetKeysForUser_Result> GetKeysForUser(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetKeysForUser_Result>("GetKeysForUser", userIdParameter);
        }
    
        public virtual ObjectResult<GetFragmentsForUser_Result> GetFragmentsForUser(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetFragmentsForUser_Result>("GetFragmentsForUser", userIDParameter);
        }
    
        public virtual ObjectResult<GetFragmentsForKey_Result> GetFragmentsForKey(Nullable<int> keyId)
        {
            var keyIdParameter = keyId.HasValue ?
                new ObjectParameter("KeyId", keyId) :
                new ObjectParameter("KeyId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetFragmentsForKey_Result>("GetFragmentsForKey", keyIdParameter);
        }
    }
}
