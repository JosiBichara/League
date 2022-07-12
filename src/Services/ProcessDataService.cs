using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using League.Models;
using System.Text.RegularExpressions;

/// <summary>
///  Class responsible for manipulate the date file
/// </summary>
namespace League.Services
{
    public class ProcessDataService : IProcessDataService
    {
        
        /// <summary>
        /// Method responsible for validating if the file is valid. 
        /// For this application a valid file must have only numbers and commas
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
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

        /// <summary>
        ///  Method responsible for reading a valid file, split for comma and 
        ///  return the matrix as a string in matrix format
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
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

        /// <summary>
        ///  Method responsible for reading a list of string and organize this like a matrix
        ///  Return the matrix as a string in matrix format where the columns and rows are inverted
        /// </summary>
        /// <param name="fileContent">File model</param>
        /// <returns></returns>
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
 
        /// <summary>
        /// Method responsible for reading a list of string and split with commas
        /// Return the matrix as a 1 line string, with values separated by commas
        /// </summary>
        /// <param name="fileContent">File model</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method responsible for reading a list of string and sum all the values
        /// Return the sum of the integers in the matrix
        /// </summary>
        /// <param name="fileContent">File model</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method responsible for reading a list of strings and multiplying all values
        /// Return the product of the integers in the matrix
        /// </summary>
        /// <param name="fileContent">File model</param>
        /// <returns></returns>
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