# 🌿SortIt rakendus

## 📖 Eesmärk
**SortIt** - õppe- ja mängurakendus, mis aitab kasutajal õppida jäätmete sortimist.  

Väljatöötatud mobiilirakendus on mõeldud selleks, et aidata kasutajatel õppida jäätmete sorteerimise reegleid, kontrollida oma teadmisi mängulises vormis ning määrata jäätmete liiki, tuvastades need kaamera pildil.

---

## 🎨 Disain ja prototüüp

Rakenduse kasutajaliidese disain, stiiliraamat ja visuaalsed komponendid on loodud **Figma** keskkonnas.

🔗 **[Figma Stiiliraamat](https://www.figma.com/design/dxGcRIHd064rljQgFvAXuR/SortIt-mobiilirakenduse-stiiliraamat?node-id=6-757&t=0pEQsRTs7IFWeZiv-1)**  

<img width="677" height="446" alt="image" src="https://github.com/user-attachments/assets/d53752bb-4b08-4736-9957-77dd9032e6f3" />

---

## ⚙️ Funktsioonid
### **Mängurežiim**
Kasutaja peab ekraanil kuvatud jäätme õigesse prügikasti lohistama.  
Iga õige vastus annab kogemuspunkte (XP), vale vastus aga ei anna punkte ja käivitab lühikese vibratsiooni.  
Mängu eesmärk on õpetada jäätmete sortimist läbi praktilise ja mängulise kogemuse. 

<img width="592" height="585" alt="image" src="https://github.com/user-attachments/assets/9d41b200-a110-4699-875e-13ec0fe35065" />


### **Õpijuhend (Guide)**
Rakenduse õppemoodul, mis koosneb slaididest, kus on kujutatud eri tüüpi jäätmed ja konteinerid.  
Igal slaidil on pilt ja nimekiri näidetest (nt „klaaspudel”, „ajaleht”, „patarei”).  
See funktsioon aitab kasutajal enne mängu õppida, kuidas jäätmeid õigesti sorteerida.  

### **Jäätmete tuvastamine kaameraga**
Kasutaja saab teha esemest foto ning rakendus analüüsib pilti kasutades ***Google Cloud Vision API-t***, et määrata jäätmeliik ja soovitada sobiv konteiner.

<img width="558" height="566" alt="image" src="https://github.com/user-attachments/assets/e65228da-b001-47dd-9af0-b2d687fce4b4" />


### **Profiilileht**
Kasutaja isiklik leht, kus kuvatakse tema:
- nimi
- valitud avatar
- tase
- XP hulk
- õigete ja valede vastuste koguarv

Profiiliinfo salvestatakse lokaalsesse SQLite andmebaasi, et andmed säiliksid ka rakenduse taaskäivitamisel.
Kasutaja saab profiili kohandada:
- muuta oma nime
- valida avatari
- jälgida oma taset ja statistikat

> Lisaks on lehel kujutatud *taime kasv*, mis sümboliseerib kasutaja arengut – alguses on see väike idu, kuid taseme tõustes kasvab see suureks puuks.
> See visuaalne element motiveerib mängijat jätkama ja näitab tema edusamme keskkonnateadlikkuse teel.

### **Statistika ja analüütika**
Rakendus salvestab mängutulemused ning kuvab kasutaja statistikat diagrammide kujul:
- õiged / valed vastused
- mängude arv
- XP punktid
- jäätmeliikide kaupa statistika
- edenemine päevade ja kuude lõikes

<img width="1022" height="517" alt="image" src="https://github.com/user-attachments/assets/8b940504-ce21-482f-83e9-0c0f5da9d756" />

### **Edenemissüsteem**
XP kogumise kaudu tõuseb kasutaja tase automaatselt.  
Iga uus tase annab uue tiitli (nt *„Recycler”*, *„Green Hero”*) ja muudab taimepildi, mis sümboliseerib kasutaja arengut keskkonnateadlikkuse teel.
Rakendus kasutab mängustamise elemente:
- XP punktide kogumine
- Tasemesüsteem
- Dünaamiline taime kasv vastavalt kasutaja arengule

### **Seaded**
Annab kasutajale võimaluse rakendust isikupärastada.  
  Seal saab:  
  - muuta kasutajaliidese keelt (eesti, inglise, vene)
  - lülitada heli sisse või välja
  - valida hele või tume teema
> Keelevahetus toimub kohe, ilma rakenduse taaskäivitamiseta.

<img width="451" height="457" alt="image" src="https://github.com/user-attachments/assets/d4b75ab0-bb27-4313-a074-978d9fe3f418" />

### **Heliefektid ja vibratsioon**
Iga õige vastuse korral kõlab positiivne helisignaal, vale vastuse korral negatiivne.  
Android-seadmetes aktiveeritakse ka lühike vibratsiooniefekt, mis muudab mängukogemuse dünaamilisemaks ja tagasisidestatumaks.  

---

## 🛠 Kasutatud tehnoloogiad
- **.NET MAUI** – rakenduse raamistik (UI ja loogika).  
- **C#** – põhikeel.  
- **SQLite** – kasutajaandmete (profiil, XP, statistika) salvestamine.  
- **MVVM arhitektuur** – `Models`, `ViewModels`, `Views` eraldatus.  
- **Resx-lokaliseerimine** – mitmekeelne tugi (et / en / ru).  
- **Plugin.Maui.Audio** – helide esitamiseks.
- **Google Vision API** – objektide tuvastamine pildilt
- **Syncfusion UI Components** – diagrammid ja statistika visualiseerimine

---

## 🚀 Paigaldamine

### 1. Klooni repositoorium
```bash
git clone https://github.com/mariasmolina/SortIt.git
```

### 2. Loo konfiguratsioonifail
Projekt kasutab API võtmete ja litsentside hoidmiseks lokaalselt faili **`appsettings.json`**.

Liigu projekti kausta **`Resources/Raw`** ning kopeeri näidisfail:
```bash
cp appsettings.example.json appsettings.json
```
**Windows PowerShell / CMD puhul:**
```bash
copy appsettings.example.json appsettings.json
```

### 3. Lisa oma võtmed faili appsettings.json
Ava loodud fail ning sisesta oma API ja litsentsivõtmed:
```json
{
  "GoogleVision": {
    "ApiKey": "SINU_GOOGLE_VISION_API_VÕTI"
 },
  "Syncfusion": {
    "LicenseKey": "SINU_SYNCFUSION_LITSENTSIVÕTI"
  }
}
```
> [!NOTE]
> #### ***Kust võtmed saada?***
> **Google Vision API võti**:<br>
> Loo Google Cloud projekt ja aktiveeri Vision API Google Cloud Console'is:<br>
> 🔗 https://console.cloud.google.com/
>
> **Syncfusion License Key**:<br>
> Registreeru Syncfusioni veebilehel ja loo tasuta Community License / hanki litsentsivõti:<br>
> 🔗 https://www.syncfusion.com/account/manage-license
>
> Mõlema teenuse kasutamiseks võib olla vajalik konto loomine ja vastava teenuse aktiveerimine.


### 4. Käivita projekt
Ava lahendus ***Visual Studio*** või ***JetBrains Rideris*** ning käivita soovitud platvormil.

---

## 🔐 Turvalisus
> [!IMPORTANT]
> Repositoorium ei sisalda turvakaalutlustel päris API võtmeid ega litsentse.
>
> Kõik tundlikud andmed tuleb lisada **lokaalselt** faili `appsettings.json`.
>
> Fail `appsettings.json` on lisatud `.gitignore` faili ning seda GitHubi ei laadita.

---

📌 Autor: *Maria Smolina*  
