using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace PokemonReviewApp.Tests
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private CategoryController _categoryController;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _categoryController = new CategoryController(_mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Test]
        public void GetCategories_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" },
                new CategoryDto { Id = 2, Name = "Category 2" }
            };

            _mockCategoryRepository.Setup(repo => repo.GetCategories()).Returns(categories);
            _mockMapper.Setup(mapper => mapper.Map<List<CategoryDto>>(categories)).Returns(categoryDtos);

            // Act
            var result = _categoryController.GetCategories();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetCategory_ExistingCategoryId_ReturnsOkResult()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };
            var categoryDto = new CategoryDto { Id = categoryId, Name = "Category 1" };

            _mockCategoryRepository.Setup(repo => repo.CategoryExists(categoryId)).Returns(true);
            _mockCategoryRepository.Setup(repo => repo.GetCategory(categoryId)).Returns(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryDto>(category)).Returns(categoryDto);

            // Act
            var result = _categoryController.GetCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetCategory_NonExistingCategoryId_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = 1;

            _mockCategoryRepository.Setup(repo => repo.CategoryExists(categoryId)).Returns(false);

            // Act
            var result = _categoryController.GetCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetPokemonByCategoryId_ExistingCategoryId_ReturnsOkResult()
        {
            // Arrange
            var categoryId = 1;
            var pokemons = new List<Pokemon>
            {
                new Pokemon { Id = 1, Name = "Pokemon 1" },
                new Pokemon { Id = 2, Name = "Pokemon 2" }
            };
            var pokemonDtos = new List<PokemonDto>
            {
                new PokemonDto { Id = 1, Name = "Pokemon 1" },
                new PokemonDto { Id = 2, Name = "Pokemon 2" }
            };

            _mockCategoryRepository.Setup(repo => repo.GetPokemonsByCategory(categoryId)).Returns(pokemons);
            _mockMapper.Setup(mapper => mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonDtos);

            // Act
            var result = _categoryController.GetPokemonByCategoryId(categoryId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateCategory_ValidCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "New Category" };
            var category = new Category { Name = "New Category" };

            _mockCategoryRepository.Setup(repo => repo.GetCategories()).Returns(new List<Category>());
            _mockMapper.Setup(mapper => mapper.Map<Category>(categoryDto)).Returns(category);
            _mockCategoryRepository.Setup(repo => repo.CreateCategory(category)).Returns(true);

            // Act
            var result = _categoryController.CreateCategory(categoryDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateCategory_CategoryAlreadyExists_ReturnsUnprocessableEntityResult()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "Existing Category" };
            var category = new Category { Name = "Existing Category" };
            var existingCategories = new List<Category> { category };

            _mockCategoryRepository.Setup(repo => repo.GetCategories()).Returns(existingCategories);
            _mockMapper.Setup(mapper => mapper.Map<Category>(categoryDto)).Returns(category);

            _categoryController.ModelState.AddModelError("", "Category already exists");

            // Act
            var result = _categoryController.CreateCategory(categoryDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(422, (result as ObjectResult)?.StatusCode);
        }

        [Test]
        public void UpdateCategory_ExistingCategoryIdAndValidCategory_ReturnsNoContentResult()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategoryDto = new CategoryDto { Id = categoryId, Name = "Updated Category" };
            var updatedCategory = new Category { Id = categoryId, Name = "Updated Category" };

            _mockCategoryRepository.Setup(repo => repo.CategoryExists(categoryId)).Returns(true);
            _mockMapper.Setup(mapper => mapper.Map<Category>(updatedCategoryDto)).Returns(updatedCategory);
            _mockCategoryRepository.Setup(repo => repo.UpdateCategory(updatedCategory)).Returns(true);

            // Act
            var result = _categoryController.UpdateCategory(categoryId, updatedCategoryDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void UpdateCategory_NonExistingCategoryId_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategoryDto = new CategoryDto { Id = categoryId, Name = "Updated Category" };

            _mockCategoryRepository.Setup(repo => repo.CategoryExists(categoryId)).Returns(false);

            // Act
            var result = _categoryController.UpdateCategory(categoryId, updatedCategoryDto);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void DeleteCategory_ExistingCategoryId_ReturnsNoContentResult()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };

            _mockCategoryRepository.Setup(repo => repo.CategoryExists(categoryId)).Returns(true);
            _mockCategoryRepository.Setup(repo => repo.GetCategory(categoryId)).Returns(category);
            _mockCategoryRepository.Setup(repo => repo.DeleteCategory(category)).Returns(true);

            // Act
            var result = _categoryController.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void DeleteCategory_NonExistingCategoryId_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = 1;

            _mockCategoryRepository.Setup(repo => repo.CategoryExists(categoryId)).Returns(false);

            // Act
            var result = _categoryController.DeleteCategory(categoryId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
