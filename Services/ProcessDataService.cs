using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using League.Models;
using System.Text.RegularExpressions;

namespace League.Services
{
    public class ProcessDataService : IProcessDataService
    {

        #pragma warning disable CS8604

        bool IProcessDataService.IsFileValid(string path){
            try{
                if(! String.IsNullOrEmpty(path) && File.Exists(path)){
                    string? content = string.Join(",", File.ReadAllLines(path));
                    Regex regExp = new Regex(@"[^0-9\,]");
                    return ! regExp.IsMatch(content);
                }

                return false;
            }
            catch(Exception){
                return false;
            }
        }


       FileContent? IProcessDataService.ReadFile(string path)
        {
            FileContent? fileContent = null;

            if(File.Exists(path)){
                string[][] lines = File.ReadLines(path).Select(s => s.Split(",".ToCharArray())).ToArray().ToArray();
                    
                if(lines != null){
                    fileContent = new FileContent();

                    foreach (var item in lines)
                    {
                        fileContent.Add(item.ToList());
                    }
                }
            }

            return fileContent;
        }

        FileContent? IProcessDataService.Invert(FileContent fileContent)
        {
            FileContent? _fileContent = null;

            if(fileContent != null){

                _fileContent = new FileContent();

                for (int i = 0; i < fileContent[0].Count; i++)
                {
                    for (int j = 0;j < fileContent.Count;j++)
                    {
                        if( j == 0)
                            _fileContent.Add(new List<string>());

                        var currentValue = fileContent[j][i];

                        _fileContent[i].Add(currentValue);
                    }
                }
            }

            return _fileContent;
        }
 
        string? IProcessDataService.Flatten(FileContent fileContent)
        {
            
            if(fileContent != null){
                
                string flatten = "";

                foreach (var item in fileContent)
                {
                    flatten += String.Join(",", item);
                    if(! fileContent.Last().Equals(item)){
                        flatten += ",";
                    }
                }

                return flatten;
            }

            return null;
        }

        long? IProcessDataService.Sum(FileContent fileContent)
        {
            if(fileContent != null){
                
                long? sum = 0;

                foreach (var item in fileContent)
                {
                    item.ForEach(x => sum += Convert.ToInt64(x));
                }

                return sum;                
            }

            return null;
            
        }

        long? IProcessDataService.Multiply(FileContent fileContent)
        {
            if(fileContent != null){
                
                long? multiply = 1;

                foreach (var item in fileContent)
                {
                    item.ForEach(x => multiply = multiply * Convert.ToInt64(x));
                }

                return multiply;                
            }

            return null;
        }
        
    }
}