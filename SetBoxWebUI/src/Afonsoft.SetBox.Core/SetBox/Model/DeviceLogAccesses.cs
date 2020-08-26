﻿using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afonsoft.SetBox.SetBox.Model
{
    [Table("AppSetBoxLogAccesses")]
    public class DeviceLogAccesses : FullAuditedEntity<long>
    {
        public virtual Device Device { get; set; }
        public string IpAcessed { get; set; }
        public string Message { get; set; }
    }
}