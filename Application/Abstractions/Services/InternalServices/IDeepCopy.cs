using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.InternalServices;

public  interface IDeepCopy
{
    void Copy<T>(T from,T to) where T : class, new();
}
