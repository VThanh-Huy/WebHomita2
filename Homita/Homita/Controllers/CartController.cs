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
        private TRA_SUAEntities data = new TRA_SUAEntities();
        // GET: Cart

        public ActionResult Product(string id)
        {
            List<SanPham> sanPhamList;

            if (string.IsNullOrEmpty(id))
            {
                // Hiển thị tất cả sản phẩm nếu không truyền id
                sanPhamList = db.SanPham.ToList();
            }
            else
            {
                // Chỉ hiển thị sản phẩm theo mã
                sanPhamList = db.SanPham.Where(sp => sp.MaSP == id).ToList();
            }

            return View(sanPhamList);
        }

        // Tạo mã ChiTietGioHang
        private string CreateCTGH()
        {
            var maxId = db.ChiTietGioHang
                          .OrderByDescending(c => c.MaCTGH)
                          .Select(c => c.MaCTGH)
                          .FirstOrDefault();

            if (string.IsNullOrEmpty(maxId))
                return "CTGH001";

            int number = int.Parse(maxId.Substring(4)) + 1;
            return "CTGH" + number.ToString("D3");
        }

         string CreateCTDH()
        {
            var maxId = db.GioHang
                          .OrderByDescending(c => c.MaGioHang)
                          .Select(c => c.MaGioHang)
                          .FirstOrDefault();

            if (string.IsNullOrEmpty(maxId))
                return "GH001";

            int number = int.Parse(maxId.Substring(2)) + 1;
            return "GH" + number.ToString("D3");
        }

        // Thêm hoặc cập nhật sản phẩm trong giỏ hàng
        [HttpGet]
        public ActionResult AddCart(string maSP, string returnUrl)
        {
            var tk = Session["TaiKhoan"] as TaiKhoan;
            if (tk == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl ?? Url.Action("CartView", "Cart") });
            }

            var taiKhoan = db.KhachHang.FirstOrDefault(k => k.MaTK == tk.MaTK);
            if (taiKhoan == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = returnUrl ?? Url.Action("CartView", "Cart") });
            }

            var maKH = taiKhoan.MaKH;
            var sanPham = db.SanPham.Find(maSP);
            if (sanPham == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "SanPhams");
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

            Session["MaGioHang"] = cart.MaGioHang;

            var detail = db.ChiTietGioHang.FirstOrDefault(d => d.MaGioHang == cart.MaGioHang && d.MaSP == maSP);
            if (detail == null)
            {
                detail = new ChiTietGioHang
                {
                    MaCTGH = CreateCTGH(),
                    MaGioHang = cart.MaGioHang,
                    MaSP = maSP,
                    SoLuong = 1
                };
                db.ChiTietGioHang.Add(detail);
            }
            else
            {
                detail.SoLuong += 1;
            }

            db.SaveChanges();

            // Trả về redirect về chính trang hiện tại (returnUrl) hoặc trang mặc định
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "SanPhams");
        }
        //update 
        [HttpPost]
        public ActionResult Update(string masp, int soluong)
        {
            var maGH = Session["MaGioHang"] as string;

            if (!string.IsNullOrEmpty(maGH))
            {
                var ctgh = db.ChiTietGioHang.FirstOrDefault(c => c.MaGioHang == maGH && c.MaSP == masp);
                if (ctgh != null)
                {
                    ctgh.SoLuong = soluong;
                    db.SaveChanges();
                }
            }
            else
            {
                // Giỏ hàng cho khách (session)
                var guestCart = Session["GuestCart"] as List<CartViewModel>;
                var item = guestCart?.FirstOrDefault(i => i.MaSP == masp);
                if (item != null)
                {
                    item.SoLuong = soluong;
                }
            }

            return RedirectToAction("CartView");
        }

        //delete
        public ActionResult Delete(string masp)
        {
            var maGH = Session["MaGioHang"] as string;

            if (!string.IsNullOrEmpty(maGH))
            {
                var ctgh = db.ChiTietGioHang.FirstOrDefault(c => c.MaGioHang == maGH && c.MaSP == masp);
                if (ctgh != null)
                {
                    db.ChiTietGioHang.Remove(ctgh);
                    db.SaveChanges();
                }
            }
            else
            {
                var guestCart = Session["GuestCart"] as List<CartViewModel>;
                guestCart?.RemoveAll(i => i.MaSP == masp);
            }

            return RedirectToAction("CartView");
        }

        //delete all
        public ActionResult DeleteAll()
        {
            var maGH = Session["MaGioHang"] as string;

            if (!string.IsNullOrEmpty(maGH))
            {
                var chiTietList = db.ChiTietGioHang.Where(c => c.MaGioHang == maGH).ToList();
                db.ChiTietGioHang.RemoveRange(chiTietList);
                db.SaveChanges();
            }
            else
            {
                Session["GuestCart"] = new List<CartViewModel>();
            }

            return RedirectToAction("CartView");
        }



        // Hiển thị giỏ hàng
        public ActionResult CartView()
        {
            var maGH = Session["MaGioHang"] as string;

            if (!string.IsNullOrEmpty(maGH))
            {
                var entities = db.ChiTietGioHang
                                 .Where(c => c.MaGioHang == maGH)
                                 .ToList();  // Lấy dữ liệu ra khỏi LINQ to Entities

                var items = entities.Select(c => new CartViewModel(c)).ToList();

                ViewBag.TotalQty = items.Sum(i => i.SoLuong);
                ViewBag.Total = items.Sum(i => i.ThanhTien);
                return View(items);
            }
            else
            {
                var guestCart = Session["GuestCart"] as List<CartViewModel> ?? new List<CartViewModel>();

                ViewBag.TotalQty = guestCart.Sum(i => i.SoLuong);
                ViewBag.Total = guestCart.Sum(i => i.ThanhTien);
                return View(guestCart);
            }
        }


    }
}