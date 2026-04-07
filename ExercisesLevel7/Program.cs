using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using static Product;
// ========================
// Model
// ========================


public class Middleware
{
    private readonly Func<Task> _next;

    public Middleware(Func<Task> next)
    {
        _next = next;
    }

    public async Task InvokeAsync()
    {
        Console.WriteLine("Before");      // Εκτύπωση πριν την εκτέλεση
        await _next();                    // Εκτέλεση του delegate
        Console.WriteLine("After");       // Εκτύπωση μετά την εκτέλεση
    }
}

// Παράδειγμα χρήσης
// ========================
// Program
// ========================
public class Program
{
    public static void Main()
    {
        InMemoryProductRepository MyRepository = new InMemoryProductRepository();
        string jsonData = File.ReadAllText(@"C:\Users\g_pol\source\repos\C#\ExercisesLevel7\ExercisesLevel7\deserializeProduct.json");
        var productList = JsonSerializer.Deserialize<List<Product>>(jsonData);
        foreach (Product item in productList)
        {
            Console.WriteLine(item.ProductName);
        }
    }

}
public class Product
{

    public Product() { }
    public Product(int id, string name, decimal price)
    {
        ProductId = id;
        ProductName = name;
        ProductPrice = price;
    }
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("productName")]
    public string ProductName { get; set; }
    [JsonPropertyName("productPrice")]
    public decimal ProductPrice { get; set; }



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

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.ProductId == id);

        public void Add(Product product)
        {
            _products.Add(product);
            Console.WriteLine($"The product {product.ProductName} with id {product.ProductId} has been added.");
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
                ? _repository.GetProducts().Max(p => p.ProductId) + 1
                : 1;

            var product = new Product(id, name, price);
            _repository.Add(product);
        }

        public void DeleteProduct(int id) => _repository.DeleteById(id);
    }




    static void Exercise32()
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
                        Console.WriteLine($"Id: {product.ProductId}, Name: {product.ProductName}, Price: {product.ProductPrice} Euro");
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
    // ============================================
    // SerializeToJson into a file with json Data
    // ============================================
    public static void SerializeToJson()
    {

        var people = new List<Person>
        {
            new Person("Giannis", "Polidoras", 42, "giannis.polidoras@example.com"),
            new Person("Maria", "Papadopoulou", 35, "maria.papadopoulou@example.com"),
            new Person("Nikos", "Kostopoulos", 28, "nikos.kostopoulos@example.com"),
            new Person("Eleni", "Georgiou", 31, "eleni.georgiou@example.com"),
            new Person("Dimitris", "Alexandris", 45, "dimitris.alexandris@example.com")
        };
        string json = JsonSerializer.Serialize(people);
        File.WriteAllText(@"C:\Users\g_pol\source\repos\C#\ExercisesLevel7\ExercisesLevel7\serialize.json", json);

    }
    // ============================================
    // DeSerializeToJson from a file with json Data
    // ============================================
    public static void DeSerializeFromJson()
    {
        string jsonData = File.ReadAllText(@"C:\Users\g_pol\source\repos\C#\ExercisesLevel7\ExercisesLevel7\deserialize.json");
        var personList = JsonSerializer.Deserialize<List<Person>>(jsonData);
    }

    public static void DeSerializeProductsFromJson()
    {
        string jsonData = File.ReadAllText(@"C:\Users\g_pol\source\repos\C#\ExercisesLevel7\ExercisesLevel7\deserialize.json");
        var personList = JsonSerializer.Deserialize<List<Product>>(jsonData);
    }
}

// ========================
// Person Class
// ========================
public class Person
{

    [JsonPropertyName("FirstName")]
    public string personName { get; set; }

    [JsonPropertyName("LastName")]
    public string personSurName { get; set; }

    [JsonPropertyName("Age")]
    public int personAge { get; set; }

    [JsonPropertyName("Email")]
    public string personEmail { get; set; }
    public Person() { }
    public Person(string FirstName, string LastName, int Age, string Email)
    {
        personName = FirstName;
        personSurName = LastName;
        personAge = Age;
        personEmail = Email;
    }
    // ========================
    // Check Validation Method Email
    // ========================

    public static (bool, string) ValidateEmail(string email)
    {
        // Έλεγχος μήκους
        if (email.Length > 254)
            return (false, "Το email είναι πολύ μεγάλο");

        // Regex pattern
        string pattern = @"^(?!.*\.\.)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        if (!Regex.IsMatch(email, pattern))
            return (false, "Μη έγκυρη σύνταξη email");

        // Χωρίζουμε username και domain
        int atIndex = email.IndexOf('@');
        string username = email.Substring(0, atIndex);
        string domain = email.Substring(atIndex + 1);

        if (username.Length > 64)
            return (false, "Το όνομα χρήστη πριν το @ είναι πολύ μεγάλο");

        if (domain.Length > 255)
            return (false, "Το domain μετά το @ είναι πολύ μεγάλο");

        return (true, "Έγκυρο email");
    }



    // Τρέχουμε τη συνάρτηση

}
