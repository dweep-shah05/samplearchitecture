using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HazGo.BuildingBlocks.Core.Common;

namespace HazGo.BuildingBlocks.Core.Domain
{
    public interface IAuthorizationClient
    {
        Task<bool> HasPermission(string role, Module module, Rights rights);
    }
}
