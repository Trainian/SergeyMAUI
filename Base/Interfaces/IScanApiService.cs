using Base.Enums;
using Base.Models;
using Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Interfaces
{
    public interface IScanApiService
    {
        Task<string> SendScannedCodes (ScannedCodesViewModel scannedCodes, MethodsToSend method);
        Task<ScanElement> GetScannedCodeInformation(ScanElement scannedCode);
        Task GetScannedCodesInformation(ScannedCodesViewModel scannedCodes);
    }
}
