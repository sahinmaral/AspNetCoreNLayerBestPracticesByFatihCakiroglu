using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ServiceWithDto<TEntity, TDto> : IServiceWithDto<TEntity, TDto> 
        where TDto : BaseDto, new()
        where TEntity : BaseEntity, new()
    {

        private readonly IGenericRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public ServiceWithDto(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResponseDto<TDto>> AddAsync(TDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            var newDto = _mapper.Map<TDto>(entity);
            return CustomResponseDto<TDto>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<CustomResponseDto<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> dtos)
        {
            IEnumerable<TEntity> entities = _mapper.Map<IEnumerable<TEntity>>(dtos);
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();

            var newDtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return CustomResponseDto<IEnumerable<TDto>>.Success(StatusCodes.Status200OK, newDtos);
        }

        public async Task<CustomResponseDto<bool>> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return CustomResponseDto<bool>.Success(StatusCodes.Status200OK,await _repository.AnyAsync(expression));
        }

        public async Task<CustomResponseDto<NoContentDto>> DeleteAsync(int id)
        {
            TEntity entity = await _repository.GetByIdAsync(id);
            _repository.Delete(entity);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> DeleteRangeAsync(IEnumerable<int> ids)
        {
            IEnumerable<TEntity> entities = await _repository.Where(x => ids.Contains(x.Id)).ToListAsync();
            _repository.DeleteRange(entities);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<IEnumerable<TDto>>> GetAllAsync()
        {
            IEnumerable<TEntity> entities = await _repository.GetAll().ToListAsync();

            IEnumerable<TDto> dtos = _mapper.Map<IEnumerable<TDto>>(entities);

            return CustomResponseDto<IEnumerable<TDto>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<CustomResponseDto<TDto>> GetByIdAsync(int id)
        {
            TEntity entity = await _repository.GetByIdAsync(id);

            TDto dto = _mapper.Map<TDto>(entity);

            return CustomResponseDto<TDto>.Success(StatusCodes.Status200OK, dto);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(TDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression)
        {
            IEnumerable<TDto> dtos = _mapper.Map<IEnumerable<TDto>>(await _repository.Where(expression).ToListAsync());
            return CustomResponseDto<IEnumerable<TDto>>.Success(StatusCodes.Status200OK,dtos);
        }
    }
}
