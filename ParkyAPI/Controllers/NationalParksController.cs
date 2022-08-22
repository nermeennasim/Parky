using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalParks")]
   // [Route("api/[controller]")]
    [ApiController]
   // [ApiExplorerSettings(GroupName = "ParkingOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : Controller
    {

        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        //constructor

        public NationalParksController(INationalParkRepository npRepo,IMapper mapper)
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
       
        public IActionResult GetAllNationalParks()
        {
            var objList = _npRepo.GetAllNationalParks();
            //we need to convert objList to Dtos we dont want to expose our data layer to anyone else
            var objDto = new List<NationalParkDtos>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDtos>(obj));
            }
            return Ok(objList);

        }
        /// <summary>
        /// Get individual National Park
        /// </summary>
        /// <param name="NationalParkId">Id of the National Park</param>
        /// <returns></returns>
        //get another request fo tindidual park
        [HttpGet("{NationalParkId:int}", Name = "GetNationalPark")]
        //handling error or response types
        [ProducesResponseType(200, Type = typeof(NationalParkDtos))]
      
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalParks(int NationalParkId)
        {   
            var obj = _npRepo.GetNationalPark(NationalParkId);
            if (obj == null)
            {
                return NotFound();
            }

            //convert this data to dto
              var objDto =_mapper.Map<NationalParkDtos>(obj);

            //instad of mapping
            //we can do it manually
            //but mapper does this for us in one line
          /*  var ObjDto = new NationalParkDtos()
            {
                Created= obj.Created,
                Id = obj.Id,
                Name = obj.Name,
                State = obj.State,


            };*/
            return Ok(objDto);

        }

        /// <summary>
        /// Create National Park inside Database .
        /// </summary>
        /// <param name="nationalParkDtos">it takes National Park Object</param>
        /// <returns></returns>
        //lets write post method
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDtos))]
        [ProducesResponseType(StatusCodes.Status201Created)]
       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDtos nationalParkDtos)
        {

            if(nationalParkDtos == null)
            {
                //return bad request
                return BadRequest(ModelState);

            }
            //check for duplicate entry
            if (_npRepo.NationalParkExists(nationalParkDtos.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404,ModelState);
            }

            //check for valid object
         /*   if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         */
            //finally we are goin to map the dto object to actual national park object
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDtos);
            if (!_npRepo.CreateNationalPark(nationalParkObj)) {

                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500,ModelState);
            }

            //if succesful return OK
           // return OK(); //instead just OK 200 we are using CreatedAtRoute and passing 3 arguments
            //this is inside 
            
             return CreatedAtRoute("GetNationalPark", new {version = HttpContext.GetRequestedApiVersion().ToString() ,
                 nationalParkId = nationalParkObj.Id },value: nationalParkObj);
          //working fine returning 201
            
           
        }

        /// <summary>
        /// Update National park , inside data base using NationalParkId as ID, and nationalParkDtos
        /// </summary>
        /// <param name="NationalParkId">Id of National Park</param>
        /// <param name="nationalParkDtos">Object of National Park</param>
        /// <returns></returns>
        [HttpPatch("{NationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
      
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateNationalPark(int NationalParkId, [FromBody] NationalParkDtos nationalParkDtos)
        {
            if (nationalParkDtos == null || NationalParkId!= nationalParkDtos.Id)
            {
                //return bad request
                return BadRequest(ModelState);

            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDtos);

            //check for duplicate entry
            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating record {nationalParkObj.Name}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a single National park Entry inside database.
        /// </summary>
        /// <param name="NationalParkId">Id of National Park </param>
       
        /// <returns></returns>
        //HTTP delete

        [HttpDelete("{NationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteNationalPark(int NationalParkId)
        {
            if (!_npRepo.NationalParkExists(NationalParkId))
            {
                return NotFound();
            }
            //get object agains this id and pass to delete method
            var nationalParkObj = _npRepo.GetNationalPark(NationalParkId);

            //check for duplicate entry
            if (!_npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong deleting record {nationalParkObj.Name}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
