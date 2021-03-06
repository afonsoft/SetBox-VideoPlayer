﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SetBoxWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Repository.Mapping
{
    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.CompanyId);

            builder.ToTable("SetBoxCompany");
            builder.Property(c => c.CompanyId).HasColumnName("CompanyId");
            builder.Property(c => c.Document).HasColumnName("Document").HasMaxLength(255);
            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(500);
            builder.Property(c => c.Fatansy).HasColumnName("Fatansy").HasMaxLength(500);
            builder.Property(c => c.CreationDateTime).HasColumnName("CreationDateTime").HasDefaultValueSql("getdate()");
            builder.HasMany(c => c.Contacts).WithOne(d => d.Company).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Address).WithOne(d => d.Company).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
