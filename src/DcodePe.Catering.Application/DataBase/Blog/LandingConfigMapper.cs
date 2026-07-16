using System.Text.Json;
using DcodePe.Catering.Application.DataBase.Blog.Models;
using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Blog
{
    public static class LandingConfigMapper
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public static LandingConfigModel Parse(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new LandingConfigModel();

            try
            {
                return JsonSerializer.Deserialize<LandingConfigModel>(json, JsonOptions) ?? new LandingConfigModel();
            }
            catch
            {
                return new LandingConfigModel();
            }
        }

        public static string? Serialize(LandingConfigModel? config)
        {
            if (config == null)
                return null;

            var hasData =
                (config.HeroSlides?.Count ?? 0) > 0
                || HasBannerData(config.PageBanners)
                || (config.Stats?.Count ?? 0) > 0;

            return hasData ? JsonSerializer.Serialize(config, JsonOptions) : null;
        }

        public static LandingConfigModel FromEntity(BlogEntity entity)
        {
            var config = Parse(entity.LandingConfigJson);
            return config ?? new LandingConfigModel();
        }

        private static bool HasBannerData(PageBannersModel? banners)
        {
            if (banners == null) return false;
            return !string.IsNullOrWhiteSpace(banners.HomeCta)
                || !string.IsNullOrWhiteSpace(banners.Nosotros)
                || !string.IsNullOrWhiteSpace(banners.Servicios)
                || !string.IsNullOrWhiteSpace(banners.Locales)
                || !string.IsNullOrWhiteSpace(banners.Contacto)
                || !string.IsNullOrWhiteSpace(banners.Presupuestador)
                || !string.IsNullOrWhiteSpace(banners.Login)
                || !string.IsNullOrWhiteSpace(banners.Registro);
        }
    }
}
