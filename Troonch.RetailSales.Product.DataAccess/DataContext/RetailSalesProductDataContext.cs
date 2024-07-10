using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Troonch.Sales.Domain.Entities;
using static System.Reflection.Metadata.BlobBuilder;

namespace Troonch.Sales.DataAccess
{
    public class RetailSalesProductDataContext : DbContext
    {
        public RetailSalesProductDataContext(DbContextOptions<RetailSalesProductDataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public  DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductGender> ProductGenders { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<ProductSizeOption> ProductSizeOptions { get; set; }
        public DbSet<ProductSizeType> ProductSizeTypes { get; set; }
        public DbSet<ProductGenderSizeTypeLookup> ProductGenderSizeTypeLookup { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<ProductTag> ProductTagLookup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
           
            #region ProductGender Seeding
            var productGenders = new List<ProductGender>();

            var productGendersName = new List<string>()
            {
                "Uomo",
                "Donna",
                "Bambino",
                "Bambina",
                "Neonato"
            };

            foreach (var gender in productGendersName)
            {
                var productGende = new ProductGender();
                productGende.Id = Guid.NewGuid();
                productGende.Name = gender;
                productGende.CreatedOn = DateTime.UtcNow;
                productGende.UpdatedOn = DateTime.UtcNow;
                productGenders.Add(productGende);
            }

            modelBuilder.Entity<ProductGender>().HasData(productGenders);
            #endregion

            #region ProductMaterials Seeding
            var productMaterials = new List<ProductMaterial>();


            var cotton = new ProductMaterial();
            cotton.Id = Guid.NewGuid();
            cotton.Value = "Cotone";
            cotton.CreatedOn = DateTime.UtcNow;
            cotton.UpdatedOn = DateTime.UtcNow;
            productMaterials.Add(cotton);


            var wool = new ProductMaterial();
            wool.Id = Guid.NewGuid();
            wool.Value = "Lana";
            wool.CreatedOn = DateTime.UtcNow;
            wool.UpdatedOn = DateTime.UtcNow;
            productMaterials.Add(wool);

            var silk = new ProductMaterial();
            silk.Id = Guid.NewGuid();
            silk.Value = "Seta";
            silk.CreatedOn = DateTime.UtcNow;
            silk.UpdatedOn = DateTime.UtcNow;
            productMaterials.Add(silk);

            var linen = new ProductMaterial();
            linen.Id = Guid.NewGuid();
            linen.Value = "Lino";
            linen.CreatedOn = DateTime.UtcNow;
            linen.UpdatedOn = DateTime.UtcNow;
            productMaterials.Add(linen);

            var hemp = new ProductMaterial();
            hemp.Id = Guid.NewGuid();
            hemp.Value = "Canapa";
            hemp.CreatedOn = DateTime.UtcNow;
            hemp.UpdatedOn = DateTime.UtcNow;
            productMaterials.Add(hemp);

            var bamboo = new ProductMaterial();
            bamboo.Id = Guid.NewGuid();
            bamboo.Value = "Bamboo";
            bamboo.CreatedOn = DateTime.UtcNow;
            bamboo.UpdatedOn = DateTime.UtcNow;
            productMaterials.Add(bamboo);

            modelBuilder.Entity<ProductMaterial>().HasData(productMaterials);
            #endregion

            #region ProductSizeType Seeding

            var productSizeTypes = new List<ProductSizeType>();

            var productSizeNames = new List<string>
            {
                "Abiti - Completi - Vestiti - Bambino/a",
                "Abiti - Vestiti - Uomo/Donna",
                "Accessori",
                "Scarpe Bambino/a",
                "Scarpe Uomo/Donna"
            };

            foreach (var productType in productSizeNames)
            {
                var productSizeType = new ProductSizeType();
                productSizeType.Id = Guid.NewGuid();
                productSizeType.Name = productType;
                productSizeType.CreatedOn = DateTime.UtcNow;
                productSizeType.UpdatedOn = DateTime.UtcNow;
                productSizeTypes.Add(productSizeType);
            }

            modelBuilder.Entity<ProductSizeType>().HasData(productSizeTypes);
            #endregion

            #region ProductSizeOption Seeding

            var options = new List<ProductSizeOption>();
            var productSizeOptionAcc = new ProductSizeOption();
            productSizeOptionAcc.Id = Guid.NewGuid();
            productSizeOptionAcc.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Accessori")).First().Id;
            productSizeOptionAcc.Value = "Taglia Unica";
            productSizeOptionAcc.Sort = 1;
            productSizeOptionAcc.CreatedOn = DateTime.UtcNow;
            productSizeOptionAcc.UpdatedOn = DateTime.UtcNow;
            options.Add(productSizeOptionAcc);


            int index = 1;

            for (int i = 35; i <= 46; i++)
            {
                var productSizeTypeOption = new ProductSizeOption();
                productSizeTypeOption.Id = Guid.NewGuid();
                productSizeTypeOption.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Scarpe Uomo/Donna")).First().Id;
                productSizeTypeOption.Value = i.ToString();
                productSizeTypeOption.Sort = index;
                productSizeTypeOption.CreatedOn = DateTime.UtcNow;
                productSizeTypeOption.UpdatedOn = DateTime.UtcNow;
                options.Add(productSizeTypeOption);

                index++;
            }

            index = 1;


            for(int i = 14; i <= 34; i++)
            {
                var productSizeTypeOption = new ProductSizeOption();
                productSizeTypeOption.Id = Guid.NewGuid();
                productSizeTypeOption.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Scarpe Bambino/a")).First().Id;
                productSizeTypeOption.Value= i.ToString();
                productSizeTypeOption.Sort = index;
                productSizeTypeOption.CreatedOn = DateTime.UtcNow;
                productSizeTypeOption.UpdatedOn = DateTime.UtcNow;
                options.Add(productSizeTypeOption);

                index++;
            }

            index = 1;


            var dressSize = new List<string>
            {
                "XXS",
                "XS",
                "S",
                "M",
                "L",
                "XL",
                "XXL",
                "XXXL",
            };

            foreach (var size in dressSize) 
            { 
                var productSizeTypeOption = new ProductSizeOption();
                productSizeTypeOption.Id = Guid.NewGuid();
                productSizeTypeOption.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Abiti - Vestiti - Uomo/Donna")).First().Id;
                productSizeTypeOption.Value = size;
                productSizeTypeOption.Sort = index;
                productSizeTypeOption.CreatedOn = DateTime.UtcNow;
                productSizeTypeOption.UpdatedOn = DateTime.UtcNow;
                options.Add(productSizeTypeOption);

                index++;
            }
            
            index = 1;

            var dressKidsSize = new List<string>
            {
                "4M",
                "5M",
                "6M",
                "7M",
                "8M",
                "10M",
                "12M",
                "16M",
                "24M",
                "3Y/A",
                "4Y/A",
                "5Y/A",
                "6Y/A",
                "7Y/A",
                "8Y/A",
                "9Y/A",
                "10Y/A",
                "11Y/A",
                "12Y/A",
                "13Y/A",
                "14Y/A",
                "15Y/A",
                "16Y/A",
            };

            foreach (var size in dressKidsSize)
            {
                var productSizeTypeOption = new ProductSizeOption();
                productSizeTypeOption.Id = Guid.NewGuid();
                productSizeTypeOption.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Abiti - Completi - Vestiti - Bambino/a")).First().Id;
                productSizeTypeOption.Value = size;
                productSizeTypeOption.Sort = index;
                productSizeTypeOption.CreatedOn = DateTime.UtcNow;
                productSizeTypeOption.UpdatedOn = DateTime.UtcNow;
                options.Add(productSizeTypeOption);

                index++;
            }

            modelBuilder.Entity<ProductSizeOption>().HasData(options);

            #endregion

            #region ProductCategory Seeding (Kids)
            var productCategories = new List<ProductCategory>();

            var productCategoryKids = new List<string>()
            {
                "Abiti - Completi",
                "Giubbino",
                "Felpa",
                "Jeans",
                "Cardigan",
                "Pigiama",
                "Top",
                "Maglie",
                "Pantaloni",
                "Salopette",
                "Camicia",
                "Shorts",
            };

            foreach (var category in productCategoryKids)
            {
                var productCategory = new ProductCategory();
                productCategory.Id = Guid.NewGuid();
                productCategory.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Abiti - Completi - Vestiti - Bambino/a")).First().Id;
                productCategory.Name = category;
                productCategory.CreatedOn = DateTime.UtcNow;
                productCategory.UpdatedOn = DateTime.UtcNow;
                productCategories.Add(productCategory);
            }
            #endregion

            #region ProductCategory Seeding (Adult)

                var productCategoryAdults = new List<string>()
                {
                "Giacche | Trench",
                "Blazer | Gilet",
                "Vestiti",
                "Body | Top",
                "Camicie",
                "Magliette",
                "Felpe",
                "Gonne",
                "Shorts",
                "Pantaloni",
                "Jeans",
                "Intimo",
                "Costumi da Bagno",
                "Polo",
                "Maglioncini",
            };

            foreach (var category in productCategoryAdults)
            {
                var productCategory = new ProductCategory(); 
                productCategory.Id = Guid.NewGuid();
                productCategory.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Abiti - Vestiti - Uomo/Donna")).First().Id;
                productCategory.Name = category;
                productCategory.CreatedOn = DateTime.UtcNow;
                productCategory.UpdatedOn = DateTime.UtcNow;
                productCategories.Add(productCategory);
            }


            var productCategoriesAccessories = new List<string>()
            {
                "Borse",
                "Gioielli",
                "Occhiali da Sole",
                "Cappelli",
                "Foulard",
                "Cinture",
                "Orologi",
                "Portafogli",
                "Sciarpe",
                "Guanti",
                "Ombrelli"
            };

            foreach (var category in productCategoriesAccessories)
            {
                var productCategory = new ProductCategory();
                productCategory.Id = Guid.NewGuid();
                productCategory.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Accessori")).First().Id;
                productCategory.Name = category;
                productCategory.CreatedOn = DateTime.UtcNow;
                productCategory.UpdatedOn = DateTime.UtcNow;
                productCategories.Add(productCategory);
            }


            var productCategoriesShoesAdults = new List<string>()
            {
                "Sneakers",
                "Sandali",
                "Tacchi",
                "Ciabatte",
                "Stivaletti",
                "Stivali",
                "Ballerine",
                "Mare",
                "Mocassini",
                "Basse",
                "Cerimonia"
            };

            foreach (var category in productCategoriesShoesAdults)
            {
                var productCategory = new ProductCategory();
                productCategory.Id = Guid.NewGuid();
                productCategory.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Scarpe Uomo/Donna")).First().Id;
                productCategory.Name = category;
                productCategory.CreatedOn = DateTime.UtcNow;
                productCategory.UpdatedOn = DateTime.UtcNow;
                productCategories.Add(productCategory);
            }

            var productCategoriesShoesKids = new List<string>()
            {
                 "Sneakers",
                 "Sandali",
                 "Ciabatte",
                 "Stivaletti",
                 "Stivali",
                 "Mare",
                 "Sportive",
                 "Cerimonia"
            };

            foreach(var category in productCategoriesShoesKids)
            {
                var productCategory = new ProductCategory();
                productCategory.Id = Guid.NewGuid();
                productCategory.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Scarpe Bambino/a")).First().Id;
                productCategory.Name = category;
                productCategory.CreatedOn = DateTime.UtcNow;
                productCategory.UpdatedOn = DateTime.UtcNow;
                productCategories.Add(productCategory);
            }

            modelBuilder.Entity<ProductCategory>().HasData(productCategories);
            #endregion
        }
    }
}