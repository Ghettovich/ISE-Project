﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AbInitio.Web.DbContexts
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AbInitioEntities : DbContext
    {
        public AbInitioEntities()
            : base("name=AbInitioEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<aanvullendepersoonsinformatie> aanvullendepersoonsinformaties { get; set; }
        public virtual DbSet<aanvullenderelatieinformatie> aanvullenderelatieinformaties { get; set; }
        public virtual DbSet<account> accounts { get; set; }
        public virtual DbSet<meestvoorkomendenaman> meestvoorkomendenamen { get; set; }
        public virtual DbSet<personeninstamboom> personeninstambooms { get; set; }
        public virtual DbSet<persoon> persoons { get; set; }
        public virtual DbSet<persoonhistorie> persoonhistories { get; set; }
        public virtual DbSet<persooninformatietype> persooninformatietypes { get; set; }
        public virtual DbSet<relatie> relaties { get; set; }
        public virtual DbSet<relatiehistorie> relatiehistories { get; set; }
        public virtual DbSet<relatieinformatietype> relatieinformatietypes { get; set; }
        public virtual DbSet<relatietype> relatietypes { get; set; }
        public virtual DbSet<stamboom> stambooms { get; set; }
        public virtual DbSet<stamboomtoegang> stamboomtoegangs { get; set; }
    }
}