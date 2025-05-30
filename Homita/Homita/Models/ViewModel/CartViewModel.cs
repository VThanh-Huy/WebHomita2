using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homita.Models.ViewModel
{
    public class CartViewModel
    {
        private TRA_SUAEntities data = new TRA_SUAEntities();

        public string MaCTGH { get; set; }
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

        public CartViewModel() { }

        public CartViewModel(ChiTietGioHang ct)
        {
            MaCTGH = ct.MaCTGH;
            MaSP = ct.MaSP;
            SoLuong = ct.SoLuong ?? 0;
            TenSP = ct.SanPham.TenSP;
            Gia = (double)ct.SanPham.Gia.GetValueOrDefault();
            HinhAnh = ct.SanPham.HinhAnh;
        }

        public CartViewModel(string id)
        {
            MaSP = id;
            SanPham sanPham = data.SanPham.Single(n => n.MaSP == id);
            TenSP = sanPham.TenSP;
            HinhAnh = sanPham.HinhAnh;
            Gia = (double)sanPham.Gia.GetValueOrDefault();
            SoLuong = 1;
            MaLoaiSP = sanPham.MaLoaiSP;
        }

        // Constructor thêm cho giỏ tạm
        //public CartViewModel(string maSP, string tenSP, string hinhAnh, int gia, int soLuong)
        //{
        //    MaSP = maSP;
        //    TenSP = tenSP;
        //    HinhAnh = hinhAnh;
        //    Gia = gia;
        //    SoLuong = soLuong;
        //}
    }

}