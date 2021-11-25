﻿using PortalFacturas.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalFacturas.Interfaces
{
    public interface IApiCenService
    {
        Task<List<ParticipantResult>> GetParticipantsAsync(string username = null);
        Task<string> GetAccessTokenAsync(string username, string password);
        Task<InstructionModel> GetInstructionsAsync(string creditor, string debtor);
        Task<string> GetXmlFileFromCen(string filepath);
        Task<byte[]> GetPdfFile(string filepath);
        Task<ResponseModel> UploadToFunctionAzure(string res);
        Task GetDocumentos(List<InstructionResult> instructions);
        public Task<byte[]> ConvertToPdf(string content);

    }
}
