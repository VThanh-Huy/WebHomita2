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
    public class CartController : Controller
    {
        private TRA_SUAEntities1 data = new TRA_SUAEntities1();
        // GET: Cart

        public ActionResult Product(string id)
        {
            List<SanPham> sanPhamList;

            if (string.IsNullOrEmpty(id))
            {
                // Hiển thị tất cả sản phẩm nếu không truyền id
                sanPhamList = data.SanPham.ToList();
            }
            else
            {
                // Chỉ hiển thị sản phẩm theo mã
                sanPhamList = data.SanPham.Where(sp => sp.MaSP == id).ToList();
            }

            return View(sanPhamList);
        }


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
        private string CreateCTDH()
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
            return "CT" + number.ToString("D3");
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

        //them vao gio hang
        public ActionResult AddCart(string masp, string strURL)
        {
            List<CartViewModel> lst = GetCartView();
            //kiem tra san pham da co trong gio hang chua
            CartViewModel sp = lst.Find(n => n.MaSP == masp);
            if (sp == null)
            {
                sp = new CartViewModel(masp);
                lst.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.SoLuong++;
                return Redirect(strURL);
            }
        }
        //tong so luong
        private int TotalItems()
        {
            int count = 0;
            List<CartViewModel> lst = Session["GioHang"] as List<CartViewModel>;
            if (lst != null)
            {
                count = lst.Sum(n => n.SoLuong);
            }
            return count;
        }

        //tinh tien
        private double TotalPrice()
        {
            double price = 0;
            List<CartViewModel> lst = Session["GioHang"] as List<CartViewModel>;
            if (lst != null)
            {
                price = lst.Sum(n => n.ThanhTien);
            }
            return price;
        }
        //show gio hang
        public ActionResult CartView()
        {
            List<CartViewModel> lst = GetCartView();
            if (lst.Count == 0)
            {
                return RedirectToAction("Index", "SanPhams");
            }
            ViewBag.Tongsoluong = TotalItems();
            ViewBag.TotalPrice = TotalPrice();
            return View(lst);
        }

        //update gio hang
        public ActionResult Update(string masp, FormCollection f)
        {
            //lay gio hang
            List<CartViewModel> lstGiohang = GetCartView();
            CartViewModel sanpham = lstGiohang.FirstOrDefault(n => n.MaSP == masp);
            if (sanpham != null && f["txtSoluong"] != null)
            {
                sanpham.SoLuong = int.Parse(f["txtSoluong"].ToString());

            }
            return RedirectToAction("CartView");
        }

        //Delete
        public ActionResult Delete(string masp)
        {
            List<CartViewModel> lstGiohang = GetCartView();
            CartViewModel sanpham = lstGiohang.FirstOrDefault(n => n.MaSP == masp);
            if (sanpham != null )
            {
                lstGiohang.RemoveAll(n => n.MaSP == masp);
                return RedirectToAction("CartView");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "SanPhams");
            }
            return RedirectToAction("CartView");
        }

        //Xoa tat ca cac san pham
        public ActionResult DeleteAll()
        {
            //lay gio hang tu session 
            List<CartViewModel> lst = GetCartView();
            lst.Clear();
            return RedirectToAction("Index", "SanPhams");
        }

        [HttpGet]
        public ActionResult Orders()
        {
            //kiem tra dang nhap
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //lay gio hang tu Session
            List<CartViewModel> lst = GetCartView();
            ViewBag.Tongsoluong = TotalItems();
            ViewBag.Tongtien = TotalPrice();

            return View(lst);
        }

        //xay dung chuc nang dat hang
        [HttpPost]
        public ActionResult Orders(FormCollection collection)
        {
            //lay tai khoang dang nhap
            string tenDangnhap = Session["UserName"].ToString();
            var taikhoan = data.TaiKhoan.FirstOrDefault(tk => tk.TenDangNhap == tenDangnhap);

            //tao don hang
            DonHang ddh = new DonHang();
            ddh.MaDH = CreateHD();
            ddh.MaKH = taikhoan.MaTK;
            ddh.NgayDat = DateTime.Now;
            ddh.TrangThai = "Chờ xác nhận";
            ddh.TongTien = (int)TotalPrice();
            ddh.PhuongThucThanhToan = collection["PhuongThucThanhToan"]; // nếu có
            data.DonHang.Add(ddh);

            //lay gio hang tu session
            List<CartViewModel> lst = GetCartView();
            //check gio hang null ko
            if (lst == null || lst.Count == 0)
            {
                return RedirectToAction("CartView");
            }

            foreach (var items in lst)
            {
                ChiTietDonHang ct = new ChiTietDonHang();
                ct.MaCTDH = CreateCTDH();
                ct.MaDH = ddh.MaDH;
                ct.MaSP = items.MaSP;
                ct.SoLuong = items.SoLuong;
                ct.DonGia = (int)items.Gia;

                data.ChiTietDonHang.Add(ct);

            }

            data.SaveChanges();

            //Xoa gio hang sau khi dat\
            Session["GioHang"] = null;

            return RedirectToAction("ConfirmOrder", "Cart");

        }
    }
}