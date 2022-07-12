using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using League.Models;
using League.Services;


namespace League.Controllers;

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

                    if(_processDataService.IsFileValid(filePath)){
                        fileContent = _processDataService.ReadFile(filePath);

                        viewModel.Echo = fileContent;
                        viewModel.Invert = _processDataService.Invert(fileContent);
                        viewModel.Flatten = _processDataService.Flatten(fileContent);
                        viewModel.Sum = _processDataService.Sum(fileContent);
                        viewModel.Multiply = _processDataService.Multiply(fileContent);

                        ViewBag.Message = "File upload sucessfuly!";
                    }else{
                        ViewBag.ErrorMessage = "File format invalid!";
                    }

                }

            }
            else if(files.Count > 1){
                ViewBag.ErrorMessage = "Submit only a single form!";
            }
            else{
                ViewBag.ErrorMessage = "File to upload required!";
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
