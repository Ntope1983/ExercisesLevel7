public class Program
{
    private static void Main(string[] args)
    {
        Exercise29();

    }
    public static void Exercise29()
    {
        Product tv = new Product(1, "Tv Tesla", 125m);
        Product mobile = new Product(2, "Xiaomi Redmi 8 Pro", 125m);
        Product xbox = new Product(3, "Xbox 2025", 300);
        InMemoryProductRepository _inMemoryProductRepository = new InMemoryProductRepository();
        _inMemoryProductRepository.Add(tv);
        _inMemoryProductRepository.Add(mobile);
        _inMemoryProductRepository.Add(xbox);
        string menu = "Please select an operation:\n" +
                      "1. Get Products\n" +
                      "2. Add Product\n" +
                      "3. Delete Product By id\n" +
                      "4. Exit\n";


        int menuValue;

        while (true)
        {
            Console.WriteLine(menu);
            string choice = Console.ReadLine();

            while (!int.TryParse(choice, out menuValue))

            {
                Console.WriteLine(menu);
                Console.WriteLine("Please Give an Integer 1-4");
                choice = Console.ReadLine();

            }
            if (menuValue == 4)
            {
                break;
            }
            else if (menuValue == 1)
            {

                foreach (Product product in _inMemoryProductRepository.GetProducts())
                {
                    Console.WriteLine($"Id:{product.productId} Name:{product.productName} Price:{product.productPrice} Euro");
                }
            }
            else if (menuValue == 2)
            {

                Console.WriteLine($"Pls give the name from the Product");
                string ProductName = Console.ReadLine();
                Console.WriteLine($"Pls give the Price from the Product");
                string StringProductPrice = Console.ReadLine();
                decimal ProductPrice;
                decimal.TryParse(StringProductPrice, out ProductPrice);
                int id;
                var products = _inMemoryProductRepository.GetProducts();

                if (products.Any())
                {
                    id = products.Max(p => p.productId) + 1;
                }
                else
                {
                    id = 1;
                }
                _inMemoryProductRepository.Add(new Product(id, ProductName, ProductPrice));
            }
            else if (menuValue == 3)
            {
                Console.WriteLine($"Pls give the id from the Product u want to delete");
                string idStringToDelete = Console.ReadLine();
                int idToDelete;
                while (!int.TryParse(idStringToDelete, out idToDelete))
                {
                    Console.WriteLine($"Pls give an Integer");
                    idStringToDelete = Console.ReadLine();
                }

                _inMemoryProductRepository.DeleteById(idToDelete);
            }
        }

    }

    static int ReadNumber(string message)
    {
        int number;
        string input;

        Console.WriteLine(message);
        input = Console.ReadLine();

        while (!int.TryParse(input, out number))
        {
            Console.WriteLine("Please give again a valid number");
            input = Console.ReadLine();
        }

        return number;
    }
    //Model of Product
    public class Product
    {
        public Product(int id, string name, decimal price)
        {
            productId = id;
            productName = name;
            productPrice = price;
        }
        public int productId { get; set; }
        public string productName { get; set; }
        public decimal productPrice { get; set; }
    }
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        public Product GetById(int id);
        public void Add(Product product);
        public void DeleteById(int id);
    }
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public IEnumerable<Product> GetProducts()
        {
            return _products;
        }
        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.productId == id);

        }
        public void Add(Product product)
        {
            _products.Add(product);
            Console.WriteLine($"The product {product.productName} with   has been Added InMemoryProductRepository");
        }
        public void DeleteById(int id)
        {
            Product removeproduct = this.GetById(id);
            if (removeproduct != null)
            {
                _products.Remove(removeproduct);
                Console.WriteLine($"The product with id:{id} has been removed");
            }
            else
            {
                Console.WriteLine($"The product with id:{id} is not in InMemoryProductRepository");
            }
        }

    }
}