# School Management System

Backend untuk sistem manajemen sekolah yang dibangun menggunakan .NET 8, PostgreSQL, dan autentikasi JWT. Proyek ini mencakup fungsionalitas CRUD untuk mengelola siswa, guru, kelas, pendaftaran, serta sistem peran (Admin, Teacher, Student) dengan hak akses yang spesifik.

## Prasyarat

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- Akses ke database PostgreSQL (misalnya melalui [Supabase](https://supabase.com/))
- Database client seperti [DBeaver](https://dbeaver.io/) (opsional, tapi direkomendasikan).

---

## Hak Akses Pengguna (User Roles)

Sistem ini memiliki tiga peran pengguna dengan wewenang yang berbeda:

👑 Admin
Admin memiliki akses penuh ke seluruh API. Ini adalah peran dengan hak tertinggi.

Akses penuh ke semua endpoint GET, POST, PUT, dan DELETE.

Satu-satunya peran yang bisa membuat dan mengelola data master Students dan Teachers.

Satu-satunya peran yang bisa menugaskan guru ke sebuah kelas (assign-teacher).

Akun admin default (admin@school.com) dibuat secara otomatis saat aplikasi pertama kali dijalankan.

🧑‍🏫 Teacher
Seorang guru hanya bisa mengelola data yang berhubungan dengan kelas yang dia ajar.

Kelas (Classes):

✅ Bisa melihat daftar kelas yang dia ajar saja.

✅ Bisa mengedit detail kelas yang dia ajar saja.

❌ Tidak bisa membuat atau menghapus kelas.

❌ Tidak bisa menugaskan guru lain ke sebuah kelas.

Siswa & Pendaftaran (Students & Enrollments):

✅ Bisa melihat daftar siswa yang terdaftar di kelas miliknya saja.

✅ Bisa mendaftarkan seorang siswa ke sebuah kelas (enroll student).

✅ Bisa menghapus pendaftaran seorang siswa (unenroll student).

🎓 Student
Seorang siswa hanya memiliki akses untuk melihat data yang relevan dengan dirinya sendiri.

Pendaftaran & Kelas (Enrollments & Classes):

✅ Bisa melihat daftar pendaftaran dan info kelas miliknya sendiri melalui endpoint khusus: GET /api/enrollments/me.

✅ Bisa melihat daftar semua kelas yang tersedia di sekolah.

✅ Bisa melihat detail kelas manapun untuk mendapatkan informasi.

Akses Terbatas:

❌ Tidak bisa melihat daftar semua siswa atau semua guru.

❌ Tidak bisa membuat, mengedit, atau menghapus data apa pun.

## Instalasi & Konfigurasi

### 1\. Clone Repositori

```bash
git clone https://github.com/Cobalttt2311/SchoolManagementSystem.git
cd your-repository
```

### 2\. Instalasi Paket NuGet

Jalankan perintah berikut di terminal untuk menginstal semua package yang diperlukan:

```bash
# EF Core & Provider PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.6

# .env file reader
dotnet add package DotNetEnv --version 3.0.0

# ASP.NET Core Identity & JWT
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.6
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.6
```

### 3\. Konfigurasi Database (DB Setup)

Proyek ini menggunakan file `.env` untuk menyimpan koneksi database.

1.  Buat file bernama `.env` di direktori utama proyek.
2.  Isi file tersebut dengan format berikut, ganti dengan detail koneksi database Anda:
    ```env
    # Contoh isi file .env
    DB_CONNECTION_STRING="Host=HOST_ANDA;Port=5432;Database=postgres;Username=USER_ANDA;Password=PASSWORD_ANDA;Pooling=false"
    ```
3.  **PENTING**: Tambahkan `.env` ke dalam file `.gitignore` Anda agar tidak ter-upload ke Git.

### 4\. Konfigurasi JWT

Buka file `appsettings.json` dan pastikan Anda memiliki section `Jwt` untuk konfigurasi token.

```json
{
  // ...
  "Jwt": {
    "Key": "KUNCI_RAHASIA_YANG_SANGAT_PANJANG_DAN_AMAN_UNTUK_PROYEK_INI",
    "Issuer": "http://localhost:5257",
    "Audience": "http://localhost:5257"
  }
  // ...
}
```

_Ganti nilai `"Key"` dengan teks rahasia acak Anda sendiri._

### 5\. Jalankan Migrasi (Migration Instructions)

Setelah konfigurasi di atas selesai, jalankan perintah ini untuk membuat semua tabel di database Anda:

```bash
dotnet ef database update
```

---

## Menjalankan Aplikasi

```bash
dotnet run
```

Aplikasi akan berjalan di `http://localhost:5257`. Dokumentasi API Swagger dapat diakses melalui:
`http://localhost:5257/swagger`

---

## Deskripsi API & Contoh Request

### Autentikasi (`/api/auth`)

#### `POST /api/auth/register`

Mendaftarkan akun login baru.

- **Teacher**: Harus menyertakan `teacherId` dari profil guru yang sudah ada.
- **Student**: Sistem akan mencari profil siswa berdasarkan `email`.

**Contoh Body untuk Teacher:**

```json
{
  "email": "prof.x@school.com",
  "password": "PasswordX123!",
  "role": "Teacher",
  "teacherId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
}
```

**Contoh Body untuk Student:**

```json
{
  "email": "jean.grey@school.com",
  "password": "PasswordJean123!",
  "role": "Student"
}
```

#### `POST /api/auth/login`

Login untuk mendapatkan token JWT.

- **Admin Default**: Email: `admin@school.com`, Password: `Admin@123`

**Contoh Body:**

```json
{
  "email": "admin@school.com",
  "password": "Admin@123"
}
```

### Classes (`/api/classes`)

#### `GET /api/classes`

- **Admin/Student**: Melihat semua kelas.
- **Teacher**: Hanya melihat kelas yang dia ajar.

#### `PUT /api/classes/{id}/assign-teacher`

Menugaskan guru ke sebuah kelas.

- **Hak Akses**: Admin.
- **Contoh Body**:
  ```json
  {
    "teacherId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
  }
  ```

### Enrollments (`/api/enrollments`)

#### `POST /api/enrollments`

Mendaftarkan seorang siswa ke sebuah kelas.

- **Hak Akses**: Admin, Teacher.
- **Contoh Body**:
  ```json
  {
    "studentId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "classId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
  }
  ```

#### `GET /api/enrollments/me`

Melihat daftar pendaftaran dan info kelas milik sendiri.

- **Hak Akses**: Student.
- _Request ini tidak memerlukan body._
