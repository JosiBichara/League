using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using League.Models;
using League.Services;


namespace League.Controllers;
/// <summary>
///  Controller responsible for initial view requests
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProcessDataService _processDataService;

    public HomeController(ILogger<HomeController> logger, IProcessDataService processDataService)
    {
        _logger = logger;
        _processDataService = processDataService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new HomeViewModel());
    }


    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Index(List<IFormFile> files)
    {
        HomeViewModel viewModel = new HomeViewModel();

        try
        {

            FileContent? fileContent = null;
            
            if(files.Count == 1 ){
                if (files[0].Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName();

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //copy the data to stream
                        await files[0].CopyToAsync(stream);
                    }

                    _logger.LogDebug("Validate the source file");
                    if(_processDataService.IsFileValid(filePath)){
                         _logger.LogDebug("reading the valid file, split for comma and return the matrix as a string in matrix format");
                        fileContent = _processDataService.ReadFile(filePath);
                        
                         _logger.LogDebug("Echo (given) - Return the matrix as a string in matrix format");
                        viewModel.Echo = fileContent;
                        
                         _logger.LogDebug("Invert - Return the matrix as a string in matrix format where the columns and rows are inverted");
                        viewModel.Invert = _processDataService.Invert(fileContent);
                        
                         _logger.LogDebug("Flatten - Return the matrix as a 1 line string, with values separated by commas.");
                        viewModel.Flatten = _processDataService.Flatten(fileContent);
                        
                         _logger.LogDebug("Sum - Return the sum of the integers in the matrix");
                        viewModel.Sum = _processDataService.Sum(fileContent);
                        
                         _logger.LogDebug("Multiply - Return the product of the integers in the matrix");
                        viewModel.Multiply = _processDataService.Multiply(fileContent);

                        ViewBag.Message = "File uploaded successfully!";
                    }else{
                        ViewBag.ErrorMessage = "Invalid file format! It should be a CSV, having only numbers separated by commas.";
                    }

                }

            }
            else if(files.Count > 1){
                ViewBag.ErrorMessage = "Please, submit only a single file!";
            }
            else{
                ViewBag.ErrorMessage = "A file to upload is required!";
            }
        }
        catch(Exception _ex){
            var message = "Error processing the file!";
            _logger.LogError(_ex, "{message} Exception: {exception}", message, _ex );
            
            ViewBag.ErrorMessage = $"{message} Check the logs for more details.";
            viewModel = new HomeViewModel();
        }
        
        return View(viewModel);
    }


    [ResponseCache(Duration = 0)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
