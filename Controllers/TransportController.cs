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
    public class TransportController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly ITransportService _transportService;
        private readonly TransportContext db;

        public TransportController(ILogger<PersonController> logger, ITransportService transportService, TransportContext db)
        {
            _logger = logger;
            _transportService = transportService;
            this.db = db;
        }

        [HttpGet("[action]")]
        public async Task<CustomResponseModel<IEnumerable<ViewTransportModel>>> GetAll()
        {
            try
            {
                // get all the transports from Database
                var transports = await db.Transport.Include(x => x.Driver).ToListAsync();

                // create empty list of view models
                var viewTransportModels = new List<ViewTransportModel>();

                // map entities to models
                foreach (var transport in transports)
                {
                    viewTransportModels.Add(new()
                    {
                        Id = transport.Id,
                        Driver = new()
                        {
                            Id = transport.Driver.Id,
                            FullName = transport.Driver.FullName,
                            Age = transport.Driver.Age,
                            IsDriver = transport.Driver.IsDriver
                        },
                        ModelName = transport.ModelName,
                        Price = transport.Price,
                        ProducerCompany = transport.ProducerCompany,
                        TransportType = transport.TransportType
                    });
                }

                // return response model
                return new CustomResponseModel<IEnumerable<ViewTransportModel>>()
                {
                    StatusCode = 200,
                    Result = viewTransportModels
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<IEnumerable<ViewTransportModel>>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }
        // get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<CustomResponseModel<ViewTransportModel>> GetById(int id)
        {
            try
            {
                // get transport from Database
                var transport = await db.Transport.FirstOrDefaultAsync(x => x.Id == id);

                // check if entity exists in Database
                if (id < 0 || transport == null)
                    return new CustomResponseModel<ViewTransportModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map entity to view model
                var viewTransportModel = new ViewTransportModel()
                {
                    Id = transport.Id,
                    Driver = new()
                    {
                        Id = transport.Driver.Id,
                        FullName = transport.Driver.FullName,
                        Age = transport.Driver.Age,
                        IsDriver = transport.Driver.IsDriver
                    },
                    ModelName = transport.ModelName,
                    Price = transport.Price,
                    ProducerCompany = transport.ProducerCompany,
                    TransportType = transport.TransportType
                };

                // return success code and respoce model
                return new CustomResponseModel<ViewTransportModel>()
                {
                    StatusCode = 200,
                    Result = viewTransportModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewTransportModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // add new item to Database
        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Create([FromQuery] CreateTransportModel createTransportModel)
        {
            try
            {
                // check if transport values are valid
                if (!_transportService.isValidTransport(createTransportModel))
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Invalid values",
                        Result = false
                    };

                // find person from database
                var person = await db.Person.FirstOrDefaultAsync(x => x.Id == createTransportModel.DriverId);

                // map create model to base entity
                var transport = new Transport()
                {
                    Driver = person,
                    ModelName = createTransportModel.ModelName,
                    Price = createTransportModel.Price,
                    ProducerCompany = createTransportModel.ProducerCompany,
                    TransportType = createTransportModel.TransportType
                };

                // add entity in database
                await db.AddAsync(transport);
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
        public async Task<CustomResponseModel<ViewTransportModel>> Update([FromQuery] UpdateTransportModel updateTransportModel)
        {
            try
            {
                // get entity from Database
                var transport = await db.Transport.Include(x => x.Driver).FirstOrDefaultAsync(x => x.Id == updateTransportModel.Id);

                // check if entity exists
                if (transport == null)
                    return new CustomResponseModel<ViewTransportModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map entity to view model
                var viewTransportModel = new ViewTransportModel()
                {
                    Id = transport.Id,
                    Driver = new()
                    {
                        Id = transport.Driver.Id,
                        FullName = transport.Driver.FullName,
                        Age = transport.Driver.Age,
                        IsDriver = transport.Driver.IsDriver
                    },
                    ModelName = transport.ModelName,
                    Price = transport.Price,
                    ProducerCompany = transport.ProducerCompany,
                    TransportType = transport.TransportType
                };

                // update in Database
                db.Transport.Update(transport);
                await db.SaveChangesAsync();

                // return success code and updated view model
                return new CustomResponseModel<ViewTransportModel>()
                {
                    StatusCode = 200,
                    Result = viewTransportModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewTransportModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // delete item from Database
        [HttpDelete("[action]/{id}")]
        public async Task<CustomResponseModel<ViewTransportModel>> Delete(int id)
        {
            try
            {
                // get entity from Database
                var transport = await db.Transport.Include(x => x.Driver).FirstOrDefaultAsync(x => x.Id == id);

                // check if entity exists
                if (transport == null)
                    return new CustomResponseModel<ViewTransportModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map entity to view model
                var viewTransportModel = new ViewTransportModel()
                {
                    Id = transport.Id,
                    Driver = new()
                    {
                        Id = transport.Driver.Id,
                        FullName = transport.Driver.FullName,
                        Age = transport.Driver.Age,
                        IsDriver = transport.Driver.IsDriver
                    },
                    ModelName = transport.ModelName,
                    Price = transport.Price,
                    ProducerCompany = transport.ProducerCompany,
                    TransportType = transport.TransportType
                };

                // remove from Database
                db.Transport.Remove(transport);
                await db.SaveChangesAsync();

                // return success code and updated view model
                return new CustomResponseModel<ViewTransportModel>()
                {
                    StatusCode = 200,
                    Result = viewTransportModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewTransportModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }
    }
}
