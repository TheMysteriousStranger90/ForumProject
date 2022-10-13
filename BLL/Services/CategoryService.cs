using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDTO> CreateAsync(CategoryDTO model)
        {
            if (model == null) throw new NotFoundException("Category can't be created");

            var category = _mapper.Map<Category>(model);
            await _unitOfWork.CategoryRepository.CreateAsync(category);
            await _unitOfWork.SaveAsync();
            model.Id = category.Id;
            return model;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategory()
        {
            var category = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (category == null) throw new NotFoundException($"This category wasn't found");

            var result = _mapper.Map<IEnumerable<CategoryDTO>>(category);
            return result;
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ForumProjectException("Value of id must be positive");
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null) throw new NotFoundException("This category wasn't found");
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateAsync(CategoryDTO model, int id)
        {
            var categoryUpdate = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (categoryUpdate == null) throw new NotFoundException("Category not found");

            categoryUpdate.Name = model.Name;

            _unitOfWork.CategoryRepository.Update(categoryUpdate);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) throw new NotFoundException("Category not found");

            _unitOfWork.CategoryRepository.Remove(category);
            await _unitOfWork.SaveAsync();
        }
    }
}