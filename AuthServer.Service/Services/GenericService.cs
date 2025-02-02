using System.Linq.Expressions;
using System.Net;
using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.ResponseDtos;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services.Abstract;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.GeneralMapping;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Service.Services;

public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity:class where TDto:class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _genericRepository;

    public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = genericRepository;
    }

    public async Task<ResponseDto<TDto>> GetByIdAsync(int id)
    {
        var entity = await _genericRepository.GetByIdAsync(id);
        if (entity is null)
        {
            var failerResponseMessage = ResponseDto<TDto>.Fail("entity cannot found");
        }

        var entityDto = ObjectMapper.Mapper.Map<TDto>(entity);
        var responseMessage = ResponseDto<TDto>.Success(entityDto);
        return responseMessage;
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync()
    {
        var entities = await _genericRepository.GetAllAsync();
        var entityDtoList = ObjectMapper.Mapper.Map<List<TDto>>(entities);
        var responseMessage = ResponseDto<IEnumerable<TDto>>.Success(entityDtoList);
        return responseMessage;
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        var list = await _genericRepository.Where(predicate).ToListAsync();
        var mappedObject = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list);
        var responseMessage = ResponseDto<IEnumerable<TDto>>.Success(mappedObject);
        return responseMessage;
    }

    public async Task<ResponseDto<TDto>> AddAsync(TDto tDto)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(tDto);
        await _genericRepository.AddAsync(newEntity);
        await _unitOfWork.SaveChangesAsync();

        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
        var responseMessage = ResponseDto<TDto>.Success(newDto, HttpStatusCode.Created);
        return responseMessage;
    }

    public async Task<ResponseDto<NoDataDto>> Remove(int id)
    {
        var isExist = await _genericRepository.GetByIdAsync(id);
        if (isExist is null)
        {
            var failerResponseMessage = ResponseDto<NoDataDto>.Fail("Id not found");
            return failerResponseMessage;
        }
        _genericRepository.Remove(isExist);
        await _unitOfWork.SaveChangesAsync();
        var responseMessage = ResponseDto<NoDataDto>.Success();
        return responseMessage;
    }

    public async Task<ResponseDto<NoDataDto>> Update(TDto tDto, int id)
    {
        var isExist = await _genericRepository.GetByIdAsync(id);
        if (isExist is null)
        {
            var failerResponseMessage = ResponseDto<NoDataDto>.Fail("Id not found", HttpStatusCode.NoContent);
            return failerResponseMessage;
        }

        var entity = ObjectMapper.Mapper.Map<TEntity>(tDto);
        _genericRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        var responseMessage = ResponseDto<NoDataDto>.Success();
        return responseMessage;
    }
}