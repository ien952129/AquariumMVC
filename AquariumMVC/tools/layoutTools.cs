using static System.Net.Mime.MediaTypeNames;
using AquariumMVC.DTO;
using AquariumMVC.Models;

namespace AquariumMVC.tools
{
    public class layoutTools
    {
        
        public static string check_log_in(string User,string admin)
        {
            if (User != null&& admin== "adminAcc")
            {
                return "admin.cshtml";
                
            }
            else if(User != null && admin != "adminAcc")
            {
                return "_Layout_member.cshtml";
                
            }
            else
            {
                return "_Layout.cshtml";
            }
        }
        public static string produce_Pid(string type)
        {
            using (var db = new aquariumwebsiteContext())
            {
                if (type == "設備")
                {
                    var pids = db.Device.Select(d => d.P_id.Substring(1)).ToList(); // 先获取ID列表
                    var maxPid = pids.Where(pid => int.TryParse(pid, out _)) // 在内存中进行处理
                                     .Select(pid => int.Parse(pid))
                                     .OrderByDescending(pid => pid)
                                     .FirstOrDefault();

                    var newPid = "D" + (maxPid + 1).ToString("D4"); // 生成新的 P_id
                    return newPid;
                    
                    
                }else if(type == "活體")
                {
                    var pids = db.Fish.Select(d => d.P_id.Substring(1)).ToList(); // 先获取ID列表
                    var maxPid = pids.Where(pid => int.TryParse(pid, out _)) // 在内存中进行处理
                                     .Select(pid => int.Parse(pid))
                                     .OrderByDescending(pid => pid)
                                     .FirstOrDefault();
                    var newPid = "L" + (maxPid + 1).ToString("D4"); // 生成新的 P_id
                    return newPid;
                }
                else
                {
                    var pids = db.Feeds.Select(d => d.P_id.Substring(1)).ToList(); // 先获取ID列表
                    var maxPid = pids.Where(pid => int.TryParse(pid, out _)) // 在内存中进行处理
                                     .Select(pid => int.Parse(pid))
                                     .OrderByDescending(pid => pid)
                                     .FirstOrDefault();
                    var newPid = "F" + (maxPid + 1).ToString("D4"); // 生成新的 P_id
                    return newPid;
                }
            }
        }


        public static string AddProduct(string type, string pid,addproduct model)
        {
            try{
                using (var db = new aquariumwebsiteContext())
                {
                    // 根据不同类型，将数据添加到不同的表中
                    if (type == "設備")
                    {
                        var newDevice = new Device
                        {
                            P_id = pid,
                            // 假设 Device 表有与 AddProductModel 相似的字段
                            Name = model.Name,
                            Kind = model.Kind,
                            Size = model.Size,
                            Price = model.Price,
                            Amount = model.Amount,
                            Memo = model.Memo,
                            Img = model.Img
                            ,
                            Type = type
                        };
                        db.Device.Add(newDevice);

                    }
                    else if (type == "活體")
                    {
                        var newFish = new Fish
                        {
                            P_id = pid,
                            // 假设 Device 表有与 AddProductModel 相似的字段
                            Name = model.Name,
                            Kind = model.Kind,
                            Size = model.Size,
                            Price = model.Price,
                            Amount = model.Amount,
                            Memo = model.Memo,
                            Img = model.Img
                            ,
                            Type = type
                        };
                        db.Fish.Add(newFish);
                    }
                    else if (type == "飼料")
                    {
                        var newFeed = new Feeds
                        {
                            P_id = pid,
                            // 假设 Device 表有与 AddProductModel 相似的字段
                            Name = model.Name,
                            Kind = model.Kind,
                            Size = model.Size,
                            Price = model.Price,
                            Amount = model.Amount,
                            Memo = model.Memo,
                            Img = model.Img,
                            Type = type
                        };
                        db.Feeds.Add(newFeed);
                    }

                    // 将改动保存到数据库
                    db.SaveChanges();
                    return "新增成功";
                }
            }
            catch(Exception ex)
            {
                return ex.ToString() + "，新增失敗";
            }
        }



    }
}

