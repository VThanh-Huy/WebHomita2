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
            var session = Session["TaiKhoan"] as KhachHang;
            if(session != null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(session);
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var user = db.TaiKhoan
                         .FirstOrDefault(u => u.Email.Trim() == Email.Trim() && u.MatKhau.Trim() == Password);

            if (user != null)
            {
                Session["UserName"] = user.TenDangNhap;
                Session["VaiTro"] = user.VaiTro;

<<<<<<< Updated upstream
                if (user.VaiTro.ToLower() == "admin")
                {
                    return RedirectToAction("Index", "AdminHome"); 
                }
                else
                {
                    return RedirectToAction("Index", "Home"); 
                }
=======
                if(user.VaiTro != "KhachHang")
                {
                    var khachang  = db.KhachHang.FirstOrDefault(kh => kh.MaTK == user.MaTK);
                    if(khachang != null)
                    {
                        Session["TaiKhoan"] = khachang.MaTK;
                    }
                }
                return RedirectToAction("Index", "Home");
>>>>>>> Stashed changes
            }

            TempData["LoginError"] = "Sai tài khoản hoặc mật khẩu!";
            TempData["ShowLoginModal"] = true;
            return RedirectToAction("Index", "Home");
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