using AuthServer.Core.Dtos.ProductDtos;
using AuthServer.Core.Dtos.UserDtos;
using AuthServer.Core.Entities;
using AutoMapper;

namespace AuthServer.Service.GeneralMapping;

 class DtoMapper : Profile
{
 public DtoMapper()
 {
  CreateMap<Product, GetProductDto>().ReverseMap();
  CreateMap<UserApp, UserAppDto>().ReverseMap();
 }
}