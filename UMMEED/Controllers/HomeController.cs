using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UMMEED.Models;
using BOL;
using BLL;
namespace UMMEED.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {

        return View();
    }

    [HttpGet]//remaining
    public IActionResult ChildPage(){
        
        
        return View();
    }

    [HttpGet]
    public IActionResult ChildStatus(int chid){
         IOManage im = new IOManage();
          List<ChildSts>chlist = im.GetChild();
        foreach(ChildSts ch in chlist){
            if(chlist!=null){
                if(ch.Chid==chid){
                    ViewData["Child"]=ch;
                }
                
            }
        }
        
         return View();
    }
   
    [HttpGet]
    public IActionResult AdminIndex(){
      IOManage im = new IOManage();
      List<ChildSts> chlist = im.GetChild();
      if(chlist!=null){
          ViewData["Child"]=chlist;
      }  
      return View();
    }
    [HttpGet]
    public IActionResult LoginAdmin(string role){
        ViewData["AdminRole"]=role;
        return View();
    }
    [HttpGet]
     public IActionResult LoginChild(string role1){
        ViewData["ChildRole"]=role1;
        return View();
    }
    [HttpGet]
    public IActionResult Insert(){
        IOManage im = new IOManage();
        List<ChildSts>chlist = im.GetChild();
        foreach(ChildSts ch in chlist){
            if(chlist!=null){
                ViewData["Child"]=ch;
            }
        }
        
        return View();
    }
   
    [HttpGet]
    public IActionResult Delete(int chid){
        IOManage im=new IOManage();
        bool status=im.RemoveUser(chid);
        if(status){
            TempData["message"]="Removed SuccessFully";
            return RedirectToAction("AdminIndex","Home");
        }
        return View();
    }

    [HttpGet]
    public IActionResult Edit([FromQuery(Name="chid")] int chid){
        IOManage im = new IOManage();
        List<ChildSts>chlist = im.GetChild();
        foreach(ChildSts ch in chlist){
            if(chlist!=null){
                if(ch.Chid==chid){
                  ViewData["Child"]=ch;
                  break;
                }
                
            }
        }

        return View();
    } 
    [HttpPost]
    public IActionResult Edit(int chid,string payment,string date){
        IOManage im = new IOManage();
        Console.WriteLine(chid);
        bool status = im.Update(chid,payment,date);
        if(status){
            TempData["message"]="Updated Successfully";
            return RedirectToAction("AdminIndex","Home");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Insert(string password,string chname,string date,int chid){
        IOManage im = new IOManage();
        string role="child";
        string chkey = Guid.NewGuid().ToString();
        bool status = false;

        status = im.ChildInx(chname,date,password,role,chid,chkey);

        if(status){
            TempData["message"]="Registered SuccessFully";
           return RedirectToAction("AdminIndex","Home");
        }

        return View();

    }
    
    [HttpPost]
    public IActionResult LoginAdmin(string uname,string password,string role){
        IOManage im = new IOManage();
        List<User>ulist  = im.GetAll();
        foreach(User u in ulist){
            // if(u!=null){
                
            //     if(u.Uname.Equals(uname) && u.Password.Equals(password)){
            //         if(u.Role==role){
            //             if(role=="admin"){
            //                 return RedirectToAction("AdminIndex","Home");
            //             }
            //             if(role=="child"){
            //                 return RedirectToAction("ChildPage","Home");
            //             }
            //            TempData["message"]="Invalid Credentials";
            //            return RedirectToAction("Login","Home");
            //         }
            //         else if(u.Role=="child" && role=="admin"){                    
            //             TempData["message"]="sorry you are not admin";
            //             return RedirectToAction("Login","Home");
            //         }
            //     }
                
            // }

            if(u!=null){
                   Console.WriteLine(u.Role+"-"+role);
                if(u.Role.Equals(role)){     //remeber to put chid in 1st condition otherwise loop will break
                    Console.WriteLine(u.Uname+"-"+uname);
                   if(u.Uname.Equals(uname) && u.Password.Equals(password)){
                      return RedirectToAction("AdminIndex","Home");
                   }
                   TempData["message"]="Invalid Credentials";
                    return RedirectToAction("LoginAdmin","Home");
                }
                TempData["message"]="sorry you are not admin";
                return RedirectToAction("LoginAdmin","Home");
            }
        }
        TempData["message"]="Get Register First";
        return RedirectToAction("LoginAdmin","Home");

        return View();
    }
    [HttpPost]
    public IActionResult LoginChild(int uid,string uname,string password,string role1){
         IOManage im = new IOManage();
        List<User>ulist  = im.GetAll();
        foreach(User u in ulist){
            if(u!=null){
                 Console.WriteLine(u.Uid+"=="+uid);
                if(u.Uid==uid){
                    
                    Console.WriteLine(u.Role+"-"+role1);
                  if(u.Role.Equals(role1)){
                       
                     if(u.Uname.Equals(uname) && u.Password.Equals(password)){
                        TempData["uid"] = uid;
                         return RedirectToAction("ChildPage","Home");
                     }
                     TempData["message"]="sorry you are not registered as Child";
                    return RedirectToAction("LoginChild","Home");
                }
                TempData["message"]="invalid credentials";
                return RedirectToAction("LoginChild","Home");
                  }
            }

        }
            TempData["message"]="Get Register First";
        return RedirectToAction("LoginChild","Home");

        return View();
        }
       
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
