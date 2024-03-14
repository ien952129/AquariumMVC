using AquariumMVC.DTO;
using AquariumMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AquariumMVC.tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing.Printing;
using AquariumMVC.functionService;
using AquariumMVC.Interface;
using System.Security.Cryptography;

namespace AquariumMVC.Controllers
{
    public class BackController : Controller
    {


        private readonly ILogger<BackController> _logger;
        aquariumwebsiteContext db = new aquariumwebsiteContext();
        private readonly memberInterface mf;
        private readonly all_ProductInterface apf;
        private readonly DeviceInterface df;
        private readonly FishInterface fhf;
        private readonly FeedsInterface fdf;
        private readonly orderInterface of;
        private readonly orderdetailInterface odf;
        private readonly addProductInterface addp;
        public BackController(ILogger<BackController> logger, memberInterface mf, all_ProductInterface apf,DeviceInterface df, FishInterface fhf, FeedsInterface fdf, orderInterface of, orderdetailInterface odf, addProductInterface addp)
        {
            _logger = logger;
            this.mf = mf;
            this.apf = apf;
            this.df = df;
            this.fhf = fhf;
            this.fdf = fdf;
            this.of = of;
            this.odf = odf;
            this.addp = addp;
        }
        public IActionResult Index()
        {
            //var memberdata = (from a in db.Memberdata select a).ToList();
            var memberdata=mf.GetMembers();
            return View(memberdata);
        }
        public IActionResult addProduct()
        {
            //var types = db.all_Product.Select(p => p.type).Distinct().ToList();
            var types = apf.getType();
            ViewBag.Types = new SelectList(types); // 这里创建了 SelectList 对象
            return View();


        }
        [HttpPost]
        public async Task<IActionResult> addProduct(addproduct model)
        {
            string msg = "";
            var product_exist = apf.product_exist(model.Name);
            //var product_exist = db.all_Product.Where(a => a.Name == model.Name).FirstOrDefault();
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                // 将文件名保存到 model.Img，如果需要
                model.Img = fileName;
            }


            if (product_exist != null)
            {

                msg = tools.layoutTools.AddProduct(model.Type, product_exist.P_id, model);
                ViewBag.insert = msg;
                var types= apf.getType();
               
                ViewBag.Types = new SelectList(types);

                return View();
            }
            else
            {
                string newpid=apf.addproduct_returnPID(model);
                
                msg = tools.layoutTools.AddProduct(model.Type, newpid, model);
                ViewBag.insert = msg;
                var types = apf.getType();
                ViewBag.Types = new SelectList(types);

                return View();
            }



        }
        [HttpGet]
        public IActionResult GetKinds(string type)
        {
            List<string> kinds = new List<string>();

            switch (type)
            {
                case "設備":
                    kinds = df.getKind();
                    break;
                case "活體":
                    kinds = fhf.getKind();
                    break;
                case "飼料":
                    kinds = fdf.getKind();
                    break;
            }

            return Json(kinds);
        }

        public IActionResult Order()
        {
            var order = of.getALLOder();
            return View(order);
        }

        //訂單修改
        public IActionResult Details_order(string guid)
        {
            var order = odf.getOderDetail(guid);
            var prices = of.getOrderPrice(guid);
            ViewBag.total_price = prices;
            return View(order);
        }

        public IActionResult delete_Order(int id)
        {
            
            string msg=of.removeOrder(id);
            
            ViewBag.msg = msg;
            return RedirectToAction("Order", "Back");
            
        }



