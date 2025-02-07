﻿using System.Collections.Generic;

namespace BoltAFE.Domain.Login
{
    public interface ILoginRepository
    {
        List<dynamic> IsValidRequestAccess(string email, string password);
        List<dynamic> IsValidRequestAccessByInstance(string email, string instance_name);
    }
}