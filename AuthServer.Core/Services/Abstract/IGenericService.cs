using System.Linq.Expressions;
using AuthServer.Core.Dtos.ResponseDtos;

namespace AuthServer.Core.Services.Abstract;

public interface IGenericService<TEntity, TDto> where TEntity:class where TDto:class
{
    Task<ResponseDto<TDto>> GetByIdAsync(int id);
    Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync();
    Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
    Task<ResponseDto<TDto>> AddAsync(TDto tDto);
    Task<ResponseDto<NoDataDto>> Remove(int id);
    Task<ResponseDto<NoDataDto>> Update(TDto tDto, int id);
}