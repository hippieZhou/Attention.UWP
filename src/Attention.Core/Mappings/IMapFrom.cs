using AutoMapper;

namespace Attention.Core.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile); //=> profile.CreateMap(typeof(T), GetType());
    }
}
