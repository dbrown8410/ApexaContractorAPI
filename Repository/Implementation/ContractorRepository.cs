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

        public ContractingChainModel GetShortestContractingChain(int contractorOneId, int contractorTwoId)
        {
            var result = new ContractingChainModel();

            var value = "Contracting Chain doesn’t exists";
            try
            {
                var nodeList = new Dictionary<int, HashSet<int>>();

                //check if direct link exists
                var rows = Context.Contracts.Where(x => (x.ContractorOneId == contractorOneId && x.ContractorTwoId == contractorTwoId) 
                    || (x.ContractorOneId == contractorTwoId && x.ContractorTwoId == contractorOneId)).ToList();               
                
                if(rows.Count > 0)
                {
                    var names = new HashSet<int>();
                    foreach (var itm in rows)
                    {
                        names.Add(itm.ContractorOneId);
                        names.Add(itm.ContractorTwoId);
                    }

                    result.Chain = ContractorChain(names);

                    return result;
                }
                else
                {
                    rows = Context.Contracts.Where(x => x.ContractorOneId == contractorOneId).ToList();
                }
                if(rows.Count > 0)
                {  
                    var orignHashSet = new HashSet<int>();
                    var elementCount = 0;
                    foreach (var row in rows)
                    {
                        //destination id
                        orignHashSet.Add(row.ContractorTwoId);
                        //child id
                        rows = Context.Contracts.Where(x => x.ContractorOneId == row.ContractorTwoId).ToList();
                        if(rows.Count > 0)
                        {
                            if (rows[0].ContractorTwoId != contractorOneId)
                            {
                                orignHashSet.Add(rows[0].ContractorTwoId);
                                elementCount += 2;
                            }
                        }
                        elementCount++;
                    };
                    nodeList.Add(contractorOneId, orignHashSet);

                    //get all the contracts for contractor 2
                    rows = Context.Contracts.Where(x => x.ContractorTwoId == contractorTwoId).ToList();

                    if(rows.Count > 0)
                    {
                        var destinationHashSet = new HashSet<int>();
                        
                        foreach (var row in rows)
                        {
                            //destination id
                            destinationHashSet.Add(row.ContractorOneId);
                            //child id
                            rows = Context.Contracts.Where(x => x.ContractorTwoId == row.ContractorOneId).ToList();
                            if (rows.Count > 0)
                            {
                                destinationHashSet.Add(rows[0].ContractorOneId);
                                elementCount++;
                            }
                            elementCount++;
                        };
                        nodeList.Add(contractorTwoId, destinationHashSet);

                        var graph = new Graph(elementCount+1);

                        foreach (var node in nodeList)
                        {
                            var nodeKey = node.Key;
                            var set = node.Value;
                            foreach(var idx in set)
                            {
                                graph.addEdges(nodeKey, idx);
                            }
                        }

                        graph.dfs(contractorOneId);

                        var chain = graph.GetContractingChainList;
                        if (chain.Count > 1)
                        {  
                            value = ContractorChain(chain);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            result.Chain = value;

            return result;        
        }

        private string ContractorChain(HashSet<int> chain)
        {
            var value = "";
            var sortedSet = new SortedSet<string>();
            foreach (var cid in chain)
            {
                var name = Context.Contractor.FirstOrDefault(x => x.Id == cid).Name;
                sortedSet.Add(name);
            }

            foreach (var name in sortedSet)
            {
                value += name + "--";
            }

            return value.Substring(0, value.Length - 2);

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
