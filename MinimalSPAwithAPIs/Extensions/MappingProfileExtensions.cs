namespace MinimalSPAwithAPIs.Extensions;

public class MappingProfileExtensions : Profile
{
    public MappingProfileExtensions()
    {
        CreateMap<MyFirstApiDTO, MyFirstApiDbTable>()
            .ForMember(dest => dest.PrimaryKey, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<MyUsersDTO, MyUsersDbTable>()
            .ForMember(dest => dest.PrimaryKey, opt => opt.Ignore())
            .ReverseMap();
    }

    private void CreateTupleMap<TSource1, TSource2, TDestination>()
    {
        var type1 = typeof(TSource1);
        var type2 = typeof(TSource2);
        var destinationType = typeof(TDestination);

        var map = CreateMap<(TSource1, TSource2), TDestination>();

        foreach (var destProp in destinationType.GetProperties())
        {
            var sourceProp1 = type1.GetProperty(destProp.Name);
            var sourceProp2 = type2.GetProperty(destProp.Name);

            if (sourceProp1 != null)
            {
                map.ForMember(destProp.Name, opt => opt.MapFrom(src => GetPropertyValue(src.Item1, sourceProp1)));
            }
            else if (sourceProp2 != null)
            {
                map.ForMember(destProp.Name, opt => opt.MapFrom(src => GetPropertyValue(src.Item2, sourceProp2)));
            }
        }
    }

    private object GetPropertyValue(object obj, System.Reflection.PropertyInfo prop)
    {
        return prop.GetValue(obj, null);
    }
}
