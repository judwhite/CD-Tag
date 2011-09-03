using CDTag.Common.Json;
using CDTag.Common.Tests.Json.Model;
using NUnit.Framework;

namespace CDTag.Common.Tests.Json
{
    [TestFixture]
    public class JsonSerializerTest
    {
        [Test]
        public void DeserializeColorValue_SimpleClass()
        {
            // Act
            ColorValue colorValue = JsonSerializer.ReadObject<ColorValue>(Properties.Resources.colorvalue_json);

            // Assert
            AssertColorValue(colorValue);
        }

        [Test]
        public void SerializeColorValue_SimpleClass()
        {
            // Arrange
            ColorValue colorValue = JsonSerializer.ReadObject<ColorValue>(Properties.Resources.colorvalue_json);

            // Act
            string json = JsonSerializer.SerializeObject(colorValue);
            ColorValue newColorValue = JsonSerializer.ReadObject<ColorValue>(json);

            // Assert
            AssertColorValue(newColorValue);
        }

        private static void AssertColorValue(ColorValue colorValue)
        {
            Assert.IsNotNull(colorValue);
            Assert.AreEqual("red", colorValue.Color);
            Assert.AreEqual("#f00", colorValue.Value);
        }

        [Test]
        public void DeserializeRecipe_NestedClassesAndArrays()
        {
            // Act
            Recipe recipe = JsonSerializer.ReadObject<Recipe>(Properties.Resources.recipe_json);

            // Assert
            AssertRecipe(recipe);
        }

        [Test]
        public void SerializeRecipe_NestedClassesAndArrays()
        {
            // Arrange
            Recipe recipe = JsonSerializer.ReadObject<Recipe>(Properties.Resources.recipe_json);

            // Act
            string json = JsonSerializer.SerializeObject(recipe);
            Recipe newRecipe = JsonSerializer.ReadObject<Recipe>(json);

            // Assert
            AssertRecipe(newRecipe);
        }

        private static void AssertRecipe(Recipe recipe)
        {
            Assert.IsNotNull(recipe);
            Assert.IsNotNull(recipe.Batters);
            Assert.IsNotNull(recipe.Batters.Batter);
            Assert.IsNotNull(recipe.Topping);

            Assert.IsNull(recipe.Fillings);
            Assert.IsNull(recipe.Image);
            Assert.IsNull(recipe.Thumbnail);

            Assert.AreEqual("0001", recipe.ID);
            Assert.AreEqual("donut", recipe.Type);
            Assert.AreEqual("Cake", recipe.Name);
            Assert.AreEqual(0.55, recipe.PPU);

            Assert.AreEqual(4, recipe.Batters.Batter.Length);
            Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
            Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);
            Assert.AreEqual("1002", recipe.Batters.Batter[1].ID);
            Assert.AreEqual("Chocolate", recipe.Batters.Batter[1].Type);
            Assert.AreEqual("1003", recipe.Batters.Batter[2].ID);
            Assert.AreEqual("Blueberry", recipe.Batters.Batter[2].Type);
            Assert.AreEqual("1004", recipe.Batters.Batter[3].ID);
            Assert.AreEqual("Devil's Food", recipe.Batters.Batter[3].Type);

            Assert.AreEqual(7, recipe.Topping.Length);
            Assert.AreEqual("5001", recipe.Topping[0].ID);
            Assert.AreEqual("None", recipe.Topping[0].Type);
            Assert.AreEqual("5002", recipe.Topping[1].ID);
            Assert.AreEqual("Glazed", recipe.Topping[1].Type);
            Assert.AreEqual("5005", recipe.Topping[2].ID);
            Assert.AreEqual("Sugar", recipe.Topping[2].Type);
            Assert.AreEqual("5007", recipe.Topping[3].ID);
            Assert.AreEqual("Powdered Sugar", recipe.Topping[3].Type);
            Assert.AreEqual("5006", recipe.Topping[4].ID);
            Assert.AreEqual("Chocolate with Sprinkles", recipe.Topping[4].Type);
            Assert.AreEqual("5003", recipe.Topping[5].ID);
            Assert.AreEqual("Chocolate", recipe.Topping[5].Type);
            Assert.AreEqual("5004", recipe.Topping[6].ID);
            Assert.AreEqual("Maple", recipe.Topping[6].Type);
        }

