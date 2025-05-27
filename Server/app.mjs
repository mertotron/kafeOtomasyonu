import express from 'express';
import mysql from 'mysql2/promise';
import cors from 'cors';
import dotenv from 'dotenv';

dotenv.config();
const app = express();
app.use(cors());
app.use(express.json());

// Veritabanı bağlantısı
const connection = mysql.createPool({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME
});
app.get('/', (req, res)=>{
  console.log("get testi alınıyor sunucu aktif");
  res.send("get testi alınıyor sunucu aktif"); 
});

app.post('/route/serialcontrol', async (req, res) => {
  const { username, password, userId } = req.body;
  const query = `
    SELECT
      sts.SerialID,
      sts.SerialNo,
      sts.SerialDate,
      sts.SerialStatus,
      us.iduserServer,
      us.usernameServer,
      us.useryetkiServer
    FROM serialtableserver AS sts
    INNER JOIN userserver AS us ON sts.UserID = us.iduserServer
    WHERE us.usernameServer = ? AND us.userpasswordServer = ? AND us.iduserServer = ?
  `;
  try {
    const [result] = await connection.query(query, [username, password, userId]);
    if (result.length > 0) {
      const row = result[0];
      console.log("true");
      return res.status(200).json({
        success: true,
        message: "Kullanıcı ve serial doğrulandı",
        serialNo: row.SerialNo,
        serialDate: row.SerialDate,
        serialStatus: row.SerialStatus,
        username: row.usernameServer,
        yetki: row.useryetkiServer
      });
    } else {
      console.log("false");
      return res.status(401).json({ success: false, message: "Bilgiler hatalı veya kayıt yok" });
    }
  } catch (err) {
    console.error("Sorgu hatası:", err);
    return res.status(500).json({ success: false, message: "Sunucu hatası" });
  }
});
// Sunucu başlat
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Sunucu çalışıyor: http://localhost:${PORT}`);
});
