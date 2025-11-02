# 🌿SortIt

## Eesmärk
**SortIt** - õppe- ja mängurakendus, mis aitab kasutajal õppida jäätmete sortimist.  
Rakendus ühendab hariva osa ja lihtsa mängu, kus tuleb määrata, millisesse konteinerisse erinevad jäätmed kuuluvad.  
Eesmärk on tõsta keskkonnateadlikkust ja muuta õppimine lõbusaks.

---

## Funktsioonid
### **Mängurežiim**
Kasutaja peab ekraanil kuvatud jäätme õigesse prügikasti lohistama.  
Iga õige vastus annab kogemuspunkte (XP), vale vastus aga ei anna punkte ja käivitab lühikese vibratsiooni.  
Mängu eesmärk on õpetada jäätmete sortimist läbi praktilise ja mängulise kogemuse. 

<img width="382" height="786" alt="image" src="https://github.com/user-attachments/assets/c6f9b73c-a9f9-40d1-8bee-621469d7f3d0" />
<img width="385" height="789" alt="image" src="https://github.com/user-attachments/assets/d38ee3d0-a565-4017-ac6a-eb417e64975c" />

### **Õpijuhend (Guide)**
Rakenduse õppemoodul, mis koosneb slaididest, kus on kujutatud eri tüüpi jäätmed ja konteinerid.  
Igal slaidil on pilt ja nimekiri näidetest (nt „klaaspudel”, „ajaleht”, „patarei”).  
See funktsioon aitab kasutajal enne mängu õppida, kuidas jäätmeid õigesti sorteerida.  

<img width="383" height="788" alt="image" src="https://github.com/user-attachments/assets/86cb8438-d516-43ce-a6bb-e06f096842e5" />

### **Profiilileht**
Kasutaja isiklik leht, kus kuvatakse tema nimi, valitud avatar, tase, XP hulk ning õigete ja valede vastuste koguarv.  
Profiiliinfo salvestatakse lokaalsesse SQLite andmebaasi, et andmed säiliks ka rakenduse taaskäivitamisel.

Lisaks on lehel kujutatud **taime kasv**, mis sümboliseerib kasutaja arengut – alguses on see väike idu, kuid taseme tõustes kasvab see suureks puuks.  See visuaalne element motiveerib mängijat jätkama ja näitab tema edusamme keskkonnateadlikkuse teel.

<img width="380" height="785" alt="image" src="https://github.com/user-attachments/assets/4367e4b2-b216-42e2-a4de-4aa5b3bd0e72" />

### **Profiili muutmine**
Võimaldab kasutajal muuta oma nime ja valida uue avatari nelja valiku seast.  
Muudatused salvestatakse koheselt andmebaasi ja kajastuvad profiililehel.  

<img width="373" height="353" alt="image" src="https://github.com/user-attachments/assets/6436fa1d-e31e-4bcd-a60a-b6dd5161e09e" />

### **Seaded**
Annab kasutajale võimaluse rakendust isikupärastada.  
  Seal saab:  
  - muuta kasutajaliidese keelt (eesti, inglise, vene)
  - lülitada heli sisse või välja
  - valida hele või tume teema
Keelevahetus toimub kohe, ilma rakenduse taaskäivitamiseta.  
<img width="378" height="781" alt="image" src="https://github.com/user-attachments/assets/5fe26c18-6e6b-4f72-9b20-4be1fc6d745e" />
<img width="378" height="785" alt="image" src="https://github.com/user-attachments/assets/0f7679f4-f9f0-40dc-abd8-7fbb9bf3c81b" />

### **Tasemete süsteem**
XP kogumise kaudu tõuseb kasutaja tase automaatselt.  
Iga uus tase annab uue tiitli (nt *„Recycler”*, *„Green Hero”*) ja muudab taimepildi, mis sümboliseerib kasutaja arengut keskkonnateadlikkuse teel.  

### **Heliefektid ja vibratsioon**
Iga õige vastuse korral kõlab positiivne helisignaal, vale vastuse korral negatiivne.  
Android-seadmetes aktiveeritakse ka lühike vibratsiooniefekt, mis muudab mängukogemuse dünaamilisemaks ja tagasisidestatumaks.  

---

## Kasutatud tehnoloogiad
- **.NET MAUI** – rakenduse raamistik (UI ja loogika).  
- **C#** – põhikeel.  
- **SQLite** – kasutajaandmete (profiil, XP, statistika) salvestamine.  
- **MVVM arhitektuur** – `Models`, `ViewModels`, `Views` eraldatus.  
- **Resx-lokaliseerimine** – mitmekeelne tugi (et / en / ru).  
- **Plugin.Maui.Audio** – helide esitamiseks.  

---

## ✅ Mis töötab
- Profiili loomine ja salvestamine  
- Keelevahetus ja teemade tugi  
- Mängu loogika ja animatsioonid  
- Heliefektid ja vibratsioon  
- Õppemoodul (slaidid)  

## ⚠️ Mis jäi pooleli
- Mõned visuaalsed detailid ja kujunduse kohandused võivad vajada täiendamist  
- Andmebaasi funktsioonide laiendamine (nt mitme profiili tugi) on planeeritud, kuid veel teostamata
- Võimalik tulevikus lisada **jäätmekonteinerite kaart**, mis aitaks kasutajal leida lähimad kogumispunktid ning muudaks rakenduse praktiliselt kasulikuks  
- **Saavutuste süsteemi** lisamine (nt „100 õiget vastust”, „10. tase saavutatud”) mängija motiveerimise ja kaasamise suurendamiseks

---

📌 Autor: *Maria Smolina*  
