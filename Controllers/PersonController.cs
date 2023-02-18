namespace Transport.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Transport.API.Context;
    using Transport.API.Entities;
    using Transport.API.Interfaces;
    using Transport.API.Models;
    using Transport.API.Models.CreateModels;
    using Transport.API.Models.UpdateModels;
    using Transport.API.Models.ViewModels;

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;
        private readonly TransportContext db;

        public PersonController(ILogger<PersonController> logger, IPersonService personService, TransportContext db)
        {
            _logger = logger;
            _personService = personService;
            this.db = db;
        }

        [HttpGet("[action]")]
        public async Task<CustomResponseModel<IEnumerable<ViewPersonModel>>> GetAll()
        {
            try
            {
                // get all the people from Database
                var people = await db.Person.ToListAsync();

                // create empty list of view models
                var viewPersonModels = new List<ViewPersonModel>();

                // map entities to models
                foreach (var person in people)
                {
                    viewPersonModels.Add(new()
                    {
                        Id = person.Id,
                        FullName = person.FullName,
                        Age = person.Age,
                        IsDriver = person.IsDriver
                    });
                }

                // return response model
                return new CustomResponseModel<IEnumerable<ViewPersonModel>>()
                {
                    StatusCode = 200,
                    Result = viewPersonModels
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 400
                return new CustomResponseModel<IEnumerable<ViewPersonModel>>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }
        // get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<CustomResponseModel<ViewPersonModel>> GetById(int id)
        {
            try
            {
                // check if id is valid
                if (id <= 0)
                    return new CustomResponseModel<ViewPersonModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Please enter valid Id"
                    };

                // get person from Database
                var person = await db.Person.FirstOrDefaultAsync(x => x.Id == id);

                // check if entity exists in Database
                if (person == null)
                    return new CustomResponseModel<ViewPersonModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map entity to view model
                var viewPersonModel = new ViewPersonModel()
                {
                    Id = person.Id,
                    FullName = person.FullName,
                    Age = person.Age,
                    IsDriver = person.IsDriver
                };

                // return success code and respoce model
                return new CustomResponseModel<ViewPersonModel>()
                {
                    StatusCode = 200,
                    Result = viewPersonModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 400
                return new CustomResponseModel<ViewPersonModel>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // add new item to Database
        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Create([FromQuery] CreatePersonModel createPersonModel)
        {
            try
            {
                // check if person values are valid
                if (!_personService.IsValidPerson(createPersonModel))
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Invalid values",
                        Result = false
                    };

                // map create model to base entity
                var person = new Person()
                {
                    FullName = createPersonModel.FullName,
                    Age = createPersonModel.Age,
                    IsDriver = createPersonModel.IsDriver
                };

                // add entity in database
                await db.AddAsync(person);
                await db.SaveChangesAsync();

                // return success code
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 200,
                    Result = true
                };

            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // update item in Database
        [HttpPut("[action]")]
        public async Task<CustomResponseModel<ViewPersonModel>> Update([FromQuery] UpdatePersonModel updatePersonModel)
        {
            try
            {
                // get entity from Database
                var person = await db.Person.FirstOrDefaultAsync(x => x.Id == updatePersonModel.Id);

                // check if entity exists
                if (person == null)
                    return new CustomResponseModel<ViewPersonModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map entity to view model
                var viewPersonModel = new ViewPersonModel()
                {
                    Id = updatePersonModel.Id,
                    FullName = updatePersonModel.FullName,
                    Age = updatePersonModel.Age,
                    IsDriver = updatePersonModel.IsDriver
                };

                // update in Database
                db.Person.Update(person);
                await db.SaveChangesAsync();

                // return success code and updated view model
                return new CustomResponseModel<ViewPersonModel>()
                {
                    StatusCode = 200,
                    Result = viewPersonModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewPersonModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // delete item from Database
        [HttpDelete("[action]/{id}")]
        public async Task<CustomResponseModel<ViewPersonModel>> Delete(int id)
        {
            try
            {
                // get entity from Database
                var person = await db.Person.FirstOrDefaultAsync(x => x.Id == id);

                // check if entity exists
                if (person == null)
                    return new CustomResponseModel<ViewPersonModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map entity to view model
                var viewPersonModel = new ViewPersonModel()
                {
                    Id = person.Id,
                    FullName = person.FullName,
                    Age = person.Age,
                    IsDriver = person.IsDriver
                };

                // remove from Database
                db.Person.Remove(person);
                await db.SaveChangesAsync();

                // return success code and updated view model
                return new CustomResponseModel<ViewPersonModel>()
                {
                    StatusCode = 200,
                    Result = viewPersonModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewPersonModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }
    }
}
