using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homita.Models.ViewModel
{
    public class DetailViewModel
    {
        public TRA_SUAEntities data = new TRA_SUAEntities();

        public SanPham SanPham { get; set; }

        public IEnumerable<SanPham> SanPhamCungLoai { get; set; }
        public IEnumerable<SanPham> HinhAnhs {  get; set; }
    }
}