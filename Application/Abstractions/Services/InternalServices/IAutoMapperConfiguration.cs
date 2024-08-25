using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services;

public interface  IAutoMapperConfiguration
{
    public  List<T> Map<T, TModel>(List<TModel> model) where T : class, new();
    public  T Map<T, TModel>(TModel model) where T : class, new();

}
