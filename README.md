# EmployeeMgmtMvc (.NET 8) — Export Excel Only + Improved Paging

- CRUD Pegawai + Search, Sort, Paging (First/Last, nomor halaman, rentang data)
- Identity + Roles (Admin/Staff) seeded (tanpa dummy pegawai)
- Export **Excel** saja (ClosedXML)
- Footer

## Setup
1) Pastikan SQL Server Docker aktif:
   docker start sqlserver
2) Migrasi:
   Add-Migration InitialCreate
   Update-Database
3) Run (F5). Login:
   - admin@local.test / Admin123!
   - staff@local.test / Staff123!