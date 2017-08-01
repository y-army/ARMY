using CommSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommSite.DAL
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    public class UserService
    {
        ///获取数据模型名称
        public static DbFactory entity = ContextFactory.GetCurrentContext();
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <returns></returns>
        public static string AdminLogin(string UserName, string UserPassword)
        {
            string mes = "";
            T_User user = entity.T_User.Where(a=>a.UserName==UserName).FirstOrDefault();
            if (user==null)
            {
                return "yong";
            }
            T_User user1 = entity.T_User.Where(a=>a.UserPassword==UserPassword).FirstOrDefault();
            if (user1 == null)
            {
                return "mi";
            }
            T_User user2 = entity.T_User.Where(a=>a.UserName==UserName && a.UserPassword==UserPassword).FirstOrDefault(); ;
            if (user2 == null)
            {
                mes = "bu";
            }
            else if (user2.S != "启用")
            {
                mes = "jin";
            }else if (user2.UserName == UserName && user2.UserPassword == UserPassword)
            {
                mes = "ok";
            }
            else
            {
                mes = "error";
            }
            return mes;
        }
        /// <summary>
        /// 查询管理员信息
        /// </summary>
        /// <returns></returns>
        public static List<T_User> userList()
        {
            List<T_User> list = (from a in entity.T_User select a).ToList();
            return list;
        }
        /// <summary>
        /// 根据管理员名称查询管理员信息
        /// </summary>
        /// <returns></returns>
        public static T_User userGenr(string UserName)
        {
            T_User user = (from u in entity.T_User where u.UserName == UserName select u).SingleOrDefault();
            return user;
        }
        /// <summary>
        /// 添加管理员信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int userAdd(T_User user)
        {
            ///查询是否有相同管理员
            T_User suser = (from u in entity.T_User where u.UserName == user.UserName select u).SingleOrDefault();
            if (suser != null)
            {
                return 2;
            }
            else
            {
                entity.T_User.Add(
                        new T_User()
                        {
                            AutoID = user.AutoID,
                            UserName = user.UserName,
                            UserPassword = user.UserPassword,
                        });
                if (entity.SaveChanges() > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }
        /// <summary>
        /// 根据管理员名称删除管理员信息
        /// </summary>
        /// <param name="UserName">管理员名称</param>
        /// <returns></returns>
        public static int userDel(Guid AutoId)
        {
            T_User user = entity.T_User.FirstOrDefault(i => i.AutoID == AutoId);
            if (user != null)
            {
                entity.T_User.Remove(user);
            }

            if (entity.SaveChanges() > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 修改管理员信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static int UserUpdate(T_User user, string UserName)
        {
            //查询要修改的数据
            T_User users = entity.T_User.Where(a => a.AutoID == user.AutoID).FirstOrDefault();
            if (users.UserPassword == user.UserPassword && users.UserName==user.UserName && users.S ==user.S)
            {
                return 2;
            }
            //设置要修改的值
            users.UserName = user.UserName;
            users.UserPassword = user.UserPassword;
        
            //更新到数据库
            int result = entity.SaveChanges();
            if (result == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 禁用启用
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public static int AdminUse(Guid AutoId)
        {
            //查询要修改的数据
            T_User users = entity.T_User.Where(a => a.AutoID == AutoId).FirstOrDefault();
            if (users.S == "启用")
            {
                users.S = "禁用";
                //更新到数据库
                int result = entity.SaveChanges();
                if (result == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (users.S == "禁用")
            {
                users.S = "启用";
                //更新到数据库
                int result = entity.SaveChanges();
                if (result == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 2;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DelAdminAll(string id)
        {
            string msg = "";
            BaseRepository<T_User> server = new BaseRepository<T_User>();
            if (id.Length == 36)
            {
                Guid autoid = new Guid(id);
                T_User users = entity.T_User.Where(a => a.AutoID == autoid).FirstOrDefault();
                bool result = server.EntityDelete(users);
                if (result)
                {

                    msg = "yes";
                }
                else
                {
                    msg = "no";
                }
            }
            else
            {
                id = id.Substring(0, id.Length - 1);
                String[] array = id.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    Guid guid = new Guid(array[i]);
                    T_User user = entity.T_User.Where(a => a.AutoID == guid).FirstOrDefault();
                    bool results = server.EntityDelete(user);
                    if (results)
                    {

                        msg = "yes";
                    }
                    else
                    {
                        msg = "no";
                    }
                }
            }
            return msg;
        }
    }
}
