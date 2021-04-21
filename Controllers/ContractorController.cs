using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApexaContractorAPI.Entities;
using ApexaContractorAPI.Models;
using ApexaContractorAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApexaContractorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly IContractorService _contractorService;

        public ContractorController(IContractorService contractorService)
        {
            _contractorService = contractorService;
        }
        // GET: api/<ContractorController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ContractorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("contractscontractslist")]
        public List<ContractsModel> GetContractorContracts()
        {
            return _contractorService.GetContractsList();
        }

        [HttpGet("shortestchain/{id1}/{id2}")]
        public ContractingChainModel GetShortestContractingChain(int id1, int id2)
        {
            return _contractorService.GetShortestContractingChain(id1, id2);
        }

        [HttpGet("contractorlist")]
        public List<ContractorModel> GetContractors()
        {
            return _contractorService.GetContractorList();
        }

        // POST api/savecontractor
        [HttpPost("savecontractor")]
        public IActionResult PostSaveContractor([FromBody] ContractorModel contractorDetail)
        {
            if (_contractorService.SaveContractor(contractorDetail))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }

        // POST api/savecontract
        [HttpPost("savecontract")]
        public IActionResult PostSaveContract([FromBody] ContractsModel contractorContracts)
        {
            var id1 = contractorContracts.ContractorOneId;
            var id2 = contractorContracts.ContractorTwoId;  
            if (_contractorService.ValidateSaveContract(id1, id2)){
                if (_contractorService.SaveContract(id1, id2))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }            
        }

        // POST api/<ContractorController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ContractorController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/deletecontract/5
        [HttpDelete("deletecontract/{id}")]
        public IActionResult Delete(int id)
        {
            if (_contractorService.RemoveContract(id))
            {
                return Ok();
            }
            else {
                return BadRequest();
            }
            
        }
    }
}
