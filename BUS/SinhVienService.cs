using DAL.Enitities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BUS
{
    public class SinhVienService
    {
        public List<SinhVien> GetAll()
        {
            using (Model1 context = new Model1())
            {
                return context.SinhViens
                              .Include(sv => sv.Lop) 
                              .ToList();
            }
        }
        

        public List<SinhVien> FindByName(string name)
        {
            using (Model1 context = new Model1())
            {
                return context.SinhViens
                              .Include(sv => sv.Lop) // Eager loading
                              .Where(sv => sv.HoTenSV.Contains(name))
                              .ToList();
            }
        }
       
        public void Insert(SinhVien sinhVien)
        {
            using (Model1 context = new Model1())
            {
                context.SinhViens.AddOrUpdate(sinhVien);
                context.SaveChanges();
            }
        }
        public void Delete(string maSV)
        {
            using (Model1 context = new Model1())
            {
                var sinhVien = context.SinhViens.FirstOrDefault(sv => sv.MaSV == maSV);
                if (sinhVien != null)
                {
                    context.SinhViens.Remove(sinhVien);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Sinh viên không tồn tại.");
                }
            }
        }
       

    }
}
