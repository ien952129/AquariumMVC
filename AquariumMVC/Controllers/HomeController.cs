
using AquariumMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AquariumMVC.tools;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.AspNetCore.Http;

namespace AquariumMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        aquariumwebsiteContext db=new aquariumwebsiteContext();
       

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string admin = HttpContext.Session.GetString("admin");
            var test = db.all_Product.ToList();
            string User = HttpContext.Session.GetString("acc");
            string layout=tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout=layout;
            return View(test);
        }

        public IActionResult test()
        {
            string P_id = "L0001";
            var details = db.AllDetailResults
            .FromSqlRaw("EXEC alldetail @P_id", new SqlParameter("@P_id", P_id)).IgnoreQueryFilters()
            .ToList();
            
            return View(details);
        }

        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Account,string Password)
        {
            var result = db.Memberdata
                .Where(m => m.Account == Account && m.Password == Password).SingleOrDefault();
            if (result == null)
            {
                ViewBag.msg= "帳密錯誤，登入失敗";
                return View();
            }
            else if(result.IsAdmin==true)
            {
                HttpContext.Session.SetString("acc", result.Account);
                HttpContext.Session.SetString("admin", "adminAcc");
                return RedirectToAction("Index","Back");
            }
            else
            {
                HttpContext.Session.SetString("acc", result.Account);
                return RedirectToAction("Index");
            }
            
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Memberdata m)
        {
            if (ModelState.IsValid)
            {
                var result = (from a in db.Memberdata
                              where m.Account == a.Account
                              select a).FirstOrDefault();
                if (result == null)
                {
                    db.Memberdata.Add(m);
                    db.SaveChanges();
                    ViewBag.msg = "註冊成功";
                    return View();
                }
                else
                {
                    ViewBag.msg = "此帳號已有人註冊";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();    //清除Session變數資料
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult ProductDetail(string fPId)
        {
            string admin = HttpContext.Session.GetString("admin");
            string User = HttpContext.Session.GetString("acc");
            string layout = tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout = layout;
            var details = db.AllDetailResults
             .FromSqlRaw("EXEC alldetail @P_id", new SqlParameter("@P_id", fPId)).IgnoreQueryFilters()
             .ToList();
            //var details = db.AllDetailResults
            //.FromSqlRaw("EXEC alldetail @P_id", new SqlParameter("@P_id", fPId))
            //.AsEnumerable()  // 在客户端进行进一步处理
            //.FirstOrDefault();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                return View(details);
            }
            else { return View(details); }
            
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        public IActionResult product_group(string type)
        {
            string admin = HttpContext.Session.GetString("admin");
            string User = HttpContext.Session.GetString("acc");
            string layout = tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout = layout;
            
            var result = db.all_Product.Where(m => m.type == type).ToList();
            ViewBag.type = type;
            return View(result);
        }

        public IActionResult product_live(string name)
        {
            string admin = HttpContext.Session.GetString("admin");
            string User = HttpContext.Session.GetString("acc");
            string layout = tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout = layout;
            var result = db.Fish
                   .Where(m => m.Kind == name)
                   .GroupBy(fish => fish.P_id)
                   .Select(group => group.First())
                   .ToList();
            ViewBag.name = name;
            return View(result);
        }
        public IActionResult product_devic(string name)
        {
            string admin = HttpContext.Session.GetString("admin");
            string User = HttpContext.Session.GetString("acc");
            string layout = tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout = layout;
            var result = db.Device.Where(m => m.Kind == name).GroupBy(fish => fish.P_id)
                   .Select(group => group.First())
                   .ToList();
            ViewBag.name = name;
            return View(result);
        }
        public IActionResult product_feeds(string name)
        {
            string admin = HttpContext.Session.GetString("admin");
            string User = HttpContext.Session.GetString("acc");
            string layout = tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout = layout;
            var result = db.Feeds.Where(m => m.Kind == name).GroupBy(fish => fish.P_id).Select(group => group.First()).ToList();
            ViewBag.name = name;
            return View(result);
        }
        [HttpGet]
        public IActionResult addShoppingCar(string pid, int aid,int amount)
        {
            string admin = HttpContext.Session.GetString("admin");
            string User = HttpContext.Session.GetString("acc");
            string layout = tools.layoutTools.check_log_in(User,admin);
            ViewBag.layout = layout;
            if (User == null)
            {
                TempData["Message"] = "請先登入";
                return RedirectToAction("ProductDetail", new { fPId = pid });
            }
            else
            {
                var p = (from a in db.OrderDetail where a.A_id == aid && a.P_id == pid && a.IsApproved == false select a).FirstOrDefault();

                var order = db.Size_priceResults
                .FromSqlRaw("EXEC Size_price @P_id, @A_id",
                        new SqlParameter("@P_id", pid),
                        new SqlParameter("@A_id", aid)).ToList();
                if (p == null)
                {
                    try
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.Account = User;
                        orderDetail.A_id = aid;
                        orderDetail.P_id = pid;
                        orderDetail.Name = order[0].Name;
                        orderDetail.Price = order[0].Price;
                        orderDetail.Qty = amount;
                        orderDetail.IsApproved = false;
                        db.OrderDetail.Add(orderDetail);
                        db.SaveChanges();
                        TempData["Message"] = "成功加入購物車";
                        return RedirectToAction("ProductDetail", new { fPId = pid });
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = "加入失敗";
                        return RedirectToAction("ProductDetail", new { fPId = pid });
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    p.Qty = p.Qty + amount;
                    db.SaveChanges();
                    TempData["Message"] = "成功加入購物車";
                    return RedirectToAction("ProductDetail", new { fPId = pid });
                }
            }
        }
        public IActionResult Shopping_Car()
        {
            string User = HttpContext.Session.GetString("acc");
            
            var res=(from a in db.OrderDetail where a.Account==User &&a.IsApproved==false select a).ToList();
            return View(res);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var item = db.OrderDetail.Find(id);
            if (item != null)
            {
                item.Qty = quantity;
                db.SaveChanges();
            }

            return Json(new { success = true });
        }

        public IActionResult deleteProduct(int fId)
        {
            var item = db.OrderDetail.Find(fId);
            if (item != null)
            {
                db.OrderDetail.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Shopping_Car");
            }
            else
            {
                return RedirectToAction("Shopping_Car");
            }
        }
        [HttpGet,HttpPost]
        public IActionResult goCheck(string total,string Receiver,string ReceiverTel,string Address)
        {
            if (Request.Method == HttpMethod.Get.Method)
            {
                var sessionTotal = HttpContext.Session.GetObjectFromJson<string>("Total");
                if (string.IsNullOrWhiteSpace(sessionTotal))
                {
                    // 如果 Session 中没有 total，则使用传递的 total 值
                    ViewBag.Total = total;
                    HttpContext.Session.SetObjectAsJson("Total", total);
                }
                else
                {
                    // 使用 Session 中保存的 total 值
                    ViewBag.Total = sessionTotal;
                }
                var order = HttpContext.Session.GetObjectFromJson<Order>("OrderData");
                if (order != null)
                {
                    // 将订单信息传递给视图
                    return View(order);
                }
                else
                {
                    // GET 请求，首次访问
                    ViewBag.Total = total; // 将 total 传递给视图
                    return View(new Order { total_price = (int)Math.Round(decimal.Parse(total)) });
                }
            }
            else
            {
                string User = HttpContext.Session.GetString("acc");
                string guid = Guid.NewGuid().ToString("D");
                var totalInt = (int)Math.Round(decimal.Parse(total));
                var order = new Order() { Account = User, Receiver = Receiver, ReceiverTel = ReceiverTel, Address = Address, Date = DateTime.Now, total_price = totalInt };
                HttpContext.Session.SetString("OrderGuid", guid);
                HttpContext.Session.SetObjectAsJson("OrderData", order);

                
                return View("confirm", order);
            }

            
        }
        [HttpPost,HttpGet]
        public IActionResult confirm(Order o)
        {
            if (TempData["mes"] != null)
            {
                ViewBag.mes = TempData["mes"].ToString();
                return View(o);
            }
            else { return View(o); }
            
        }
        
        [HttpPost]
        public IActionResult ConfirmOrder(Order model)
        {
            string guid = HttpContext.Session.GetString("OrderGuid");
            try
            {
                Order order = new Order
                {
                    OrderGuid = guid,
                    Account = HttpContext.Session.GetString("acc"),
                    Receiver = model.Receiver,
                    ReceiverTel = model.ReceiverTel,
                    Address = model.Address,
                    Date = model.Date,
                    total_price = model.total_price
                };

                // 保存到数据库
                db.Order.Add(order);
                var res = from a in db.OrderDetail where a.IsApproved == false && a.OrderGuid == null select a;
                foreach (var b in res)
                {
                    b.OrderGuid = guid;
                    b.IsApproved = true;
                }
                db.SaveChanges();
                TempData["mes"] = "購買成功";
                HttpContext.Session.Remove("OrderGuid");
                return RedirectToAction("confirm", order); 
            }
            catch (Exception ex)
            {
                TempData["mes"] = "購買失敗";
                return RedirectToAction("goCheck"); 
            }
        }

        public IActionResult Order()
        {
            string User = HttpContext.Session.GetString("acc");
            var res=(from a in db.Order where a.Account==User select a).ToList();
            return View(res);
        }
        public IActionResult order_content(string guid)
        {
            var order = (db.OrderDetail.Where(m=>m.OrderGuid==guid)).ToList();
            var prices = db.Order.Where(m => m.OrderGuid == guid).Select(m => m.total_price).First();
            ViewBag.total_price = prices;
            return View(order);
        }
    }
}