        [Test]
        public void DeserializeRecipeArray_ArrayOfClasses()
        {
            // Act
            Recipe[] recipes = JsonSerializer.ReadObject<Recipe[]>(Properties.Resources.recipe_array_json);

            // Assert
            AssertRecipeArray(recipes);
        }

        [Test]
        public void SerializeRecipeArray_ArrayOfClasses()
        {
            // Arrange
            Recipe[] recipes = JsonSerializer.ReadObject<Recipe[]>(Properties.Resources.recipe_array_json);

            // Act
            string json = JsonSerializer.SerializeObject(recipes);
            Recipe[] newRecipes = JsonSerializer.ReadObject<Recipe[]>(json);

            // Assert
            AssertRecipeArray(newRecipes);
        }

        private static void AssertRecipeArray(Recipe[] recipes)
        {
            // Assert
            Assert.IsNotNull(recipes);
            Assert.AreEqual(3, recipes.Length);

            // Assert recipes[0]
            {
                Recipe recipe = recipes[0];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0001", recipe.ID);
                Assert.AreEqual("donut", recipe.Type);
                Assert.AreEqual("Cake", recipe.Name);
                Assert.AreEqual(0.55, recipe.PPU);

                Assert.AreEqual(4, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);
                Assert.AreEqual("1002", recipe.Batters.Batter[1].ID);
                Assert.AreEqual("Chocolate", recipe.Batters.Batter[1].Type);
                Assert.AreEqual("1003", recipe.Batters.Batter[2].ID);
                Assert.AreEqual("Blueberry", recipe.Batters.Batter[2].Type);
                Assert.AreEqual("1004", recipe.Batters.Batter[3].ID);
                Assert.AreEqual("Devil's Food", recipe.Batters.Batter[3].Type);

                Assert.AreEqual(7, recipe.Topping.Length);
                Assert.AreEqual("5001", recipe.Topping[0].ID);
                Assert.AreEqual("None", recipe.Topping[0].Type);
                Assert.AreEqual("5002", recipe.Topping[1].ID);
                Assert.AreEqual("Glazed", recipe.Topping[1].Type);
                Assert.AreEqual("5005", recipe.Topping[2].ID);
                Assert.AreEqual("Sugar", recipe.Topping[2].Type);
                Assert.AreEqual("5007", recipe.Topping[3].ID);
                Assert.AreEqual("Powdered Sugar", recipe.Topping[3].Type);
                Assert.AreEqual("5006", recipe.Topping[4].ID);
                Assert.AreEqual("Chocolate with Sprinkles", recipe.Topping[4].Type);
                Assert.AreEqual("5003", recipe.Topping[5].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[5].Type);
                Assert.AreEqual("5004", recipe.Topping[6].ID);
                Assert.AreEqual("Maple", recipe.Topping[6].Type);
            }

            // Assert recipes[1]
            {
                Recipe recipe = recipes[1];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0002", recipe.ID);
                Assert.AreEqual("donut", recipe.Type);
                Assert.AreEqual("Raised", recipe.Name);
                Assert.AreEqual(0.55, recipe.PPU);

                Assert.AreEqual(1, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);

                Assert.AreEqual(5, recipe.Topping.Length);
                Assert.AreEqual("5001", recipe.Topping[0].ID);
                Assert.AreEqual("None", recipe.Topping[0].Type);
                Assert.AreEqual("5002", recipe.Topping[1].ID);
                Assert.AreEqual("Glazed", recipe.Topping[1].Type);
                Assert.AreEqual("5005", recipe.Topping[2].ID);
                Assert.AreEqual("Sugar", recipe.Topping[2].Type);
                Assert.AreEqual("5003", recipe.Topping[3].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[3].Type);
                Assert.AreEqual("5004", recipe.Topping[4].ID);
                Assert.AreEqual("Maple", recipe.Topping[4].Type);
            }

            // Assert recipes[2]
            {
                Recipe recipe = recipes[2];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0003", recipe.ID);
                Assert.AreEqual("donut", recipe.Type);
                Assert.AreEqual("Old Fashioned", recipe.Name);
                Assert.AreEqual(0.55, recipe.PPU);

                Assert.AreEqual(2, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);
                Assert.AreEqual("1002", recipe.Batters.Batter[1].ID);
                Assert.AreEqual("Chocolate", recipe.Batters.Batter[1].Type);

                Assert.AreEqual(4, recipe.Topping.Length);
                Assert.AreEqual("5001", recipe.Topping[0].ID);
                Assert.AreEqual("None", recipe.Topping[0].Type);
                Assert.AreEqual("5002", recipe.Topping[1].ID);
                Assert.AreEqual("Glazed", recipe.Topping[1].Type);
                Assert.AreEqual("5003", recipe.Topping[2].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[2].Type);
                Assert.AreEqual("5004", recipe.Topping[3].ID);
                Assert.AreEqual("Maple", recipe.Topping[3].Type);
            }
        }

