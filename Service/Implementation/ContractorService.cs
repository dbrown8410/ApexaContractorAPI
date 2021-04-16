using ApexaContractorAPI.Models;
using ApexaContractorAPI.Repository.Interfaces;
using ApexaContractorAPI.Service.Interfaces;
using System.Collections.Generic;

namespace ApexaContractorAPI.Service.Implementation
{
    public class ContractorService : BaseService, IContractorService
    {
        private readonly IContractorRepository _contractorRepository;

        public ContractorService(IContractorRepository contractorRepository)
        {
            _contractorRepository = contractorRepository;
        }

        public List<ContractsModel> GetContractsList()
        {
            return _contractorRepository.GetContractsList();
        }

        public List<ContractorModel> GetContractorList()
        {
            return _contractorRepository.GetContractorList();
        }

        public bool RemoveContract(int contractId)
        {
            return _contractorRepository.RemoveContract(contractId);
        }

        public bool SaveContract(int contractorOneID, int contractorTwoID)
        {
            return _contractorRepository.SaveContract(contractorOneID, contractorTwoID);
        }

        public bool SaveContractor(ContractorModel contractor)
        {
            return _contractorRepository.SaveContractor(contractor);
        }

        public bool ValidateSaveContract(int contractorOneID, int contractorTwoID)
        {
            return _contractorRepository.ValidateSaveContract(contractorOneID, contractorTwoID);
        }
    }
}
