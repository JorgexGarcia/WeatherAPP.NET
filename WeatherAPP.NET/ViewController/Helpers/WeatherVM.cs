using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPP.Models;
using WeatherAPP.ViewModel.Commands;

namespace WeatherAPP.Helpers
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set {
                query = value;
                OnPropertyChanged("Query");
            }
        }

        public ObservableCollection<City> Cities {get; set;}

        private Weather weather;

        public Weather Weather
        {
            get { return weather; }
            set
            {
                weather = value;
                OnPropertyChanged("Weather");
            }
        }

        private City city;

        public City City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
                GetWeatherConditions();
            }
        }

        private async void GetWeatherConditions()
        {
            Query = String.Empty;
            Cities.Clear();
            Weather = await WeatherHelper.GetWeather(City.Key);

        }

        public SearchCommand SearchCommand { get; set; }    
        

        public WeatherVM()
        {
            if(DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                City = new City
                {
                    LocalizedName = "Valencia"
                };
                Weather = new Weather
                {
                    WeatherText = "Soleado",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = "21"
                        }
                    }
                };
            }

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        public async void MakeQuery()
        {
            var cities = await WeatherHelper.GetCities(Query);

            Cities.Clear();

        foreach(var city in cities)
            {
                Cities.Add(city);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
