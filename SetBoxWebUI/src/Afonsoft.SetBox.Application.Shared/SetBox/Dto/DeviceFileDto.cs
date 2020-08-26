﻿using Abp.Application.Services.Dto;

namespace Afonsoft.SetBox.SetBox.Dto
{
    public class DeviceFileDto : EntityDto<long>
    {
        public FileDto File { get; set; }
        public int? Order { get; set; }
    }
}
