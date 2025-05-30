using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homita.Models.ViewModel
{
    public class CartViewModel
    {
        private TRA_SUAEntities data = new TRA_SUAEntities();

        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public double Gia { get; set; }
        public string Size { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }    
        public string MaLoaiSP { get; set; }

        public List<SanPham> SanPhams { get; set; }

        public Double ThanhTien
        {
            get { return SoLuong * Gia; }
        }

        public CartViewModel(string id)
        {
            MaSP = id;
            SanPham sanPham = data.SanPham.Single(n =>n.MaSP ==id);
            TenSP = sanPham.TenSP;
            HinhAnh = sanPham.HinhAnh;
            Gia = int.Parse(Math.Truncate(sanPham.Gia ?? 0m).ToString());
            SoLuong = 1;
            MaLoaiSP = sanPham.MaLoaiSP;
        }

    }
}   