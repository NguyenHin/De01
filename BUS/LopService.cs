using DAL.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class LopService
    {
        public List<Lop> GetAll()
        {
            using (Model1 context = new Model1())
            {
                return context.Lops.ToList();
            }
        }

       
    }
}
