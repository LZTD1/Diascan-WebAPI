using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Collections.Generic;
using System.Linq;
    
namespace PokemonReviewApp.Tests.Repository
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private DataContext _context;
        private CategoryRepository _categoryRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            _context = new DataContext(options);
            _categoryRepository = new CategoryRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public void CategoryExists_ExistingCategoryId_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            // Act
            var result = _categoryRepository.CategoryExists(category.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CategoryExists_NonExistingCategoryId_ReturnsFalse()
        {
            // Arrange
            var categoryId = 1;

            // Act
            var result = _categoryRepository.CategoryExists(categoryId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CreateCategory_ValidCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Name = "New Category" };

            // Act
            var result = _categoryRepository.CreateCategory(category);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteCategory_ValidCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            // Act
            var result = _categoryRepository.DeleteCategory(category);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetCategories_WhenCalled_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _context.Categories.AddRange(categories);
            _context.SaveChanges();

            // Act
            var result = _categoryRepository.GetCategories();

            // Assert
            Assert.AreEqual(categories.Count, result.Count);
            Assert.IsTrue(categories.SequenceEqual(result));
        }

        [Test]
        public void GetCategory_ExistingCategoryId_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            // Act
            var result = _categoryRepository.GetCategory(categoryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(categoryId, result.Id);
        }

        [Test]
        public void GetCategory_NonExistingCategoryId_ReturnsNull()
        {
            // Arrange
            var categoryId = 1;

            // Act
            var result = _categoryRepository.GetCategory(categoryId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetPokemonsByCategory_ExistingCategoryId_ReturnsPokemons()
        {
            // Arrange
            var categoryId = 1;
            var pokemon1 = new Pokemon { Id = 1, Name = "Pokemon 1" };
            var pokemon2 = new Pokemon { Id = 2, Name = "Pokemon 2" };
            var category = new Category { Id = categoryId, Name = "Category 1" };
            var pokemonCategories = new List<PokemonCategory>
            {
                new PokemonCategory { CategoryId = categoryId, PokemonId = pokemon1.Id },
                new PokemonCategory { CategoryId = categoryId, PokemonId = pokemon2.Id }
            };

            _context.Categories.Add(category);
            _context.Pokemon.AddRange(pokemon1, pokemon2);
            _context.PokemonCategories.AddRange(pokemonCategories);
            _context.SaveChanges();

            // Act
            var result = _categoryRepository.GetPokemonsByCategory(categoryId);

            // Assert
            Assert.AreEqual(pokemonCategories.Count, result.Count);
            Assert.IsTrue(pokemonCategories.Select(pc => pc.Pokemon).SequenceEqual(result));
        }


        [Test]
        public void Save_WhenCalled_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            _context.Categories.Add(category);

            // Act
            var result = _categoryRepository.Save();

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateCategory_ValidCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            // Act
            var result = _categoryRepository.UpdateCategory(category);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