        [Test]
        public void DeserializeRecipe_NestedClasses()
        {
            // Act
            Recipe recipe = JsonSerializer.ReadObject<Recipe>(Properties.Resources.image_thumbnail_json);

            // Assert
            AssertRecipeWithImages(recipe);
        }

        [Test]
        public void SerializeRecipe_NestedClasses()
        {
            // Arrange
            Recipe recipe = JsonSerializer.ReadObject<Recipe>(Properties.Resources.image_thumbnail_json);

            // Act
            string json = JsonSerializer.SerializeObject(recipe);
            Recipe newRecipe = JsonSerializer.ReadObject<Recipe>(json);

            // Assert
            AssertRecipeWithImages(newRecipe);
        }

        private static void AssertRecipeWithImages(Recipe recipe)
        {
            // Assert
            Assert.IsNotNull(recipe);
            Assert.IsNotNull(recipe.Image);
            Assert.IsNotNull(recipe.Thumbnail);

            Assert.AreEqual("0001", recipe.ID);
            Assert.AreEqual("donut", recipe.Type);
            Assert.AreEqual("Cake", recipe.Name);

            Assert.AreEqual("images/0001.jpg", recipe.Image.Url);
            Assert.AreEqual(200, recipe.Image.Width);
            Assert.AreEqual(200, recipe.Image.Height);

            Assert.AreEqual("images/thumbnails/0001.jpg", recipe.Thumbnail.Url);
            Assert.AreEqual(32, recipe.Thumbnail.Width);
            Assert.AreEqual(32, recipe.Thumbnail.Height);
        }

        [Test]
        public void DeserializeDonuts_LargeNested()
        {
            // Act
            RecipeBook book = JsonSerializer.ReadObject<RecipeBook>(Properties.Resources.donuts_json);

            // Assert
            AssertDonuts(book);
        }

        [Test]
        public void SerializeDonuts_LargeNested()
        {
            // Arrange
            RecipeBook book = JsonSerializer.ReadObject<RecipeBook>(Properties.Resources.donuts_json);

            // Act
            string json = JsonSerializer.SerializeObject(book);
            RecipeBook newBook = JsonSerializer.ReadObject<RecipeBook>(json);

            // Assert
            AssertDonuts(newBook);
        }

        private static void AssertDonuts(RecipeBook book)
        {
            // Assert
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.Items);
            Assert.IsNotNull(book.Items.Item);

            Assert.AreEqual(6, book.Items.Item.Length, "book.Items.Item.Length");

            // Assert book.Items.Item[0]
            {
                Recipe recipe = book.Items.Item[0];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0001", recipe.ID);
                Assert.AreEqual("donut", recipe.Type);
                Assert.AreEqual("Cake", recipe.Name);
                Assert.AreEqual(0.55, recipe.PPU);

                Assert.AreEqual(4, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);
                Assert.AreEqual("1002", recipe.Batters.Batter[1].ID);
                Assert.AreEqual("Chocolate", recipe.Batters.Batter[1].Type);
                Assert.AreEqual("1003", recipe.Batters.Batter[2].ID);
                Assert.AreEqual("Blueberry", recipe.Batters.Batter[2].Type);
                Assert.AreEqual("1004", recipe.Batters.Batter[3].ID);
                Assert.AreEqual("Devil's Food", recipe.Batters.Batter[3].Type);

                Assert.AreEqual(7, recipe.Topping.Length);
                Assert.AreEqual("5001", recipe.Topping[0].ID);
                Assert.AreEqual("None", recipe.Topping[0].Type);
                Assert.AreEqual("5002", recipe.Topping[1].ID);
                Assert.AreEqual("Glazed", recipe.Topping[1].Type);
                Assert.AreEqual("5005", recipe.Topping[2].ID);
                Assert.AreEqual("Sugar", recipe.Topping[2].Type);
                Assert.AreEqual("5007", recipe.Topping[3].ID);
                Assert.AreEqual("Powdered Sugar", recipe.Topping[3].Type);
                Assert.AreEqual("5006", recipe.Topping[4].ID);
                Assert.AreEqual("Chocolate with Sprinkles", recipe.Topping[4].Type);
                Assert.AreEqual("5003", recipe.Topping[5].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[5].Type);
                Assert.AreEqual("5004", recipe.Topping[6].ID);
                Assert.AreEqual("Maple", recipe.Topping[6].Type);
            }

