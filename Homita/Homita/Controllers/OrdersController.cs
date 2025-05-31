using Homita.Models.ViewModel;
using Homita.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Homita.Controllers
{
    public class OrdersController : Controller
    {
        private TRA_SUAEntities data = new TRA_SUAEntities();
        // GET: Cart

        public List<CartViewModel> GetCartView()
        {//lay ben cartview
            List<CartViewModel> lst = Session["GioHang"] as List<CartViewModel>;
            if (lst == null)
            {
                lst = new List<CartViewModel>();
                Session["GioHang"] = lst;
            }
            return lst;
        }

        //tao ma CTHD
        string CreateCTDH()
        {
            var maMax = data.ChiTietDonHang
                            .ToList()
                            .Select(n => n.MaCTDH)
                            .Where(n => n.StartsWith("CT"))
                            .OrderByDescending(n => n)
                            .FirstOrDefault();

            if (string.IsNullOrEmpty(maMax))
            {
                return "CT001";
            }

            int number = int.Parse(maMax.Substring(2)) + 1;
            string maCTDH = "CT" + number.ToString("D3");
            return maCTDH;
        }


        //tao ma hoa don
        string CreateHD()
        {
            var maMax = data.DonHang
                            .ToList()
                            .Select(n => n.MaDH)
                            .Where(n => n.StartsWith("HD"))
                            .OrderByDescending(n => n)
                            .FirstOrDefault();
            //kiem tra neu maMax la null hoac rong, tra ve ma hoa don mac dinh
            if (string.IsNullOrEmpty(maMax))
            {
                return "HD001";
            }
            //tach so va tang them 1
            int MaDH = int.Parse(maMax.Substring(2)) + 1;
            //tao ma hoa don moi 
            string maHoaDon = "HD" + MaDH.ToString("D3");//dam bao ma hoa don co 3 chu so
            return maHoaDon;
        }

        [HttpGet]
        public ActionResult Orders()
        {
            var taiKhoanSession = Session["TaiKhoan"] as TaiKhoan;
            //if (taiKhoanSession == null)
            //{
            //    TempData["LoginRequired"] = "Bạn cần đăng nhập để đặt hàng.";
            //    return RedirectToAction("Index", "SanPhams");
            //}

            var khachhang = data.KhachHang.FirstOrDefault(kh => kh.MaTK == taiKhoanSession.MaTK);
            //if (khachhang == null)
            //{
            //    TempData["LoginRequired"] = "Không tìm thấy thông tin khách hàng.";
            //    return RedirectToAction("Index", "SanPhams");
            //}

            ViewBag.kh = khachhang;
            List<CartViewModel> lst = GetCartView();
            //ViewBag.Tongsoluong = TotalItems();
            //ViewBag.Tongtien = TotalPrice();

            return View(lst);
        }

        //xay dung chuc nang dat hang
        [HttpPost]
        public ActionResult Orders(FormCollection collection)
        {
            var taiKhoanSession = Session["TaiKhoan"] as TaiKhoan;
            if (taiKhoanSession == null)
                return RedirectToAction("Login", "Account");

            var khachhang = data.KhachHang.FirstOrDefault(kh => kh.MaTK == taiKhoanSession.MaTK);
            if (khachhang == null)
                return RedirectToAction("Index", "SanPhams");

            // ✅ Lấy giỏ hàng từ Session
            var gioHangSession = Session["GioHang"] as List<CartViewModel>;
            if (gioHangSession == null || gioHangSession.Count == 0)
                return RedirectToAction("CartView", "Cart");

            var sanPhamList = data.SanPham.ToList();

            decimal tongTien = 0;
            foreach (var ct in gioHangSession)
            {
                var sp = sanPhamList.FirstOrDefault(s => s.MaSP == ct.MaSP);
                if (sp != null)
                {
                    tongTien += ct.SoLuong * sp.Gia.Value;
                }
            }

            string maHD = CreateHD();

            DonHang ddh = new DonHang
            {
                MaDH = maHD,
                MaKH = khachhang?.MaKH,
                MaNV = null,
                NgayDat = DateTime.Now,
                GioVao = DateTime.Now.TimeOfDay,
                TrangThai = "Chờ xác nhận",
                TongTien = (int)Math.Round(tongTien),
                TienKhachDua = 0,
                TienThoiLai = 0,
                PhuongThucThanhToan = collection["PhuongThucThanhToan"]
            };
            data.DonHang.Add(ddh);

            foreach (var item in gioHangSession)
            {
                var sp = sanPhamList.FirstOrDefault(s => s.MaSP == item.MaSP);
                if (sp != null)
                {
                    ChiTietDonHang ct = new ChiTietDonHang
                    {
                        MaCTDH = CreateCTDH(),
                        MaDH = ddh.MaDH,
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong,
                        DonGia = (int)sp.Gia.Value
                    };
                    data.ChiTietDonHang.Add(ct);
                }
            }

            data.SaveChanges();

            // ✅ Xóa giỏ hàng khỏi Session
            Session["GioHang"] = null;

            return RedirectToAction("ConfirmOrder", "Orders");
        }




        public ActionResult ConfirmOrder()
        {
            return View();
        }
    }
}