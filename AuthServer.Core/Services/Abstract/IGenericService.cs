using System.Linq.Expressions;
using AuthServer.Core.Dtos;

namespace AuthServer.Core.Services.Abstract;

public interface IGenericService<TEntity, TDto> where TEntity:class where TDto:class
{
    Task<ResponseDto<TDto>> GetByIdAsync(int id);
    Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync();
    Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
    Task<ResponseDto<TDto>> AddAsync(TEntity entity);
    Task<ResponseDto<NoDataDto>> Remove(TEntity entity);
    Task<ResponseDto<NoDataDto>> Update(TEntity entity);
}