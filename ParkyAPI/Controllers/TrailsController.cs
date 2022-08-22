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
    [Route("api/v{version:apiVersion}/trails")]
    // [Route("api/Trails")]
    [ApiController]
  //  [ApiExplorerSettings(GroupName = "ParkingOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        //dependency injection
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        //constructor

        public TrailsController(ITrailRepository trailRepo,IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of all Trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
       
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            //we need to convert objList to Dtos we dont want to expose our data layer to anyone else
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objList);

        }
        /// <summary>
        /// Get individual Trail
        /// </summary>
        /// <param name="trailId">Id of the Trail</param>
        /// <returns></returns>
        //get another request to individual Trail
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        //handling error or response types
        [ProducesResponseType(200, Type = typeof(TrailDto))]
      
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {   
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }

            //convert this data to dto
              var objDto =_mapper.Map<TrailDto>(obj);

          
            return Ok(objDto);

        }

        /// <summary>
        /// Get List of Trails with given NationalPark Id
        /// </summary>
        /// <param name="nationalParkId">Id of the Trail</param>
        /// <returns></returns>
       
        [HttpGet("[action]/{nationalParkId:int}")]
        //handling error or response types
        [ProducesResponseType(200, Type = typeof(TrailDto))]

        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }

            //convert this data to dto
            var objDto = new List<TrailDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }


            return Ok(objDto);

        }
        /// <summary>
        /// Create Trail inside Database .
        /// </summary>
        /// <param >it takes Trail Object</param>
        /// <returns></returns>
        //lets write post method
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailInsertDto trailDtos)
        {

            if(trailDtos == null)
            {
                //return bad request
                return BadRequest(ModelState);

            }
            //check for duplicate entry
            if (_trailRepo.TrailExists(trailDtos.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404,ModelState);
            }

            //check for valid object
         /*   if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         */
            //finally we are goin to map the trail dto object to actual trail object
            var trailObj = _mapper.Map<Trail>(trailDtos);
            if (!_trailRepo.CreateTrail(trailObj)) {

                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500,ModelState);
            }

            //if succesful return OK
           // return OK(); //instead just OK 200 we are using CreatedAtRoute and passing 3 arguments
            //this is inside 
            
             return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id },value: trailObj);
          //working fine returning 201
            
           
        }

        /// <summary>
        /// Update Trail , inside data base using Trail Id as ID, and trailDtos
        /// </summary>
        /// <param name="trailId">Id of Trail</param>
        /// <param name="trailDtos">Object of Trail Dto</param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
      
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDtos)
        {
            if (trailDtos == null || trailId!= trailDtos.Id)
            {
                //return bad request
                return BadRequest(ModelState);

            }

            var trailObj = _mapper.Map<Trail>(trailDtos);

            //check for duplicate entry
            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating record {trailObj.Name}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a single Trail Entry inside database.
        /// </summary>
        /// <param name="trailId">Id of Trail </param>
        /// <param name="trailDtos">Trail Object, Name and Distance are required</param>
        /// <returns></returns>
        //HTTP delete

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteTrail(int trailId, [FromBody] TrailDto trailDtos)
        {
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }
            //get object agains this id and pass to delete method
            var trailObj = _trailRepo.GetTrail(trailId);

            //check for duplicate entry
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong deleting record {trailObj.Name}!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