        public async Task<IActionResult> edit_Order(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await of.GetOrderByAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit_Order(int id, [Bind("OrderGuid,Account,Receiver,ReceiverTel,Address,Date,total_price")] orderDTO orderData)
        {
            string result = await of.UpdateOrderAsync(id, orderData);
            ViewBag.msg = result;

            if (result == "Update successful")
            {
                return View(orderData);
            }
            else
            {
                return View(orderData);
            }
        }


        //會員系統處理
        public IActionResult addMember()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addMember(memberDTO m)
        {
            if (ModelState.IsValid)
            {
                
                    string msg =mf.addmember(m);
                    ViewBag.msg = msg;
                    return View(m);
                
            }
            return View(m);
        }

        public IActionResult delete_Member(string id)
        {
            var remove = mf.delete_Member(id);
               
            ViewBag.msg = remove;
            return RedirectToAction("Index", "Back");
            
        }



        public async Task<IActionResult> edit_member(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await mf.GetMemberByAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit_member(string id, [Bind("Account,Password,Name,Email,Address,IsAdmin")] memberDTO member)
        {
            string result = await mf.UpdateMemberAsync(id, member);
            ViewBag.msg = result;

            if (result == "Update successful")
            {
                return View(member);
            }
            else
            {
                return View(member);
            }
            
        }

        




        //產品修改
        public IActionResult editProduct()
        {
            var types = apf.GetAllProducts();
            if (TempData["mes"] != null)
            {
                ViewBag.msg = TempData["mes"];
            }
            return View(types);

        }



        public IActionResult editProductDetail(string fPId)
        {
            var details = db.AllDetailResults
             .FromSqlRaw("EXEC alldetail @P_id", new SqlParameter("@P_id", fPId)).IgnoreQueryFilters()
             .ToList();
            return View(details);


        }

        public async Task<IActionResult> editProductDetailData(string fPId, int aid)
        {
            if (aid == null && fPId == null)
            {
                return NotFound();
            }

            char firstLetter = fPId[0];

            if (firstLetter == 'D')
            {
                var product = await df.getDevice(fPId, aid);
                //var product = await db.Device.Where(a => a.P_id == fPId && a.D_id == aid).SingleOrDefaultAsync();
                if (product == null)
                {
                    return NotFound();
                }
                alldetail a = df.DeviceDTOtoalldetail(product);
                return View(a);
            }
            else if (firstLetter == 'L')
            {
                var product = await fhf.getFish(fPId, aid);

                if (product == null)
                {
                    return NotFound();
                }
                alldetail a = fhf.FishDTOtoalldetail(product);
                return View(a);

            }
            else if (firstLetter == 'F')
            {
                var product = await fdf.getFeeds(fPId, aid);
                if (product == null)
                {
                    return NotFound();
                }
                alldetail a = fdf.FeedsDTOtoalldetail(product);
                return View(a);

            }
            else { return NotFound(); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editProductDetailData([Bind("A_id,P_id,Size,Name,Price,Amount,Memo,Kind,Type,Img")] alldetail d)
        {
            if (ModelState.IsValid)
            {
                string existingImage = null;

                // 检查是否有新图片上传
                var file = HttpContext.Request.Form.Files.FirstOrDefault();
                if (file != null && file.Length > 0)
                {
                    // 如果有新图片，保存它
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    d.Img = fileName; // 更新 Img 属性为新文件名
                }
                else
                {
                    var or_imgs = db.Size_priceResults
                    .FromSqlRaw("EXEC Size_price @P_id, @A_id",
                        new SqlParameter("@P_id", d.P_id),
                        new SqlParameter("@A_id", d.A_id))
                    .IgnoreQueryFilters()
                    .ToList();

                    var or_img = or_imgs.FirstOrDefault();
                    d.Img = or_img.Img;
                    
                }

                char firstLetter = d.P_id[0];

                if (firstLetter == 'D')
                {

                    
                    string msg = await addp.editDevice(d.P_id, d.A_id, d);

                    ViewBag.msg = msg;
                    

                }
                else if (firstLetter == 'L')
                {

                   
                    string msg = await addp.editFish(d.P_id, d.A_id, d);
                    ViewBag.msg = msg;
                    

                }
                else if (firstLetter == 'F')
                {

                    string msg = await addp.editFeeds(d.P_id, d.A_id, d);
                    ViewBag.msg = msg;
                    

                }
                else
                {
                    ViewBag.msg = "修改失敗";
                }
                string msg1 = await apf.Updateall_Product(d);
                ViewBag.msg1=msg1;

                return View(d);
                



            }
            ViewBag.msg = "修改失敗";
            return View(d);
            
        }

        

        public IActionResult delete_Product(string pid,int? aid)
        {
            char firstLetter = pid[0];
            bool productRemoved = false;
            bool success=true;
            string err = "";
            OperationResult a=new OperationResult();
            if (firstLetter == 'D')
            {

                a = df.Getdevuce_sync_And_remove(pid, aid);
                productRemoved = a.Type;
                success = a.Success;
                err = a.Message;
                
            }
            else if (firstLetter == 'L')
            {
                a = fhf.Getfish_sync_And_remove(pid, aid);
                productRemoved = a.Type;
                success = a.Success;
                err = a.Message;

            }
            else 
            {
                a = fdf.Getfeeds_sync_And_remove(pid, aid);
                productRemoved = a.Type;
                success = a.Success;
                err = a.Message;
            }
            
            if (productRemoved==false) 
            {
                var allcontent = apf.removeProduct(pid);
                
            }
            TempData["mes"] = err;


            if (productRemoved)
            {
            // 检查 pid 下是否还有其他商品
                bool hasOtherProducts = apf.hasOtherProducts(pid);
                _logger.LogInformation($"Has other products: {hasOtherProducts}");
                if (!hasOtherProducts)
                {
                    var allProduct=apf.removeProduct(pid);
                    
                }
            }
            db.SaveChanges();
            return RedirectToAction("editProduct");
        }

    }
}