            // Assert book.Items.Item[1]
            {
                Recipe recipe = book.Items.Item[1];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0002", recipe.ID);
                Assert.AreEqual("donut", recipe.Type);
                Assert.AreEqual("Raised", recipe.Name);
                Assert.AreEqual(0.55, recipe.PPU);

                Assert.AreEqual(1, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);

                Assert.AreEqual(5, recipe.Topping.Length);
                Assert.AreEqual("5001", recipe.Topping[0].ID);
                Assert.AreEqual("None", recipe.Topping[0].Type);
                Assert.AreEqual("5002", recipe.Topping[1].ID);
                Assert.AreEqual("Glazed", recipe.Topping[1].Type);
                Assert.AreEqual("5005", recipe.Topping[2].ID);
                Assert.AreEqual("Sugar", recipe.Topping[2].Type);
                Assert.AreEqual("5003", recipe.Topping[3].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[3].Type);
                Assert.AreEqual("5004", recipe.Topping[4].ID);
                Assert.AreEqual("Maple", recipe.Topping[4].Type);
            }

            // Assert book.Items.Item[2]
            {
                Recipe recipe = book.Items.Item[2];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0003", recipe.ID);
                Assert.AreEqual("donut", recipe.Type);
                Assert.AreEqual("Old Fashioned", recipe.Name);
                Assert.AreEqual(0.55, recipe.PPU);

                Assert.AreEqual(2, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);
                Assert.AreEqual("1002", recipe.Batters.Batter[1].ID);
                Assert.AreEqual("Chocolate", recipe.Batters.Batter[1].Type);

                Assert.AreEqual(4, recipe.Topping.Length);
                Assert.AreEqual("5001", recipe.Topping[0].ID);
                Assert.AreEqual("None", recipe.Topping[0].Type);
                Assert.AreEqual("5002", recipe.Topping[1].ID);
                Assert.AreEqual("Glazed", recipe.Topping[1].Type);
                Assert.AreEqual("5003", recipe.Topping[2].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[2].Type);
                Assert.AreEqual("5004", recipe.Topping[3].ID);
                Assert.AreEqual("Maple", recipe.Topping[3].Type);
            }

            // Assert book.Items.Item[3]
            {
                Recipe recipe = book.Items.Item[3];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Fillings);
                Assert.IsNotNull(recipe.Fillings.Filling);

                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0004", recipe.ID);
                Assert.AreEqual("bar", recipe.Type);
                Assert.AreEqual("Bar", recipe.Name);
                Assert.AreEqual(0.75, recipe.PPU);

                Assert.AreEqual(1, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);

                Assert.AreEqual(2, recipe.Topping.Length);
                Assert.AreEqual("5003", recipe.Topping[0].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[0].Type);
                Assert.AreEqual("5004", recipe.Topping[1].ID);
                Assert.AreEqual("Maple", recipe.Topping[1].Type);

