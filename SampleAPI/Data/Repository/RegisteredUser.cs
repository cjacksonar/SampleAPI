using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class RegisteredUser : RepositoryBase, IRepository<Entities.RegisteredUser>
    {
        private string _productId;

        #region Constructors
        public RegisteredUser(string productId)
        {
            if (_productId == null) { _productId = productId; }
        }
        #endregion

        public void Add(ref Entities.RegisteredUser item)
        {
            try
            {
                base.Context.RegisteredUsers.Add(item);
                base.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
            }
        }
        public void Delete(Entities.RegisteredUser item)
        {
            try
            {
                var entity = base.Context.RegisteredUsers.Find(item.Id);
                if (entity == null) { return; }
                base.Context.Entry(entity).State = EntityState.Deleted;
                base.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
            }
        }
        public List<Entities.RegisteredUser> GetList()
        {
            try
            {
                return base.Context.RegisteredUsers.Where(x => x.ProductId == _productId && x.AllowEditing == true).ToList();
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
                return null;
            }
        }
        public void Save(Entities.RegisteredUser item)
        {
            try
            {
                var entity = base.Context.RegisteredUsers.Find(item.Id);
                if (entity == null) { return; }
                base.Context.Entry(entity).CurrentValues.SetValues(item);
                base.Context.Entry(entity).State = EntityState.Modified;
                base.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
            }
        }
        public bool IsValidUser(string userId, string userPassword, ref int userRoleId, ref string userRole, ref string OrganizationName,
                                ref int userIdKey, ref string userName, ref string userEmailAddress)
        {
            try
            {
                DataTable dt = new DataTable();
                var sql = string.Format("SELECT Id, OrganizationName, UserRoleId, RoleDescription, UserName, UserEmail FROM vwRegisteredUser WHERE ProductId='{0}' AND UserId='{1}' AND UserPassword = '{2}'", _productId, userId, userPassword);
                dt = base.GetDataTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    userIdKey = Convert.ToInt16(dt.Rows[0]["Id"].ToString());
                    userRoleId = Convert.ToInt16(dt.Rows[0]["UserRoleId"].ToString());
                    userName = dt.Rows[0]["UserName"].ToString();
                    userEmailAddress = dt.Rows[0]["UserEmail"].ToString();
                    OrganizationName = dt.Rows[0]["OrganizationName"].ToString();
                    userRole = dt.Rows[0]["RoleDescription"].ToString();
                    SaveUserLogin(userId);  //  if valid registered user update number of logins and last login date
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
                return false;
            }
        }
        public int GetUserRoleId(string userId, string userPassword)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = base.GetDataTable(string.Format("SELECT Id, UserRoleId FROM RegisteredUser WHERE ProductId='{0}' AND UserId='{1}' AND UserPassword = '{2}'", _productId, userId, userPassword));
                if (dt.Rows.Count == 0)
                {
                    return -1;
                }
                else
                {
                    var entity = base.Context.RegisteredUsers.Find(Convert.ToInt16(dt.Rows[0]["Id"]));
                    if (entity == null) { return -1; }
                    return Convert.ToInt16(dt.Rows[0]["UserRoleId"]);
                }
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
                return -1;
            }
        }
        public string GetUserOrganizationName(string productId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = base.GetDataTable(string.Format("SELECT OrganizationName FROM Registration WHERE ProductId='{0}'", productId));
                if (dt.Rows.Count == 0)
                {
                    return string.Empty;
                }
                else
                {

                    return dt.Rows[0]["OrganizationName"].ToString();
                }
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
                return string.Empty;
            }
        }
        public void SaveUserLogin(string userId)
        {
            try
            {

                DataTable dt = new DataTable();
                dt = base.GetDataTable(string.Format("SELECT Id, UserRoleId FROM RegisteredUser WHERE ProductId='{0}' AND UserId='{1}'", _productId, userId));
                if (dt.Rows.Count == 0) { return; }
                else
                {
                    var id = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                    var entity = base.Context.RegisteredUsers.Find(id);
                    if (entity == null) { return; }
                    entity.NumberOfLogins = entity.NumberOfLogins + 1;
                    entity.LastLogin = DateTime.Now;
                    base.Context.Entry(entity).CurrentValues.SetValues(entity);
                    base.Context.Entry(entity).State = EntityState.Modified;
                    base.Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.HasException = true;
                this.ClassException = ex;
                return;
            }
        }
    }
}

