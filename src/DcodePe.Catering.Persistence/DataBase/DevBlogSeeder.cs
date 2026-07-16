using DcodePe.Catering.Application.DataBase.Blog;
using DcodePe.Catering.Application.DataBase.Blog.Models;
using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace DcodePe.Catering.Persistence.DataBase
{
    public static class DevBlogSeeder
    {
        public static void EnsureBlog(DataBaseService db)
        {
            PatchMissingContent(db);

            if (db.Blog.Any(b => b.Estado == true))
                return;

            var existing = db.Blog.FirstOrDefault(b => b.BlogID == 1);
            if (existing != null)
            {
                existing.Estado = true;
                existing.Titulo ??= "¿Quiénes somos?";
                existing.Descripcion ??= "En Ayllu Eventos creamos experiencias memorables para cada celebración.";
                existing.Resumen ??= "Creamos experiencias memorables para bodas, quinceaños, eventos corporativos y celebraciones familiares con calidad, creatividad y compromiso.";
                existing.MisionTitulo ??= "Misión";
                existing.MisionTexto ??= "Brindar experiencias gastronómicas y de organización excepcionales, superando las expectativas de cada cliente en sus celebraciones más importantes.";
                existing.VisionTitulo ??= "Visión";
                existing.VisionTexto ??= "Ser la empresa referente en eventos y catering del Perú, reconocida por la calidad, innovación y calidez de nuestro servicio.";
                db.SaveChanges();
                return;
            }

            db.Blog.Add(new BlogEntity
            {
                BlogID = 1,
                Titulo = "¿Quiénes somos?",
                Descripcion =
                    "En Ayllu Eventos nuestros inicios fueron muy humildes. Comenzamos vendiendo helados en las playas pequeñas de Lima y poco a poco nos expandimos a playas más grandes.",
                Resumen =
                    "Creamos experiencias memorables para bodas, quinceaños, eventos corporativos y celebraciones familiares con calidad, creatividad y compromiso.",
                MisionTitulo = "Misión",
                MisionTexto =
                    "Brindar experiencias gastronómicas y de organización excepcionales, superando las expectativas de cada cliente en sus celebraciones más importantes.",
                VisionTitulo = "Visión",
                VisionTexto =
                    "Ser la empresa referente en eventos y catering del Perú, reconocida por la calidad, innovación y calidez de nuestro servicio.",
                Imagenes = "/uploads/blog/equipo_ayllu.jpeg",
                ValoresJson =
                    """[{"Nombre":"Compromiso","Descripcion":"Trabajamos con pasión en cada evento como si fuera el nuestro."},{"Nombre":"Creatividad","Descripcion":"Diseñamos experiencias únicas adaptadas a cada cliente."},{"Nombre":"Calidad","Descripcion":"Ofrecemos lo mejor en gastronomía, decoración y organización."}]""",
                UsuarioCreacion = "seed",
                FechaCreacion = DateTime.UtcNow,
                Estado = true,
            });

            db.SaveChanges();
        }

        private static void PatchMissingContent(DataBaseService db)
        {
            foreach (var blog in db.Blog.Where(b => b.Estado == true))
            {
                var changed = false;

                if (string.IsNullOrWhiteSpace(blog.Resumen))
                {
                    blog.Resumen = "Creamos experiencias memorables para bodas, quinceaños, eventos corporativos y celebraciones familiares con calidad, creatividad y compromiso.";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(blog.MisionTitulo))
                {
                    blog.MisionTitulo = "Misión";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(blog.MisionTexto))
                {
                    blog.MisionTexto = "Brindar experiencias gastronómicas y de organización excepcionales, superando las expectativas de cada cliente en sus celebraciones más importantes.";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(blog.VisionTitulo))
                {
                    blog.VisionTitulo = "Visión";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(blog.VisionTexto))
                {
                    blog.VisionTexto = "Ser la empresa referente en eventos y catering del Perú, reconocida por la calidad, innovación y calidez de nuestro servicio.";
                    changed = true;
                }

                if (string.IsNullOrWhiteSpace(blog.LandingConfigJson) || blog.LandingConfigJson.Contains("equipo_ayllu.jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    blog.LandingConfigJson = BuildDefaultLandingConfigJson(blog);
                    changed = true;
                }

                if (changed)
                    db.SaveChanges();
            }
        }

        private static string BuildDefaultLandingConfigJson(BlogEntity blog)
        {
            var images = blog.ImagenesUrls?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList() ?? new List<string>();
            var heroImage = images.FirstOrDefault() ?? "/uploads/blog/equipo_ayllu.jpeg";
            var slideImages = images.Count >= 3
                ? images.Take(3).ToList()
                : new List<string> { heroImage, heroImage, heroImage };

            return LandingConfigMapper.Serialize(new LandingConfigModel
            {
                HeroSlides =
                [
                    new() { Tag = "Matrimonios", Title = "Hacemos de tu boda un recuerdo inolvidable", Subtitle = "Cada detalle pensado para contar tu historia de amor con elegancia y calidez.", ImageUrl = slideImages[0], Orden = 0, Activo = true },
                    new() { Tag = "Eventos corporativos", Title = "Experiencias que inspiran y conectan", Subtitle = "Producción integral para empresas que buscan impacto, orden y excelencia.", ImageUrl = slideImages[1], Orden = 1, Activo = true },
                    new() { Tag = "Celebraciones", Title = "Momentos únicos, recuerdos para siempre", Subtitle = "Quinceañeros, aniversarios y fiestas con la magia que tu evento merece.", ImageUrl = slideImages[2], Orden = 2, Activo = true },
                ],
                Stats =
                [
                    new() { Value = "+500", NumericValue = 500, Prefix = "+", Suffix = "", Label = "Eventos realizados", Animate = true },
                    new() { Value = "+15", NumericValue = 15, Prefix = "+", Suffix = "", Label = "Años de experiencia", Animate = true },
                    new() { Value = "100%", Label = "Compromiso personal", Animate = false },
                    new() { Value = "360°", Label = "Producción integral", Animate = false },
                ],
            });
        }
    }
}