                Assert.AreEqual(3, recipe.Fillings.Filling.Length);
                Assert.AreEqual("7001", recipe.Fillings.Filling[0].ID);
                Assert.AreEqual("None", recipe.Fillings.Filling[0].Name);
                Assert.AreEqual(0, recipe.Fillings.Filling[0].AddCost);
                Assert.AreEqual("7002", recipe.Fillings.Filling[1].ID);
                Assert.AreEqual("Custard", recipe.Fillings.Filling[1].Name);
                Assert.AreEqual(0.25, recipe.Fillings.Filling[1].AddCost);
                Assert.AreEqual("7003", recipe.Fillings.Filling[2].ID);
                Assert.AreEqual("Whipped Cream", recipe.Fillings.Filling[2].Name);
                Assert.AreEqual(0.25, recipe.Fillings.Filling[2].AddCost);
            }

            // Assert book.Items.Item[4]
            {
                Recipe recipe = book.Items.Item[4];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);

                Assert.IsNull(recipe.Fillings);
                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0005", recipe.ID);
                Assert.AreEqual("twist", recipe.Type);
                Assert.AreEqual("Twist", recipe.Name);
                Assert.AreEqual(0.65, recipe.PPU);

                Assert.AreEqual(1, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);

                Assert.AreEqual(2, recipe.Topping.Length);
                Assert.AreEqual("5002", recipe.Topping[0].ID);
                Assert.AreEqual("Glazed", recipe.Topping[0].Type);
                Assert.AreEqual("5005", recipe.Topping[1].ID);
                Assert.AreEqual("Sugar", recipe.Topping[1].Type);
            }

            // Assert book.Items.Item[5]
            {
                Recipe recipe = book.Items.Item[5];

                Assert.IsNotNull(recipe);
                Assert.IsNotNull(recipe.Batters);
                Assert.IsNotNull(recipe.Batters.Batter);
                Assert.IsNotNull(recipe.Topping);
                Assert.IsNotNull(recipe.Fillings);
                Assert.IsNotNull(recipe.Fillings.Filling);

                Assert.IsNull(recipe.Image);
                Assert.IsNull(recipe.Thumbnail);

                Assert.AreEqual("0006", recipe.ID);
                Assert.AreEqual("filled", recipe.Type);
                Assert.AreEqual("Filled", recipe.Name);
                Assert.AreEqual(0.75, recipe.PPU);

                Assert.AreEqual(1, recipe.Batters.Batter.Length);
                Assert.AreEqual("1001", recipe.Batters.Batter[0].ID);
                Assert.AreEqual("Regular", recipe.Batters.Batter[0].Type);

                Assert.AreEqual(4, recipe.Topping.Length);
                Assert.AreEqual("5002", recipe.Topping[0].ID);
                Assert.AreEqual("Glazed", recipe.Topping[0].Type);
                Assert.AreEqual("5007", recipe.Topping[1].ID);
                Assert.AreEqual("Powdered Sugar", recipe.Topping[1].Type);
                Assert.AreEqual("5003", recipe.Topping[2].ID);
                Assert.AreEqual("Chocolate", recipe.Topping[2].Type);
                Assert.AreEqual("5004", recipe.Topping[3].ID);
                Assert.AreEqual("Maple", recipe.Topping[3].Type);

                Assert.AreEqual(4, recipe.Fillings.Filling.Length);
                Assert.AreEqual("7002", recipe.Fillings.Filling[0].ID);
                Assert.AreEqual("Custard", recipe.Fillings.Filling[0].Name);
                Assert.AreEqual(0, recipe.Fillings.Filling[0].AddCost);
                Assert.AreEqual("7003", recipe.Fillings.Filling[1].ID);
                Assert.AreEqual("Whipped Cream", recipe.Fillings.Filling[1].Name);
                Assert.AreEqual(0, recipe.Fillings.Filling[1].AddCost);
                Assert.AreEqual("7004", recipe.Fillings.Filling[2].ID);
                Assert.AreEqual("Strawberry Jelly", recipe.Fillings.Filling[2].Name);
                Assert.AreEqual(0, recipe.Fillings.Filling[2].AddCost);
                Assert.AreEqual("7005", recipe.Fillings.Filling[3].ID);
                Assert.AreEqual("Rasberry Jelly", recipe.Fillings.Filling[3].Name);
                Assert.AreEqual(0, recipe.Fillings.Filling[3].AddCost);
            }
        }

        [Test]
        public void DeserializeIntArray()
        {
            // Act
            int[] intArray = JsonSerializer.ReadObject<int[]>(Properties.Resources.int_array_json);

            // Assert
            AssertIntArray(intArray);
        }

        [Test]
        public void SerializeIntArray()
        {
            // Arrange
            int[] intArray = JsonSerializer.ReadObject<int[]>(Properties.Resources.int_array_json);

            // Act
            string json = JsonSerializer.SerializeObject(intArray);
            int[] newIntArray = JsonSerializer.ReadObject<int[]>(json);

            // Assert
            AssertIntArray(newIntArray);
        }

        private static void AssertIntArray(int[] intArray)
        {
            Assert.IsNotNull(intArray);

            Assert.AreEqual(5, intArray.Length);

            Assert.AreEqual(100, intArray[0]);
            Assert.AreEqual(500, intArray[1]);
            Assert.AreEqual(300, intArray[2]);
            Assert.AreEqual(200, intArray[3]);
            Assert.AreEqual(400, intArray[4]);
        }

        [Test]
        public void DeserializeClassArray()
        {
            // Act
            ColorValue[] colors = JsonSerializer.ReadObject<ColorValue[]>(Properties.Resources.class_array_json);

            // Assert
            AssertClassArray(colors);
        }

        [Test]
        public void SerializeClassArray()
        {
            // Arrange
            ColorValue[] colors = JsonSerializer.ReadObject<ColorValue[]>(Properties.Resources.class_array_json);

            // Act
            string json = JsonSerializer.SerializeObject(colors);
            ColorValue[] newColors = JsonSerializer.ReadObject<ColorValue[]>(json);

            // Assert
            AssertClassArray(newColors);
        }

        private static void AssertClassArray(ColorValue[] colors)
        {
            Assert.IsNotNull(colors);

            Assert.AreEqual(7, colors.Length);

            Assert.AreEqual("red", colors[0].Color);
            Assert.AreEqual("#f00", colors[0].Value);
            Assert.AreEqual("green", colors[1].Color);
            Assert.AreEqual("#0f0", colors[1].Value);
            Assert.AreEqual("blue", colors[2].Color);
            Assert.AreEqual("#00f", colors[2].Value);
            Assert.AreEqual("cyan", colors[3].Color);
            Assert.AreEqual("#0ff", colors[3].Value);
            Assert.AreEqual("magenta", colors[4].Color);
            Assert.AreEqual("#f0f", colors[4].Value);
            Assert.AreEqual("yellow", colors[5].Color);
            Assert.AreEqual("#ff0", colors[5].Value);
            Assert.AreEqual("black", colors[6].Color);
            Assert.AreEqual("#000", colors[6].Value);
        }

        [Test]
        public void DeserializePerson()
        {
            // Act
            Person person = JsonSerializer.ReadObject<Person>(Properties.Resources.person_json);

            // Assert
            AssertPerson(person);
        }

        [Test]
        public void SerializePerson()
        {
            // Arrange
            Person person = new Person();
            person.FirstName = "John";
            person.LastName = "Smith";
            person.Age = 25;
            person.Address = new Address { StreetAddress = "21 2nd Street", City = "New York", State = "NY", PostalCode = "10021" };
            person.PhoneNumber = new[] {
                new PhoneNumber { Type = "home", Number = "212 555-1234" },
                new PhoneNumber { Type = "fax", Number = "646 555-4567" },
            };

            // Act
            string json = JsonSerializer.SerializeObject(person);
            Person deserializedPerson = JsonSerializer.ReadObject<Person>(json);

            // Assert
            AssertPerson(person);
            AssertPerson(deserializedPerson);
        }

        private static void AssertPerson(Person person)
        {
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.PhoneNumber);
            Assert.IsNotNull(person.Address);

            Assert.AreEqual("John", person.FirstName);
            Assert.AreEqual("Smith", person.LastName);
            Assert.AreEqual(25, person.Age);

            Assert.AreEqual("21 2nd Street", person.Address.StreetAddress);
            Assert.AreEqual("New York", person.Address.City);
            Assert.AreEqual("NY", person.Address.State);
            Assert.AreEqual("10021", person.Address.PostalCode);

            Assert.AreEqual(2, person.PhoneNumber.Length);

            Assert.AreEqual("home", person.PhoneNumber[0].Type);
            Assert.AreEqual("212 555-1234", person.PhoneNumber[0].Number);

            Assert.AreEqual("fax", person.PhoneNumber[1].Type);
            Assert.AreEqual("646 555-4567", person.PhoneNumber[1].Number);
        }
    }
}
