using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CabManagment.Models;
using CabManagment.SAL.Interface;

namespace CabManagment.Controllers;
[Route("/")]
[ApiController]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICabService cabService;
    public HomeController(ILogger<HomeController> logger,ICabService cabService)
    {
        _logger = logger;
        this.cabService=cabService;
    }
    [HttpGet("FetchUsers")]
    public JsonResult ShowAll(){
        List<CabUser>clist = cabService.ShowAll();
        return new JsonResult(clist);
    }
    
    [HttpPost("AddUsers/{id}")]
    public JsonResult AddUser(CabUser cuser){
        bool status = cabService.AddUser(cuser);
        if(status){
            return new JsonResult(cuser);
        }
        return new JsonResult(null);
    }
    
    [HttpGet("FetchCabDetails")]
    public JsonResult ShowCab(){
        List<Cab>clist = cabService.ShowCab();
        return new JsonResult(clist);

    }
    
    [HttpGet("FetchDiverDetails")]
    public JsonResult ShowDriver(){
        List<CabDriver> dlist = cabService.ShowDriver();
        return new JsonResult(dlist);
    }

    [HttpGet("FetchBookedDetails")]
    public JsonResult ShowBooking(){
        List<BookedCab>blist = cabService.ShowBooking();
        return new JsonResult(blist);
    }
    
    [HttpPost("AddBooking/{id}")]
    public JsonResult AddBooking(BookedCab booker){
        bool status = cabService.AddBooking(booker);
        if(status){
            return new JsonResult(booker);
        }
        return new JsonResult(null);
    }
    [HttpDelete("RemoveBooking/{id}")]
    public JsonResult RemoveBooking(int id){
         bool status = cabService.RemoveBooking(id);
        if(status){
            return new JsonResult(id);
        }
        return new JsonResult(null);
    }
    [HttpPut("UpdateBooking/{id}")]
    public JsonResult UpdateBooking(BookedCab booker){
         bool status = cabService.UpdateBooking(booker);
        if(status){
            return new JsonResult(booker);
        }
        return new JsonResult(null);
    }

        
}
