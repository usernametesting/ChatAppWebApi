using Application.Abstractions.Services.InternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impementions.Helpers;

public class DeepCopy : IDeepCopy
{
    public void Copy<T>(T from, T to) where T : class, new()
    {
        foreach (var prop in from.GetType().GetProperties())
            if (prop.GetValue(from) is not null)
                to?.GetType()?.GetProperty(prop.Name)?.SetValue(to, prop.GetValue(from));
    }
}
