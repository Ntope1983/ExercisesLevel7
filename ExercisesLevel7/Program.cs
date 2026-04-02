namespace ProductApp
{
    // ========================
    // Model
    // ========================
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

    // ========================
    // Repository Interface
    // ========================
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product? GetById(int id);
        void Add(Product product);
        void DeleteById(int id);
    }

    // ========================
    // InMemory Repository
    // ========================
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        public IEnumerable<Product> GetProducts() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.productId == id);

        public void Add(Product product)
        {
            _products.Add(product);
            Console.WriteLine($"The product {product.productName} with id {product.productId} has been added.");
        }

        public void DeleteById(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
                Console.WriteLine($"The product with id {id} has been removed.");
            }
            else
            {
                Console.WriteLine($"The product with id {id} was not found.");
            }
        }
    }

    // ========================
    // Service Layer Interface
    // ========================
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int id);
        void AddProduct(string name, decimal price);
        void DeleteProduct(int id);
    }

    // ========================
    // Service Layer Implementation
    // ========================
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Product> GetAllProducts() => _repository.GetProducts();

        public Product? GetProductById(int id) => _repository.GetById(id);

        public void AddProduct(string name, decimal price)
        {
            int id = _repository.GetProducts().Any()
                ? _repository.GetProducts().Max(p => p.productId) + 1
                : 1;

            var product = new Product(id, name, price);
            _repository.Add(product);
        }

        public void DeleteProduct(int id) => _repository.DeleteById(id);
    }

    // ========================
    // Program
    // ========================
    public class Program
    {
        static void Main(string[] args)
        {
            // Δημιουργία repository και service
            IProductRepository repository = new InMemoryProductRepository();
            IProductService productService = new ProductService(repository);

            // Προσθήκη αρχικών προϊόντων
            productService.AddProduct("Tv Tesla", 125m);
            productService.AddProduct("Xiaomi Redmi 8 Pro", 125m);
            productService.AddProduct("Xbox 2025", 300m);

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
                    Console.WriteLine("Please give an integer 1-4");
                    choice = Console.ReadLine();
                }

                if (menuValue == 4) break;

                switch (menuValue)
                {
                    case 1: // Get Products
                        foreach (var product in productService.GetAllProducts())
                        {
                            Console.WriteLine($"Id: {product.productId}, Name: {product.productName}, Price: {product.productPrice} Euro");
                        }
                        break;

                    case 2: // Add Product
                        Console.WriteLine("Enter product name:");
                        string name = Console.ReadLine();

                        Console.WriteLine("Enter product price:");
                        decimal price;
                        while (!decimal.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Please enter a valid decimal number for price.");
                        }

                        productService.AddProduct(name, price);
                        break;

                    case 3: // Delete Product
                        Console.WriteLine("Enter the id of the product to delete:");
                        int idToDelete;
                        while (!int.TryParse(Console.ReadLine(), out idToDelete))
                        {
                            Console.WriteLine("Please enter a valid integer.");
                        }

                        productService.DeleteProduct(idToDelete);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1-4.");
                        break;
                }
            }
        }
    }
}