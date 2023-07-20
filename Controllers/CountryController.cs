
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly IEntityRepository<Country> _entityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(IEntityRepository<Country> entityRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _entityRepository = entityRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(
                _countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>(
                _countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(
                _countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpGet("{countryId}/getOwners")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetOwnersFromCountry(int countryId)
        {
            var owners = _mapper.Map<List<OwnerDto>>
                (_countryRepository.GetOwnersFromCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var category = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_entityRepository.CreateEntity(countryMap))
            {
                ModelState.AddModelError("", "Something wrong!");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created!");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_entityRepository.UpdateEntity(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound(ModelState);

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_entityRepository.DeleteEntity(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
