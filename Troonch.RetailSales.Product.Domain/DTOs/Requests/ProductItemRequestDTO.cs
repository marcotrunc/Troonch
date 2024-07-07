
using System;
using System.Globalization;
using Troonch.Domain.Base.Enums;

namespace Troonch.RetailSales.Product.Domain.DTOs.Requests
{
    public class ProductItemRequestDTO
    {
        public Guid? Id { get; set; } = Guid.Empty;
        public Guid ProductId { get; set; }
        public Guid ProductSizeOptionId { get; set; }
        public string ProductColor { get; set; }
        public CurrencyBase Currency { get; set; } = CurrencyBase.EUR;
        private decimal _originalPrice { get; set; } = decimal.Zero;
        public string OriginalPrice
        {
            get
            {

                string originalPriceString = Convert.ToString(_originalPrice, CultureInfo.InvariantCulture);
                return originalPriceString;
            }
            set
            {
                value = value.Replace(',', '.');

                bool isParsed = decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal originalPriceParsed);
                if(isParsed)
                {
                    _originalPrice = originalPriceParsed;
                }
                else
                {
                    _originalPrice = decimal.MinusOne;
                }
            }
        }
        private decimal _salePrice { get; set; } = decimal.Zero;

        public string SalePrice
        {
            get
            {

                string salePriceString = Convert.ToString(_salePrice, CultureInfo.InvariantCulture);
                return salePriceString;
            }
            set
            {
                value = value.Replace(',', '.');

                bool isParsed = decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal salePriceParsed);
                if (isParsed)
                {
                    _salePrice = salePriceParsed;
                }
                else
                {
                    _salePrice = decimal.MinusOne;
                }
            }
        }

        public string Barcode { get; set; } = string.Empty;
        public int QuantityAvailable { get; set; }
    }
}
