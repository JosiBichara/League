using League.Models;
using League.Services;

namespace League.Tests;

public class Tests
{
    string pathFileValid = Path.GetTempFileName(), pathFileInvalid = Path.GetTempFileName();
    IProcessDataService _processDataService;
    FileContent fileContent;

    [SetUp]
    public void Setup()
    {
        CreateTestFiles();
        _processDataService = new ProcessDataService();
        fileContent = _processDataService.ReadFile(pathFileValid); 
    }

    private void CreateTestFiles(){
        using(var fileValid = File.CreateText(pathFileValid)){
            fileValid.WriteLine("1,2,3");
            fileValid.WriteLine("4,5,6");
            fileValid.WriteLine("7,8,9");
            fileValid.Close();
        }

        using(var fileInvalid = File.CreateText(pathFileInvalid)){
            fileInvalid.WriteLine("ABC,2,3");
            fileInvalid.Close();
        }
    }

    [Test]
    public void IsFileValid_FileValid()
    {
        Assert.IsTrue(_processDataService.IsFileValid(pathFileValid));
    }

    [Test]
    public void IsFileValid_FileInvalid()
    {
        Assert.IsFalse(_processDataService.IsFileValid(pathFileInvalid));
    }


    [Test]
    public void ReadFile_ReturnOK()
    {
        List<List<string>> contentExpected = new List<List<string>>();
        contentExpected.Add(new List<string>{"1","2","3"});
        contentExpected.Add(new List<string>{"4","5","6"});
        contentExpected.Add(new List<string>{"7","8","9"});

        string contentText = "";
        foreach(var item in contentExpected){ 
            contentText += string.Join("", item);
        }

        string contentFile = "";
        foreach(var item in fileContent){ 
            contentFile += string.Join("", item);
        }

        Assert.IsTrue(contentText.Equals(contentFile));
    }

    [Test]
    public void Invert_ReturnOK()
    {
        List<List<string>> contentExpected = new List<List<string>>();
        contentExpected.Add(new List<string>{"1","4","7"});
        contentExpected.Add(new List<string>{"2","5","8"});
        contentExpected.Add(new List<string>{"3","6","9"});

        var fileInverted = _processDataService.Invert(fileContent);

        string contentText = "";
        foreach(var item in contentExpected){ 
            contentText += string.Join("", item);
        }

        string contentFile = "";
        foreach(var item in fileInverted){ 
            contentFile += string.Join("", item);
        }

        Assert.IsTrue(contentText.Equals(contentFile));
    }

    [Test]
    public void Flatten_ReturnOK()
    {
        string contentExpected = "1,2,3,4,5,6,7,8,9";
        Assert.IsTrue(contentExpected.Equals(_processDataService.Flatten(fileContent)));
    }

    [Test]
    public void Sum_ReturnOK()
    {
        long? numberExpected = 45;
        Assert.IsTrue(numberExpected == _processDataService.Sum(fileContent));
    }

    [Test]
    public void Multiply_ReturnOK()
    {
        long? numberExpected = 362880;
        Assert.IsTrue(numberExpected == _processDataService.Multiply(fileContent));
    }
}