﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    public class Company
    {
        public Guid CompanyId { get; set; }
        public string FullName { get; set; }
        public string CNPJ { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }

    public class Address
    {
        public Company Company { get; set; }
        public Guid AddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
    }
}