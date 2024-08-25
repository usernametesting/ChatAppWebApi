using Application.Abstractions.Services;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Impementions.Helpers;

public class AutoMapperConfiguration : IAutoMapperConfiguration
{

    public T Map<T, TModel>(TModel model) where T : class, new()
    {

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TModel, T>();
        });

        var mapper = configuration.CreateMapper();

        var result = mapper.Map<T>(model);

        return result;
    }

    public List<T> Map<T, TModel>(List<TModel> model) where T : class, new()
    {

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TModel, T>();
        });

        var mapper = configuration.CreateMapper();

        var result = mapper.Map<List<T>>(model);

        return result;
    }
}
