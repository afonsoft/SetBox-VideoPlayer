﻿using System.Collections.Generic;
using Afonsoft.SetBox.Authorization.Permissions.Dto;

namespace Afonsoft.SetBox.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}