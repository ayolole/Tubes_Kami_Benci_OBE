# Tubes_Kami_Benci_OBE

Repository tugas besar Strategi Algoritma berbasis **Robocode Tank Royale** menggunakan **C# (.NET 10)**. Proyek ini berisi beberapa bot dengan strategi greedy berbeda untuk bertahan hidup, mencari target, dan menyerang lawan pada arena Robocode.

## Anggota Kelompok

| Nama | NIM |
|------|-----|
| Melvin Satria Gunanta Sitepu | 124140173 |
| Qinthafa Treza Aimar | 124140155 |
| Govin Kurniawansyah | 124140025 |

---

# Daftar Bot

## 1. Yujin
**Strategi: Target Lock + Strafing Attack**

Fitur:
- Mengunci satu target utama
- Menjaga radar tetap fokus pada target
- Bergerak menyamping (strafe) agar lebih sulit ditembak
- Menembak berdasarkan posisi target yang terkunci

Prioritas:
```txt
Deteksi target
↓
Lock target
↓
Strafe movement
↓
Tracking
↓
Fire
```

---

## 2. AllMight
**Strategi: Nearest Enemy Hunter**

Fitur:
- Memprioritaskan musuh dengan jarak terdekat
- Mengganti target jika ada musuh lain yang lebih dekat
- Menyerang agresif pada jarak dekat
- Cocok untuk duel maupun kondisi banyak musuh

Prioritas:

```txt
Cari musuh
↓
Pilih target terdekat
↓
Dekati
↓
Fire
```

---

## 3. Deku
**Strategi: Free Movement + Predictive Shooting**

Fitur:
- Bergerak dinamis agar sulit diprediksi
- Menggunakan prediksi arah musuh saat menembak
- Fokus pada survival dan efisiensi tembakan

Prioritas:

```txt
Gerak bebas
↓
Prediksi posisi lawan
↓
Aim
↓
Fire
```

---

## 4. Bakugo
**Strategi: Aggressive Lock-On**

Fitur:
- Mengunci target pertama yang tertangkap radar
- Bergerak mendekati lawan
- Menyerang secara agresif dengan tembakan kontinu

Prioritas:

```txt
Scan
↓
Lock target
↓
Mendekat
↓
Fire terus
```

---

# Requarement

- Language: **C# 10**
- Framework: **.NET 10**
- API: **Robocode.TankRoyale.BotApi 0.30.0**
- Engine: **Robocode Tank Royale GUI**

---

# Cara Menjalankan

## 1. Jalankan GUI Robocode

Download:

- `robocode-tankroyale-gui-0.30.0.jar`

Lalu jalankan:

```bash
java -jar robocode-tankroyale-gui-0.30.0.jar
```

---

## 2. Tambahkan Directory Bot

Pada GUI:

```txt
Config
↓
Bot Root Directories
↓
Tambahkan folder bot
```

---

## 3. Build Bot

Masuk ke folder bot:

```bash
dotnet build
```

Atau:

Windows:

```cmd
BotName.cmd
```

Linux:

```bash
./BotName.sh
```

---

## 4. Boot Bot

Pada GUI:

```txt
Battle
↓
Start Battle
↓
Boot bot
↓
Add bot
↓
Start Battle
```

---

# Struktur Folder

Contoh struktur:

```txt
Tubes_Kami_Benci_OBE/
│
├── doc/
│    └──  KamiBenciOBE.pdf
├── src/
│    ├── main-bot/
│    │       └──  Yujin/           
│    │
│    └── alternative-bots/
│            ├── AllMight/
│            ├── Deku/
│            └── Bakugo/
│
└── README.md
```

---

# Konfigurasi Bot

Contoh file `.json`:

```json
{
  "name": "Yujin",
  "version": "1.0",
  "authors": [
      "Melvin",
      "Qinthafa",
      "Govin"
  ],
  "countryCodes": ["id"],
  "platform": ".Net 10.0",
  "programmingLang": "C# 10"
}
```

---

# Referensi

- Robocode Tank Royale Starter Guide
- Robocode Tank Royale API
- Dokumentasi .NET

---

**Kelompok: Kami Benci OBE**  
