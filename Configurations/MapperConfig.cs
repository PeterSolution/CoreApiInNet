using AutoMapper;
using CoreApiInNet.Data;
using CoreApiInNet.Model;

namespace CoreApiInNet.Configurations
{
    public class MapperConfig:Profile
    {
        public MapperConfig() 
        {
            CreateMap<DbModelData, HelpingModelData>().ReverseMap();
            CreateMap<FullDataModel, DbModelData>().ReverseMap();

        }
    }
}
