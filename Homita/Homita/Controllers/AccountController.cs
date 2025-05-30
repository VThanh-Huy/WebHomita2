using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Homita.Models;
using Homita.Models.Helper;
using Homita.Models.ViewModel;
using Microsoft.Ajax.Utilities;

namespace WebTraSua.Controllers
{
    public class AccountController : Controller
    {
        private TRA_SUAEntities1 db = new TRA_SUAEntities1();
        private Create_UserID maTuDong;
        public AccountController()
        {
            maTuDong = new Create_UserID(db);
        }

        public ActionResult Profile()
        {
            var taiKhoan = Session["TaiKhoan"] as TaiKhoan;
            if (taiKhoan == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Nếu muốn hiển thị thông tin khách hàng
            var khachHang = db.KhachHang.FirstOrDefault(k => k.MaTK == taiKhoan.MaTK);
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(khachHang);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string tendangnhap, string Matkhau)
        {
            var taiKhoan = db.TaiKhoan.FirstOrDefault(t => t.TenDangNhap == tendangnhap && t.MatKhau == Matkhau);
            if (taiKhoan != null)
            {
                Session["TaiKhoan"] = taiKhoan;

                // Lấy khách hàng tương ứng với tài khoản
                var khachHang = db.KhachHang.FirstOrDefault(k => k.MaTK == taiKhoan.MaTK);
                if (khachHang != null)
                {
                    // Lấy giỏ hàng của khách hàng nếu có
                    var gioHang = db.GioHang.FirstOrDefault(g => g.MaKH == khachHang.MaKH);
                    if (gioHang != null)
                    {
                        Session["MaGioHang"] = gioHang.MaGioHang;
                    }
                    else
                    {
                        // Nếu chưa có giỏ hàng, có thể tạo mới hoặc không làm gì
                        Session["MaGioHang"] = null;
                    }
                }

                return RedirectToAction("Index", "Home"); // hoặc trang bạn muốn chuyển đến sau khi login thành công
            }
            else
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Kiểm tra xem Email hoặc Tên đăng nhập đã tồn tại chưa
                var existingAccount = db.TaiKhoan.FirstOrDefault(tk => tk.Email == model.Email || tk.TenDangNhap == model.TenDangNhap);
                if (existingAccount != null)
                {
                    ModelState.AddModelError("", "Email hoặc Tên đăng nhập đã được sử dụng.");
                    return View(model);
                }

                // Tạo mã tài khoản mới
                string newMaTK = maTuDong.GenerateNewMaTK();

                // Thêm vào bảng TaiKhoan
                var taiKhoan = new TaiKhoan
                {
                    MaTK = newMaTK,
                    Email = model.Email,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = model.MatKhau, 
                    VaiTro = model.VaiTro
                };
                db.TaiKhoan.Add(taiKhoan);
                db.SaveChanges();

                if (model.VaiTro == "KhachHang")
                {
                    // Tạo mã khách hàng mới
                    string newMaKH = maTuDong.GenerateNewMaKH();

                    // Thêm vào bảng KhachHang
                    var khachHang = new KhachHang
                    {
                        MaKH = newMaKH,
                        HoTen = model.HoTen,
                        SDT = model.SDT,
                        DiaChi = model.DiaChi,
                        MaTK = newMaTK
                    };
                    db.KhachHang.Add(khachHang);
                }
                else if (model.VaiTro == "NhanVien")
                {
                    // Tạo mã nhân viên mới
                    string newMaNV = maTuDong.GenerateNewMaNV();

                    // Thêm vào bảng NhanVien
                    var nhanVien = new NhanVien
                    {
                        MaNV = newMaNV,
                        HoTen = model.HoTen,
                        Email = model.Email,
                        MatKhau = model.MatKhau, 
                        MaTK = newMaTK
                    };
                    db.NhanVien.Add(nhanVien);
                }

                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }

}