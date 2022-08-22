using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalParks")]
    [ApiVersion("2.0")]

    // [Route("api/[controller]")]
    [ApiController]
   // [ApiExplorerSettings(GroupName = "ParkingOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : Controller
    {

        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        //constructor

        public NationalParksV2Controller(INationalParkRepository npRepo,IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of all National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDtos>))]
       
        public IActionResult GetNationalParks()
        {
            var obj = _npRepo.GetAllNationalParks().FirstOrDefault();
            //we need to convert objList to Dtos we dont want to expose our data layer to anyone else
           
            return Ok(_mapper.Map<NationalParkDtos>(obj));

        }
     

    }
}
