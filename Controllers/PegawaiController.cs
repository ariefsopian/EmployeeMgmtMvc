using ClosedXML.Excel;
using EmployeeMgmtMvc.Data;
using EmployeeMgmtMvc.Infrastructure;
using EmployeeMgmtMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmtMvc.Controllers
{
    [Authorize]
    public class PegawaiController : Controller
    {
        private readonly AppDbContext _context;
        public PegawaiController(AppDbContext context)=>_context=context;

        [Authorize(Roles="Admin,Staff")]
        public async Task<IActionResult> Index(string? sortOrder, string? currentFilter, string? searchString, int pageNumber=1, int pageSize=10)
        {
            ViewData["CurrentSort"]=sortOrder;
            ViewData["NameSortParm"]=string.IsNullOrEmpty(sortOrder)?"nama_desc":"";
            ViewData["DeptSortParm"]=sortOrder=="dept"?"dept_desc":"dept";
            ViewData["HireSortParm"]=sortOrder=="hire"?"hire_desc":"hire";
            ViewData["SalarySortParm"]=sortOrder=="salary"?"salary_desc":"salary";

            if (searchString!=null) pageNumber=1; else searchString=currentFilter;
            ViewData["CurrentFilter"]=searchString;

            var query=_context.Pegawai.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query=query.Where(p=>p.Nama.Contains(searchString)||p.Department.Contains(searchString)||p.Email.Contains(searchString)||p.NIK.Contains(searchString));
            }
            query=sortOrder switch
            {
                "nama_desc"=>query.OrderByDescending(p=>p.Nama),
                "dept"=>query.OrderBy(p=>p.Department),
                "dept_desc"=>query.OrderByDescending(p=>p.Department),
                "hire"=>query.OrderBy(p=>p.HireDate),
                "hire_desc"=>query.OrderByDescending(p=>p.HireDate),
                "salary"=>query.OrderBy(p=>p.Salary),
                "salary_desc"=>query.OrderByDescending(p=>p.Salary),
                _=>query.OrderBy(p=>p.Nama),
            };
            var model=await PaginatedList<Pegawai>.CreateAsync(query.AsNoTracking(), pageNumber, pageSize);
            return View(model);
        }

        [Authorize(Roles="Admin")] public IActionResult Create()=>View();
        [Authorize(Roles="Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pegawai pegawai){ if(!ModelState.IsValid) return View(pegawai); _context.Add(pegawai); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(int? id){ if(id==null) return NotFound(); var p=await _context.Pegawai.FindAsync(id); if(p==null) return NotFound(); return View(p); }

        [Authorize(Roles="Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pegawai pegawai){ if(id!=pegawai.Id) return NotFound(); if(!ModelState.IsValid) return View(pegawai); _context.Update(pegawai); await _context.SaveChangesAsync(); return RedirectToAction(nameof(Index)); }

        [Authorize(Roles="Admin,Staff")]
        public async Task<IActionResult> Details(int? id){ if(id==null) return NotFound(); var p=await _context.Pegawai.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id); if(p==null) return NotFound(); return View(p); }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int? id){ if(id==null) return NotFound(); var p=await _context.Pegawai.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id); if(p==null) return NotFound(); return View(p); }

        [Authorize(Roles="Admin")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id){ var p=await _context.Pegawai.FindAsync(id); if(p!=null){ _context.Pegawai.Remove(p); await _context.SaveChangesAsync(); } return RedirectToAction(nameof(Index)); }

        [Authorize(Roles="Admin,Staff")]
        public async Task<IActionResult> ExportExcel(string? searchString, string? sortOrder)
        {
            var query=_context.Pegawai.AsQueryable();
            if(!string.IsNullOrWhiteSpace(searchString))
                query=query.Where(p=>p.Nama.Contains(searchString)||p.Department.Contains(searchString)||p.Email.Contains(searchString)||p.NIK.Contains(searchString));
            query=sortOrder switch
            {
                "nama_desc"=>query.OrderByDescending(p=>p.Nama),
                "dept"=>query.OrderBy(p=>p.Department),
                "dept_desc"=>query.OrderByDescending(p=>p.Department),
                "hire"=>query.OrderBy(p=>p.HireDate),
                "hire_desc"=>query.OrderByDescending(p=>p.HireDate),
                "salary"=>query.OrderBy(p=>p.Salary),
                "salary_desc"=>query.OrderByDescending(p=>p.Salary),
                _=>query.OrderBy(p=>p.Nama),
            };
            var data=await query.AsNoTracking().ToListAsync();
            using var wb=new XLWorkbook(); var ws=wb.Worksheets.Add("Pegawai");
            var headers=new[]{"Id","Nama","NIK","Email","Department","Phone","Alamat","Jenis Kelamin","Tanggal Lahir","Tanggal Masuk","Gaji","No. Rekening"};
            for(int i=0;i<headers.Length;i++) ws.Cell(1,i+1).Value=headers[i];
            ws.Range(1,1,1,headers.Length).Style.Font.Bold=true;
            int r=2;
            foreach(var p in data){
                ws.Cell(r,1).Value=p.Id; ws.Cell(r,2).Value=p.Nama; ws.Cell(r,3).Value=p.NIK; ws.Cell(r,4).Value=p.Email;
                ws.Cell(r,5).Value=p.Department; ws.Cell(r,6).Value=p.Phone; ws.Cell(r,7).Value=p.Alamat; ws.Cell(r,8).Value=p.JenisKelamin.ToString();
                ws.Cell(r,9).Value=p.TanggalLahir; ws.Cell(r,10).Value=p.HireDate; ws.Cell(r,11).Value=p.Salary; ws.Cell(r,12).Value=p.NomorRekening; r++;
            }
            ws.Columns().AdjustToContents();
            using var ms=new MemoryStream(); wb.SaveAs(ms);
            return File(ms.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",$"Pegawai_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }
    }
}