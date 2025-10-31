using SortIt.Models;
using SortIt.ViewModels;

namespace SortIt.Services
{
    public class SlidesService
    {
        private readonly LanguageViewModel lang = new LanguageViewModel();

        public static SlidesService Service = new SlidesService();

        // Возвращает слайды для карусели
        public List<Slide> GetSlides()
        {
            var slides = new List<Slide>();

            slides.Add(new Slide
            {
                Image = "a_klaaspakend.png",
                Title = "Klaaspakend",
                Items = new List<string> {
                    lang.GlassBootle,
                    lang.GlassCan,
                    lang.PerfumeBottle,
                    lang.GlassJarLid
                }
            });

            slides.Add(new Slide
            {
                Image = "b_ohtlikudjaatmed.png",
                Title = "Ohtlikud jäätmed",
                Items = new List<string> {
                    lang.Battery,
                    lang.Bulb,
                    lang.PaintCan,
                    lang.Thermometer
                }
            });

            slides.Add(new Slide
            {
                Image = "c_pandipakend.png",
                Title = "Pandipakend",
                Items = new List<string> {
                    lang.DepositBootle,
                    lang.DepositCan,
                    lang.DepositGlass,
                    lang.PlasticBottleCap
                }
            });

            slides.Add(new Slide
            {
                Image = "d_pappjapaberpakend.png",
                Title = "Papp ja paberpakend",
                Items = new List<string> {
                    lang.Box,
                    lang.Newspaper,
                    lang.PaperBag,
                    lang.CerealBox
                }
            });

            slides.Add(new Slide
            {
                Image = "e_plastmetalljoogikartong.png",
                Title = "Plast, metall, joogikartong",
                Items = new List<string> {
                    lang.FilmWrapping,
                    lang.MetalCan,
                    lang.PlasticBottle,
                    lang.JuiceCarton,
                    lang.TinLid
                }
            });

            slides.Add(new Slide
            {
                Image = "f_ringlusnoud.png",
                Title = "Ringlusnõud",
                Items = new List<string> {
                    lang.ReusableMug,
                    lang.ReusablePlate,
                    lang.ReusableCutlery,
                    lang.ReusableBottle
                }
            });

            slides.Add(new Slide
            {
                Image = "g_segaolmejaatmed.png",
                Title = "Segaolmejäätmed",
                Items = new List<string> {
                    lang.Napkin,
                    lang.MedicalMask,
                    lang.Toothbrush,
                    lang.CeramicShard,
                    lang.CigaretteButt
                }
            });

            slides.Add(new Slide
            {
                Image = "h_biojaatmed.png",
                Title = "Biojäätmed",
                Items = new List<string> {
                    lang.AppleCore,
                    lang.BananaPeel,
                    lang.TeaBag,
                    lang.VegetablePeels,
                    lang.EggShells
                }
            });

            slides.Add(new Slide
            {
                Image = "h_vanapaber.png",
                Title = "Vanapaber",
                Items = new List<string> {
                    lang.PaperNewspaper,
                    lang.Magazine,
                    lang.OldBook,
                    lang.Notebook,
                    lang.Envelope
                }
            });

            return slides;
        }
    }
}
