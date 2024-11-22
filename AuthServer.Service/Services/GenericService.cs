using System.Linq.Expressions;
using AuthServer.Core.Dtos;
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
        var product = await _genericRepository.GetByIdAsync(id);
        if (product == null)
        {
            var failerResponseMessage = ResponseDto<TDto>.Fail("Id not found", 404, true);
        }

        var productDto = ObjectMapper.Mapper.Map<TDto>(product);
        var responseMessage = ResponseDto<TDto>.Success(productDto, 200);
        return responseMessage;
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync()
    {
        var productsFromRepository = await _genericRepository.GetAllAsync();
        var productList = ObjectMapper.Mapper.Map<List<TDto>>(productsFromRepository);
        var responseMessage = ResponseDto<IEnumerable<TDto>>.Success(productList, 200);
        return responseMessage;
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        var list = _genericRepository.Where(predicate);
        var mappedObject = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());
        var responseMessage = ResponseDto<IEnumerable<TDto>>.Success(mappedObject, 200);
        return responseMessage;
    }

    public async Task<ResponseDto<TDto>> AddAsync(TDto tDto)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(tDto);
        await _genericRepository.AddAsync(newEntity);
        await _unitOfWork.SaveChangesAsync();

        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
        var responseMessage = ResponseDto<TDto>.Success(newDto, 200);
        return responseMessage;
    }

    public async Task<ResponseDto<NoDataDto>> Remove(int id)
    {
        var isExist = await _genericRepository.GetByIdAsync(id);
        if (isExist == null)
        {
            var failerResponseMessage = ResponseDto<NoDataDto>.Fail("Id not found", 404, true);
            return failerResponseMessage;
        }
        _genericRepository.Remove(isExist);
        await _unitOfWork.SaveChangesAsync();
        var responseMessage = ResponseDto<NoDataDto>.Success(204);
        return responseMessage;
    }

    public async Task<ResponseDto<NoDataDto>> Update(TDto tDto, int id)
    {
        var isExist = await _genericRepository.GetByIdAsync(id);
        if (isExist == null)
        {
            var failerResponseMessage = ResponseDto<NoDataDto>.Fail("Id not found", 404, true);
            return failerResponseMessage;
        }

        var entity = ObjectMapper.Mapper.Map<TEntity>(tDto);
        _genericRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        var responseMessage = ResponseDto<NoDataDto>.Success(204);
        return responseMessage;
    }
}