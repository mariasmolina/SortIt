using System.Collections.ObjectModel;
using SortIt.Models;
using SortIt.Services;
using System.ComponentModel;

namespace SortIt.ViewModels
{
    public class GuideViewModel : INotifyPropertyChanged
    {
        public List<Slide> Slides { get; set; }
        private SlidesService slidesService;

        public GuideViewModel()
        {
            slidesService = new SlidesService();
            Slides = new List<Slide>();
        }

        // Загружает слайды из сервиса
        public void LoadSlides()
        {
            Slides.Clear();

            // получает список слайдов из сервиса
            Slides = slidesService.GetSlides();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Slides)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}