namespace NtigraDemoMVC.Models
{
    public class PurchaseItemViewModel : ProductViewModel
    {
        public bool IsSelected { get; set; }
        public int Quantity { get; set; }
        public double TotalDiscount { get => Quantity * Discount;  }
        public double TotalPrice { get => Quantity * Price; }
        public double PriceAfterDiscount { get => TotalPrice - TotalDiscount; }

    }

    public class CartViewModel
    {
        public List<PurchaseItemViewModel>? PurchasedProducts { get; set; }

        public bool IsEligibleForDiscount { get; set; }
        public double TotalDiscount { get => (PurchasedProducts != null ? PurchasedProducts.Sum(s => s.TotalDiscount) : 0); }
        public double TotalPrice { get => (PurchasedProducts != null ? PurchasedProducts.Sum(s => s.TotalPrice) : 0); }
        public double PriceAfterDiscount { get => TotalPrice - TotalDiscount; }
    }
   
}
