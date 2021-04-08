using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dis
{
    class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public List<SaleDetail> ListOfProducts { get; set; }
    }

    class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public List<SaleDetail> OrderList { get; set; }
    }

    class SaleDetail
    {
        public int id { get; set; }
        public Order SaleOrdDet { get; set; }
        public Product SaleProdDet { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }

    class SaleOrdDetContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=InClassAssignment10;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (SaleOrdDetContext context = new SaleOrdDetContext())
            {
                context.Database.EnsureCreated();



                Order order1 = new Order { CustomerName = "Shar" };
                Order order2 = new Order { CustomerName = "chandana" };
                Order order3 = new Order { CustomerName = "sindhu" };
                Product product1 = new Product
                {
                    ProductName = "cerial",
                    ProductPrice = 21,
                };
                Product product2 = new Product
                {
                    ProductName = "stick",
                    ProductPrice = 3,
                };
                Product product3 = new Product
                {
                    ProductName = "Toy",
                    ProductPrice = 5,
                };

                SaleDetail SaleDetail1 = new SaleDetail
                {
                    SaleOrdDet = order1,
                    SaleProdDet = product1,
                    Quantity = 3,
                    OrderDate = DateTime.Now
                };
                SaleDetail SaleDetail2 = new SaleDetail
                {
                    SaleOrdDet = order2,
                    SaleProdDet = product1,
                    Quantity = 6,
                    OrderDate = DateTime.Now
                };
                SaleDetail SaleDetail3 = new SaleDetail
                {
                    SaleOrdDet = order3,
                    SaleProdDet = product2,
                    Quantity = 8,
                    OrderDate = DateTime.Now
                };
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);
                context.Products.Add(product1);
                context.Products.Add(product2);
                context.Products.Add(product3);
                context.SaleDetails.Add(SaleDetail1);
                context.SaleDetails.Add(SaleDetail2);
                context.SaleDetails.Add(SaleDetail3);

                context.SaveChanges();

                //Taking all the orders where a particular product is sold
                IQueryable<SaleDetail> SaleProductOrders = context.SaleDetails
                    .Include(c => c.SaleProdDet).Where(c => c.SaleProdDet == product1);

                //Gets a list of all orders where the selected product is sold
                List<SaleDetail> SelectedSaleProductOrders = SaleProductOrders.ToList();

                //For orders where a given product sold maximum
                SaleDetail maxsaleproducts = context.SaleDetails
                  .Include(c => c.SaleProdDet).Where(c => c.SaleProdDet == product1).OrderByDescending(x => x.Quantity).FirstOrDefault();

            }




        }
    }
}