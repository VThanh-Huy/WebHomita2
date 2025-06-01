using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Homita.Models;

namespace Homita.Controllers
{
    public class SanPhamsController : Controller
    {
        private TRA_SUAEntities1 db = new TRA_SUAEntities1();

        // GET: SanPhams
        public ActionResult Index()
        {
            var sanPham = db.SanPham.Include(s => s.LoaiSanPham);
            return View(sanPham.ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham, "MaLoaiSP", "TenLoaiSP");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                // Nếu có ảnh được upload
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    // Lấy tên file
                    string fileName = Path.GetFileName(ImageUpload.FileName);
                    // Đường dẫn lưu file (thư mục ~/Content/Images)
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    // Lưu file
                    ImageUpload.SaveAs(path);

                    // Lưu đường dẫn vào thuộc tính HinhAnh (dùng trong View)
                    sanPham.HinhAnh = "~/Content/Images/" + fileName;
                }

                db.SanPham.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }


        // GET: SanPhams/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        //POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                // Nếu người dùng upload ảnh mới
                //if (sanPham.ImageUpload != null && sanPham.ImageUpload.ContentLength > 0)
                //{
                //    string fileName = Path.GetFileNameWithoutExtension(sanPham.ImageUpload.FileName);
                //    string extension = Path.GetExtension(sanPham.ImageUpload.FileName);
                //    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                //    string path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                //    sanPham.ImageUpload.SaveAs(path);

                //    // Cập nhật thuộc tính HinhAnh
                //    sanPham.HinhAnh = "~/Content/Images/" + fileName;
                //}

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham, "MaLoaiSP", "TenLoai", sanPham.MaLoaiSP);
            return View(sanPham);
        }


        // GET: SanPhams/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPham.Find(id);
            // Kiểm tra ràng buộc khóa ngoại trước khi xóa
            db.SanPham.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
