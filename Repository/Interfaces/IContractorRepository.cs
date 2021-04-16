using ApexaContractorAPI.Models;
using System.Collections.Generic;

namespace ApexaContractorAPI.Repository.Interfaces
{
    public interface IContractorRepository
    {
        bool SaveContractor(ContractorModel contractor);
        bool ValidateSaveContract(int contractorOneID, int contractorTwoID);
        bool SaveContract(int contractorOneID, int contractorTwoID);
        bool RemoveContract(int contractId);
        List<ContractorModel> GetContractorList();
        List<ContractsModel> GetContractsList();
    }
}
