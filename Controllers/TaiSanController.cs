using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TranVanKiet_2026_2.Models;

namespace TranVanKiet_2026_2.Controllers
{
    public class TaiSanController : Controller
    {
        private readonly QLTaiSanContext _context;

        public TaiSanController(QLTaiSanContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var danhSach = await _context.TaiSans
                .Include(t => t.MaLoaiNavigation)
                .Where(t => t.NguyenGia != null && t.NguyenGia >= 20)
                .ToListAsync();

            ViewBag.SectionTitle = "Trần Văn Kiệt";
            return View(danhSach);
        }

        
        [HttpGet]
        public async Task<IActionResult> FilterByLoai(int maLoai)
        {
            var query = _context.TaiSans
                .Include(t => t.MaLoaiNavigation)
                .Where(t => t.NguyenGia != null && t.NguyenGia >= 20);

            if (maLoai > 0)
            {
                query = query.Where(t => t.MaLoai == maLoai);
                var loai = await _context.LoaiTaiSans.FindAsync(maLoai);
                ViewBag.SectionTitle = loai?.TenLoai ?? "Tài sản";
            }
            else
            {
                ViewBag.SectionTitle = "Trần Văn Kiệt";
            }

            var ketQua = await query.ToListAsync();
            return PartialView("_DanhSachTaiSan", ketQua);
        }

 
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var taiSan = await _context.TaiSans.FindAsync(id);
            if (taiSan == null) return NotFound();

            ViewBag.MaLoai = new SelectList(_context.LoaiTaiSans, "MaLoai", "TenLoai", taiSan.MaLoai);
            return View(taiSan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTs,MaLoai,TenTs,NguyenGia,Anh")] TaiSan taiSan)
        {
            if (id != taiSan.MaTs) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiSan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TaiSans.Any(e => e.MaTs == taiSan.MaTs))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaLoai = new SelectList(_context.LoaiTaiSans, "MaLoai", "TenLoai", taiSan.MaLoai);
            return View(taiSan);
        }
    }
}