using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public DbSet<ProductGenderCategoryLookup> ProductGenderCategoryLookup { get; set; }
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
                "Cardigan",
                "Pigiama",
                "Top",
                "Salopette",
            };

            var productGenderCategoryList = new List<ProductGenderCategoryLookup>();

            foreach (var category in productCategoryKids)
            {
                var productCategory = new ProductCategory();
                productCategory.Id = Guid.NewGuid();
                productCategory.ProductSizeTypeId = productSizeTypes.Where(pst => pst.Name.Contains("Abiti - Completi - Vestiti - Bambino/a")).First().Id;
                productCategory.Name = category;
                productCategory.CreatedOn = DateTime.UtcNow;
                productCategory.UpdatedOn = DateTime.UtcNow;
                productCategories.Add(productCategory);

                var productGenderBoyCategory = new ProductGenderCategoryLookup();
                productGenderBoyCategory.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Bambino")).First().Id;
                productGenderBoyCategory.ProductCategoryId = productCategory.Id;
                productGenderCategoryList.Add(productGenderBoyCategory);

                var productGenderGirlCategory = new ProductGenderCategoryLookup();
                productGenderGirlCategory.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Bambina")).First().Id; ;
                productGenderGirlCategory.ProductCategoryId = productCategory.Id;
                productGenderCategoryList.Add(productGenderGirlCategory);
            }
            #endregion

            #region ProductCategory Seeding (Adult)
                var productCategoryAdultsMan = new List<string>()
                {
                    "Giacche",
                    "Gilet",
                    "Camicie",
                    "Magliette",
                    "Felpe",
                    "Shorts",
                    "Pantaloni",
                    "Jeans",
                    "Intimo",
                    "Costumi da Bagno",
                    "Polo",
                    "Maglioncini",
                };

                var productCategoryAdults = new List<string>()
                {
                "Trench",
                "Blazer",
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

                var productGenderWomanCategory = new ProductGenderCategoryLookup();
                productGenderWomanCategory.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Donna")).First().Id; 
                productGenderWomanCategory.ProductCategoryId = productCategory.Id;
                productGenderCategoryList.Add(productGenderWomanCategory);

                if(productCategoryAdultsMan.Any(categoryMan => categoryMan == category))
                {
                    var productGenderManCategory = new ProductGenderCategoryLookup();
                    productGenderManCategory.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Uomo")).First().Id;
                    productGenderManCategory.ProductCategoryId = productCategory.Id;
                    productGenderCategoryList.Add(productGenderManCategory);
                }
            }

            var duplicates = productGenderCategoryList
            .GroupBy(x => new { x.ProductGenderId, x.ProductCategoryId })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
            if (duplicates.Any())
            {
                Console.WriteLine("1");
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

                foreach(var gender in  productGenders)
                {
                    if (gender.Name.ToLower().Contains("neonato"))
                    {
                        continue;
                    }

                    var productGenderAccessories = new ProductGenderCategoryLookup();
                    productGenderAccessories.ProductGenderId = gender.Id ;
                    productGenderAccessories.ProductCategoryId = productCategory.Id;
                    productGenderCategoryList.Add(productGenderAccessories);
                }
            }


             duplicates = productGenderCategoryList
            .GroupBy(x => new { x.ProductGenderId, x.ProductCategoryId })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
            if (duplicates.Any())
            {
                Console.WriteLine("2");
            }


            var productCategoriesShoesAdults = new List<string>()
            {
                "Sneakers",
                "Sandali",
                "Tacchi",
                "Stivaletti",
                "Stivali",
                "Ballerine",
                "Mocassini",
                "Basse",
                "Cerimonia"
            };

            var productCategoriesOnlyForWoman = new List<string>()
            {
                "Ballerine",
                "Tacchi",
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



                var productGenderShoesWoman = new ProductGenderCategoryLookup();
                productGenderShoesWoman.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Donna")).First().Id;
                productGenderShoesWoman.ProductCategoryId = productCategory.Id;
                productGenderCategoryList.Add(productGenderShoesWoman);

                if(!productCategoriesOnlyForWoman.Any(pcow => pcow.ToLower() == category.ToLower()))
                {
                    var productGenderShoesMan = new ProductGenderCategoryLookup();
                    productGenderShoesMan.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Uomo")).First().Id;
                    productGenderShoesMan.ProductCategoryId = productCategory.Id;
                    productGenderCategoryList.Add(productGenderShoesMan);
                }

            }

            duplicates = productGenderCategoryList
            .GroupBy(x => new { x.ProductGenderId, x.ProductCategoryId })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
            if (duplicates.Any())
            {
                Console.WriteLine("3");
            }

            var productCategoriesShoesKids = new List<string>()
            {
                 "Ciabatte",
                 "Sportive",
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

                var productGenderBoyCategory = new ProductGenderCategoryLookup();
                productGenderBoyCategory.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Bambino")).First().Id;
                productGenderBoyCategory.ProductCategoryId = productCategory.Id;
                productGenderCategoryList.Add(productGenderBoyCategory);

                var productGenderGirlCategory = new ProductGenderCategoryLookup();
                productGenderGirlCategory.ProductGenderId = productGenders.Where(pg => pg.Name.Contains("Bambina")).First().Id; ;
                productGenderGirlCategory.ProductCategoryId = productCategory.Id;
                productGenderCategoryList.Add(productGenderGirlCategory);

            }


            modelBuilder.Entity<ProductCategory>().HasData(productCategories);
            modelBuilder.Entity<ProductGenderCategoryLookup>().HasData(productGenderCategoryList);
            #endregion
        }
    }
}