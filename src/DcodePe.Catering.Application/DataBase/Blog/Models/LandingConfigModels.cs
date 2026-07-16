using System.Collections.Generic;



namespace DcodePe.Catering.Application.DataBase.Blog.Models

{

    public class LandingConfigModel

    {

        public List<HeroSlideModel> HeroSlides { get; set; } = new();

        public PageBannersModel PageBanners { get; set; } = new();

        public List<StatItemModel> Stats { get; set; } = new();

    }



    public class HeroSlideModel

    {

        public string? Tag { get; set; }

        public string? Title { get; set; }

        public string? Subtitle { get; set; }

        public string? ImageUrl { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; } = true;

    }



    public class PageBannersModel

    {

        public string? HomeCta { get; set; }

        public string? Nosotros { get; set; }

        public string? Servicios { get; set; }

        public string? Locales { get; set; }

        public string? Contacto { get; set; }

        public string? Presupuestador { get; set; }

        public string? Login { get; set; }

        public string? Registro { get; set; }

    }



    public class StatItemModel

    {

        public string? Value { get; set; }

        public int? NumericValue { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string? Label { get; set; }

        public bool Animate { get; set; }

    }

}

