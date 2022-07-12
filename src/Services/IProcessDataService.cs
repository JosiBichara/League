using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using League.Models;

namespace League.Services
{
    public interface IProcessDataService
    {
        bool IsFileValid(string path);
        FileContent? ReadFile(string path);
        FileContent? Invert(FileContent fileContent);
        string?  Flatten(FileContent fileContent);
        long?  Sum(FileContent fileContent);
        long?  Multiply(FileContent fileContent);
    }
}