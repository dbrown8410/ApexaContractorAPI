using ApexaContractorAPI.Classes;
using ApexaContractorAPI.Entities;
using ApexaContractorAPI.Models;
using ApexaContractorAPI.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApexaContractorAPI.Repository.Implementation
{
    public class ContractorRepository : BaseRepository, IContractorRepository
    {
        private readonly IMapper _mapper;

        public ContractorRepository(ContractorDBContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public List<ContractsModel> GetContractsList()
        {
            var list = new List<ContractsModel>();
            try
            {
                var rows = Context.Contracts.ToList();
                if (rows != null)
                {
                    list = _mapper.Map<List<ContractsModel>>(rows);
                    foreach(var row in list)
                    {
                        row.ContractorOneName = Context.Contractor.FirstOrDefault(x => x.Id == row.ContractorOneId).Name;
                        row.ContractorTwoName = Context.Contractor.FirstOrDefault(x => x.Id == row.ContractorTwoId).Name;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<ContractorModel> GetContractorList()
        {
            var list = new List<ContractorModel>();
            try
            {
                var rows = Context.Contractor.ToList();
                if(rows != null)
                {
                    list = _mapper.Map<List<ContractorModel>>(rows);
                }
            }catch(Exception ex)
            {

            }
            return list;
        }

        public bool RemoveContract(int contractId)
        {
            var removed = false;
            try
            {
                var data = Context.Contracts.FirstOrDefault(x => x.Id == contractId);
                Context.Contracts.Remove(data);
                Context.SaveChanges();
                removed = true;
            }catch(Exception ex)
            {
                removed = false;
            }
            return removed;
        }

        public bool SaveContract(int contractorOneID, int contractorTwoID)
        {
            var saved = false;
            try
            {
                var data = new Contracts
                {
                    Id = 0,
                    ContractorOneId = contractorOneID,
                    ContractorTwoId = contractorTwoID,
                };

                Context.Contracts.Add(data);
                Context.SaveChanges();
                saved = true;
            }
            catch (Exception ex)
            {
                saved = false;
            }

            return saved;
        }

        public bool SaveContractor(ContractorModel contractor)
        {
            var saved = false;
            try
            {
                var id = contractor.Id;
                var data = _mapper.Map<Contractor>(contractor);
                if (id <= 0)
                {
                    //generate a random health status
                    data.HealthStatus = HealthStatus.GetRandomHealthStatusByProbability();
                }

                Context.Contractor.Add(data);
                Context.SaveChanges();
                saved = true;
            }
            catch(Exception ex)
            {
                saved = false;
            }

            return saved;
        }

        public bool ValidateSaveContract(int contractorOneID, int contractorTwoID)
        {
            var valid = contractorOneID == contractorTwoID;
            //cannot extablish contract with yourself
            if (valid)
                return false;
            try
            {
                //valid to save when contract doesn't already exist
                valid = Context.Contracts.Any(x => (x.ContractorOneId == contractorOneID && x.ContractorTwoId == contractorTwoID)||
                    (x.ContractorOneId == contractorTwoID && x.ContractorTwoId == contractorOneID)) == false;
            }
            catch(Exception ex)
            {

            }

            return valid;
        }
    }
}
