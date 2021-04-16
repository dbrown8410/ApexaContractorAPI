using ApexaContractorAPI.Models;
using System.Collections.Generic;

namespace ApexaContractorAPI.Service.Interfaces
{
    public interface IContractorService
    {
        //Create a contractor
        bool SaveContractor(ContractorModel contractor);
        //validate saving contract between 2 contractors
        bool ValidateSaveContract(int contractorOneID, int contractorTwoID);
        //Establish contract between 2 contractors
        bool SaveContract(int contractorOneID, int contractorTwoID);
        //Terminate contract
        bool RemoveContract(int contractId);
        //Get a list of all contractors
        List<ContractorModel> GetContractorList();
        //Get a list of all contracts between contractors
        List<ContractsModel> GetContractsList();
    }
}
