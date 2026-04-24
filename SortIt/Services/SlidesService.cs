using SortIt.Models;
using SortIt.Resources.Constants;
using SortIt.Resources.Localization;
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
                Image = AppResources.a_klaaspakend,
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
                Image = AppResources.b_ohtlikudjaatmed,
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
                Image = AppResources.c_pandipakend,
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
                Image = AppResources.d_pappjapaberpakend,
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
                Image = AppResources.e_plastmetalljoogikartong,
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
                Image = AppResources.f_ringlusnoud,
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
                Image = AppResources.g_segaolmejaatmed,
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
                Image = AppResources.h_biojaatmed,
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
                Image = AppResources.h_vanapaber,
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
