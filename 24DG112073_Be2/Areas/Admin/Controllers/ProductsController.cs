using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _24DG112073_Be2.Models;

namespace _24DG112073_Be2.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Products
        // Lấy và hiển thị danh sách tất cả sản phẩm
        public ActionResult Index()
        {
            // Dùng .Include() để lấy luôn thông tin Category liên quan
            // Giống như "JOIN" trong SQL để có thể hiển thị tên danh mục ở View
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        // Hiển thị thông tin chi tiết của một sản phẩm dựa trên ID
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Tìm sản phẩm theo ID, cũng dùng .Include() để lấy tên danh mục
            Product product = db.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        // Hiển thị form để tạo sản phẩm mới
        public ActionResult Create()
        {
            // Tạo một SelectList chứa danh sách các Category để làm DropDownList
            // "CategoryID" là giá trị (value), "CategoryName" là nội dung hiển thị (text)
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // Nhận dữ liệu từ form và lưu sản phẩm mới vào CSDL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductDescription,ProductPrice,ProductImage,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu dữ liệu nhập vào không hợp lệ, tạo lại SelectList và hiển thị lại form
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        // Lấy thông tin sản phẩm cần sửa và hiển thị form Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            // Tạo SelectList, và chọn sẵn danh mục hiện tại của sản phẩm
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // Nhận dữ liệu và cập nhật sản phẩm trong CSDL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductDescription,ProductPrice,ProductImage,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // Nếu không hợp lệ, tạo lại SelectList
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        // Hiển thị trang xác nhận xóa sản phẩm
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Lấy thông tin sản phẩm và tên danh mục để hiển thị xác nhận
            Product product = db.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        // Thực hiện hành động xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
