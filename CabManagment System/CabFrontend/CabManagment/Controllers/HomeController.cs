using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CabManagment.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Net.Http.Headers;

namespace CabManagment.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> UserSelction(){
         using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await client.GetAsync("FetchUsers");
             
             if(getData.IsSuccessStatusCode){
                //string result = getData.Content.ReadAsStringAsync().Result;
                HttpContext.Session.SetString("ResultOfCList", getData.Content.ReadAsStringAsync().Result);
                // var result = HttpContext.Session.GetString("ResultOfCList");
                 //  List<CabUser>ulist = JsonConvert.DeserializeObject<List<CabUser>>(result);
             }
         }
        return View();
    }
    [HttpGet]
    public IActionResult NewUser(){
        var result = HttpContext.Session.GetString("ResultOfCList");
        List<CabUser>ulist = JsonConvert.DeserializeObject<List<CabUser>>(result);
        int max = 0;
        foreach(CabUser cu in ulist){
           if(max<cu.Id){
            max = cu.Id;
           }
        }
        max+=1;
        ViewData["cuid"] = max;
        
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> NewUser(int cid,string uname,string uphone){
        CabUser cb = new CabUser();
        cb.Id = cid;
        cb.Uname = uname;
        cb.Uphoneno = uphone;
        using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020/"+"AddUsers/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //sending value to another application in key value format
            
            HttpResponseMessage getData =await client.PostAsJsonAsync<CabUser>(cid+"",cb);
            
        
            if(getData.IsSuccessStatusCode){
                HttpContext.Session.SetInt32("CurrentUser",cb.Id);                 
            }
        }

         using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await client.GetAsync("FetchUsers");
             
             if(getData.IsSuccessStatusCode){                
                HttpContext.Session.SetString("UpdatedResultOfCList", getData.Content.ReadAsStringAsync().Result);
                return RedirectToAction("CabOrderIndex","Home");
             }
         }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> CabOrderIndex(){
        
         var resultUser = HttpContext.Session.GetString("UpdatedResultOfCList");
         List<CabUser>userlist = JsonConvert.DeserializeObject<List<CabUser>>(resultUser);

         var userid = HttpContext.Session.GetInt32("CurrentUser");

         int CabUserId = (int)userid;
         userlist.ForEach(x=>Console.WriteLine(x.Uname+"------------------------->>"));
         Console.WriteLine(CabUserId+"------------------------------------>");
         foreach(CabUser cu in userlist){
            if(cu.Id==CabUserId){
                Console.WriteLine(cu.Uname+"------------------------------------>");
                ViewData["userName"]=cu.Uname;
            }
         }

        using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await client.GetAsync("FetchCabDetails");
            HttpResponseMessage getData2 = await client.GetAsync("FetchDiverDetails");
             
             if(getData.IsSuccessStatusCode && getData2.IsSuccessStatusCode){
                //string result = getData.Content.ReadAsStringAsync().Result;
                HttpContext.Session.SetString("ResultOfCab", getData.Content.ReadAsStringAsync().Result);
                HttpContext.Session.SetString("ResultofDriver",getData2.Content.ReadAsStringAsync().Result);
               
                var resultCab = HttpContext.Session.GetString("ResultOfCab");
                List<Cab>cablist = JsonConvert.DeserializeObject<List<Cab>>(resultCab);
                ViewData["Cab"]= cablist;

                var resultDriver = HttpContext.Session.GetString("ResultofDriver");
                 List<CabDriver>dlist = JsonConvert.DeserializeObject<List<CabDriver>>(resultDriver);
                 ViewData["DriverList"] = dlist;
             }
         }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult>  CabOrderIndex(int Cabobjid,int driverobjid){
        Cab Cabobj = new Cab();
        CabDriver driverobj = new CabDriver();
       
        var resultCab = HttpContext.Session.GetString("ResultOfCab");
        List<Cab>cablist = JsonConvert.DeserializeObject<List<Cab>>(resultCab);
        foreach(Cab cb in cablist){
            if(Cabobjid==cb.Id){
               Cabobj.Id = cb.Id;
               Cabobj.Model = cb.Model;
               Cabobj.RegistrationNo = cb.RegistrationNo;
               Cabobj.Colour = cb.Colour;

            }
        }

         var resultDriver = HttpContext.Session.GetString("ResultofDriver");
         List<CabDriver>dlist = JsonConvert.DeserializeObject<List<CabDriver>>(resultDriver);
         foreach(CabDriver cd in dlist){
            if(driverobjid==cd.Id){
               driverobj.Id=cd.Id;
               driverobj.DName=cd.DName;
               driverobj.DEmail=cd.DEmail;
               driverobj.DPhoneNo=cd.DPhoneNo; 
            }
         }


        var userid = HttpContext.Session.GetInt32("CurrentUser");
        int CabUserId = (int)userid;
        BookedCab rembook = new BookedCab();
             rembook.Id = CabUserId;
             rembook.Model = Cabobj.Model;
             Console.WriteLine(Cabobj.Model+"-------------------->");
             rembook.RegistrationNo = Cabobj.RegistrationNo;
             rembook.Colour = Cabobj.Colour;
             rembook.DName = driverobj.DName;
             rembook.DEmail = driverobj.DEmail;
             rembook.DPhoneNo = driverobj.DPhoneNo;
         using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020/"+"AddBooking/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //sending value to another application in key value format
            
            HttpResponseMessage getData =await client.PostAsJsonAsync<BookedCab>(CabUserId+"",rembook);
            
        
            if(getData.IsSuccessStatusCode){
                HttpContext.Session.SetInt32("CurrentBooking",rembook.Id);                 
            }
            
        } 
         using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage getData = await client.GetAsync("FetchBookedDetails");
             
             if(getData.IsSuccessStatusCode){                
                HttpContext.Session.SetString("CurrentBooking", getData.Content.ReadAsStringAsync().Result);
                return RedirectToAction("CabBookedIndex","Home");
             }
         }    
       return View(); 
    }

    [HttpGet]
    public IActionResult CabBookedIndex(){
        var resultUser = HttpContext.Session.GetString("CurrentBooking");
        List<BookedCab>Bookedlist = JsonConvert.DeserializeObject<List<BookedCab>>(resultUser);
       // ViewData["Booker"] = Bookedlist;

         var resultUser1 = HttpContext.Session.GetString("UpdatedResultOfCList");
         List<CabUser>userlist = JsonConvert.DeserializeObject<List<CabUser>>(resultUser1);

         var userid = HttpContext.Session.GetInt32("CurrentUser");
         int CabUserId = (int)userid;
         foreach(BookedCab bc in Bookedlist){
            if(CabUserId==bc.Id){
                ViewData["Booker"] = bc;
            }
         }
         userlist.ForEach(x=>Console.WriteLine(x.Uname+"------------------------->>"));
         Console.WriteLine(CabUserId+"------------------------------------>");
         foreach(CabUser cu in userlist){
            if(cu.Id==CabUserId){
                Console.WriteLine(cu.Uname+"------------------------------------>");
                ViewData["userName"]=cu.Uname;
            }
         }
        return View();
    }
    [HttpPost]
    public IActionResult CabBookedIndex(int ConfirmId){
        
        TempData["BookingMessage"] = "Thankyou For Booking Your Cab is on The Way";   
        return RedirectToAction("Index","Home");
    }
    [HttpGet]
    public async Task<IActionResult> UpdateCabBooking(int Bookid){
      using(var client = new HttpClient()){
            client.BaseAddress= new Uri("http://localhost:5020/"+"RemoveBooking/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //sending value to another application in key value format
    
           HttpResponseMessage getData =await client.DeleteAsync(Bookid+"");
       
        if(getData.IsSuccessStatusCode){
            TempData["message"]="Please Update Your Booking";
            return RedirectToAction("CabOrderIndex","Home");
        }
       
      } 
      return View();
    }


   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
