﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        [Test]
        public void EveryType_String()
        {
            // Arrange
            EveryType obj = ArrangeEveryType();

            // Act
            string json = JsonSerializer.SerializeObject(obj);
            EveryType newEveryType = JsonSerializer.ReadObject<EveryType>(json);

            // Assert
            AssertEveryType(obj, newEveryType);
        }

        [Test]
        public void EveryType_Stream()
        {
            // Arrange
            EveryType obj = ArrangeEveryType();

            // Act
            string json = JsonSerializer.SerializeObject(obj);
            EveryType newEveryType;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                newEveryType = JsonSerializer.ReadObject<EveryType>(stream);
            }

            // Assert
            AssertEveryType(obj, newEveryType);
        }

        private static EveryType ArrangeEveryType()
        {
            // Arrange
            EveryType everyType = new EveryType
                                {
                                    String = "Hello \"World\"!",
                                    NullString = null,
                                    Boolean = true,
                                    DateTime = new DateTime(2004, 3, 2),
                                    Int16 = 2,
                                    UInt16 = UInt16.MaxValue,
                                    Int32 = -5,
                                    UInt32 = 42,
                                    Int64 = 99,
                                    UInt64 = 9234032984,
                                    Single = 0.5f,
                                    Double = 4.5d,
                                    Decimal = 9.9m,
                                    BooleanArray = new[] { true, true, false, true, false },
                                    DateTimeArray = new[] { new DateTime(2011, 6, 29), new DateTime(1776, 7, 4) },
                                    StringArray = new[] { "Hello", "\"World\"", ",testing,", null },
                                    Int16Array = new short[] { 3, 4, 5 },
                                    UInt16Array = new ushort[] { 1, 2, 9 },
                                    Int32Array = new[] { -1, -8, -7, 0, 1 },
                                    UInt32Array = new uint[] { 7, 8, 9 },
                                    Int64Array = new long[] { -1232384, 120989123, -1293813 },
                                    UInt64Array = new ulong[] { 239874, 982347, 9237489 },
                                    SingleArray = new[] { 1.1f, -2.2f, 3.3f },
                                    DoubleArray = new[] { 4.4d, -5.5d, 6.6d },
                                    DecimalArray = new[] { 0.12345678m, 123123.123123m, -129380.12313m },
                                    BooleanDictionary = new Dictionary<string,bool>(),
                                    DateTimeDictionary = new Dictionary<string,DateTime>(),
                                    StringDictionary = new Dictionary<string, string>(),
                                    Int16Dictionary = new Dictionary<string, short>(),
                                    UInt16Dictionary = new Dictionary<string, ushort>(),
                                    Int32Dictionary = new Dictionary<string, int>(),
                                    UInt32Dictionary = new Dictionary<string, uint>(),
                                    Int64Dictionary = new Dictionary<string, long>(),
                                    UInt64Dictionary = new Dictionary<string, ulong>(),
                                    SingleDictionary = new Dictionary<string, float>(),
                                    DoubleDictionary = new Dictionary<string, double>(),
                                    DecimalDictionary = new Dictionary<string, decimal>(),
                                    Enum = TestEnum.Two,
                                    EnumArray = new[] { TestEnum.One, TestEnum.Three },
                                    EnumDictionary = new Dictionary<string, TestEnum>(),
                                    Class = new TestClass { Name = "foo" },
                                    ClassArray = new[] { new TestClass { Name = "bar" }, new TestClass { Name = "2000" } },
                                    ClassDictionary = new Dictionary<string, TestClass>()
                                };

            everyType.BooleanDictionary.Add("isPassing", true);
            everyType.BooleanDictionary.Add("isFailing", false);

            everyType.DateTimeDictionary.Add("anniversary", new DateTime(2008, 11, 1));
            everyType.DateTimeDictionary.Add("herBirthday", new DateTime(1980, 2, 6));

            everyType.StringDictionary.Add("one", "hello");
            everyType.StringDictionary.Add("two", "wo\\\"rld]");

            everyType.Int16Dictionary.Add("three", -3);
            everyType.Int16Dictionary.Add("four", 4);

            everyType.UInt16Dictionary.Add("five", 5123);
            everyType.UInt16Dictionary.Add("six", 13213);

            everyType.Int32Dictionary.Add("one", -123);
            everyType.Int32Dictionary.Add("\"", 1234);

            everyType.UInt32Dictionary.Add("asdf", 4444445);
            everyType.UInt32Dictionary.Add("\\", 555);

            everyType.Int64Dictionary.Add("'", 109823424);
            everyType.Int64Dictionary.Add("\"\"", 8972234);

            everyType.UInt64Dictionary.Add("one", 982374987234);
            everyType.UInt64Dictionary.Add("two", 823982748234);

            everyType.SingleDictionary.Add("uno", 0.543f);
            everyType.SingleDictionary.Add("dos", -0.599f);

            everyType.DoubleDictionary.Add("three", 9.999d);
            everyType.DoubleDictionary.Add("four", -99.999d);

            everyType.DecimalDictionary.Add("five", 1231313.12313123m);
            everyType.DecimalDictionary.Add("six", -2908234.23847924m);

            everyType.EnumDictionary.Add("[]", TestEnum.Three);
            everyType.EnumDictionary.Add("][", TestEnum.One);
            everyType.EnumDictionary.Add("abc", TestEnum.Two);
            everyType.EnumDictionary.Add("xyz", TestEnum.One);

            everyType.ClassDictionary.Add("class1", new TestClass { Name = "name 1" });
            everyType.ClassDictionary.Add("class2", new TestClass { Name = "{}[]\"\\" });

            return everyType;
        }

        private static void AssertEveryType(EveryType oldEveryType, EveryType newEveryType)
        {
            // Assert
            Assert.IsNotNull(newEveryType);

            Assert.IsNotNull(newEveryType.StringArray);
            Assert.IsNotNull(newEveryType.Int16Array);
            Assert.IsNotNull(newEveryType.UInt16Array);
            Assert.IsNotNull(newEveryType.Int32Array);
            Assert.IsNotNull(newEveryType.UInt32Array);
            Assert.IsNotNull(newEveryType.Int64Array);
            Assert.IsNotNull(newEveryType.UInt64Array);
            Assert.IsNotNull(newEveryType.SingleArray);
            Assert.IsNotNull(newEveryType.DoubleArray);
            Assert.IsNotNull(newEveryType.DecimalArray);
            Assert.IsNotNull(newEveryType.StringDictionary);
            Assert.IsNotNull(newEveryType.Int16Dictionary);
            Assert.IsNotNull(newEveryType.UInt16Dictionary);
            Assert.IsNotNull(newEveryType.Int32Dictionary);
            Assert.IsNotNull(newEveryType.UInt32Dictionary);
            Assert.IsNotNull(newEveryType.Int64Dictionary);
            Assert.IsNotNull(newEveryType.UInt64Dictionary);
            Assert.IsNotNull(newEveryType.SingleDictionary);
            Assert.IsNotNull(newEveryType.DoubleDictionary);
            Assert.IsNotNull(newEveryType.DecimalDictionary);
            Assert.IsNotNull(newEveryType.EnumArray);
            Assert.IsNotNull(newEveryType.EnumDictionary);
            Assert.IsNotNull(newEveryType.Class);
            Assert.IsNotNull(newEveryType.ClassArray);
            Assert.IsNotNull(newEveryType.ClassDictionary);

            Assert.AreEqual(oldEveryType.String, newEveryType.String);
            Assert.IsNull(newEveryType.NullString);
            Assert.AreEqual(oldEveryType.Boolean, newEveryType.Boolean);
            Assert.AreEqual(oldEveryType.DateTime, newEveryType.DateTime);
            Assert.AreEqual(oldEveryType.Int16, newEveryType.Int16);
            Assert.AreEqual(oldEveryType.UInt16, newEveryType.UInt16);
            Assert.AreEqual(oldEveryType.Int32, newEveryType.Int32);
            Assert.AreEqual(oldEveryType.UInt32, newEveryType.UInt32);
            Assert.AreEqual(oldEveryType.Int64, newEveryType.Int64);
            Assert.AreEqual(oldEveryType.UInt64, newEveryType.UInt64);
            Assert.AreEqual(oldEveryType.Single, newEveryType.Single);
            Assert.AreEqual(oldEveryType.Double, newEveryType.Double);
            Assert.AreEqual(oldEveryType.Decimal, newEveryType.Decimal);
            Assert.AreEqual(oldEveryType.Enum, newEveryType.Enum);

            Assert.AreEqual(oldEveryType.Class.Name, newEveryType.Class.Name);

            Assert.AreEqual(oldEveryType.BooleanDictionary.Count, newEveryType.BooleanDictionary.Count);
            foreach (var kvp in oldEveryType.BooleanDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.BooleanDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.DateTimeDictionary.Count, newEveryType.DateTimeDictionary.Count);
            foreach (var kvp in oldEveryType.DateTimeDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.DateTimeDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.StringDictionary.Count, newEveryType.StringDictionary.Count);
            foreach (var kvp in oldEveryType.StringDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.StringDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.Int16Dictionary.Count, newEveryType.Int16Dictionary.Count);
            foreach (var kvp in oldEveryType.Int16Dictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.Int16Dictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.UInt16Dictionary.Count, newEveryType.UInt16Dictionary.Count);
            foreach (var kvp in oldEveryType.UInt16Dictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.UInt16Dictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.Int32Dictionary.Count, newEveryType.Int32Dictionary.Count);
            foreach (var kvp in oldEveryType.Int32Dictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.Int32Dictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.UInt32Dictionary.Count, newEveryType.UInt32Dictionary.Count);
            foreach (var kvp in oldEveryType.UInt32Dictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.UInt32Dictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.Int64Dictionary.Count, newEveryType.Int64Dictionary.Count);
            foreach (var kvp in oldEveryType.Int64Dictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.Int64Dictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.UInt64Dictionary.Count, newEveryType.UInt64Dictionary.Count);
            foreach (var kvp in oldEveryType.UInt64Dictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.UInt64Dictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.SingleDictionary.Count, newEveryType.SingleDictionary.Count);
            foreach (var kvp in oldEveryType.SingleDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.SingleDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.DoubleDictionary.Count, newEveryType.DoubleDictionary.Count);
            foreach (var kvp in oldEveryType.DoubleDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.DoubleDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.DecimalDictionary.Count, newEveryType.DecimalDictionary.Count);
            foreach (var kvp in oldEveryType.DecimalDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.DecimalDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.EnumDictionary.Count, newEveryType.EnumDictionary.Count);
            foreach (var kvp in oldEveryType.EnumDictionary)
            {
                Assert.AreEqual(kvp.Value, newEveryType.EnumDictionary[kvp.Key]);
            }

            Assert.AreEqual(oldEveryType.ClassDictionary.Count, newEveryType.ClassDictionary.Count);
            foreach (var kvp in oldEveryType.ClassDictionary)
            {
                Assert.AreEqual(kvp.Value.Name, newEveryType.ClassDictionary[kvp.Key].Name);
            }

            Assert.AreEqual(oldEveryType.BooleanArray.Length, newEveryType.BooleanArray.Length);
            for (int i = 0; i < oldEveryType.BooleanArray.Length; i++)
                Assert.AreEqual(oldEveryType.BooleanArray[i], newEveryType.BooleanArray[i]);

            Assert.AreEqual(oldEveryType.DateTimeArray.Length, newEveryType.DateTimeArray.Length);
            for (int i = 0; i < oldEveryType.DateTimeArray.Length; i++)
                Assert.AreEqual(oldEveryType.DateTimeArray[i], newEveryType.DateTimeArray[i]);

            Assert.AreEqual(oldEveryType.StringArray.Length, newEveryType.StringArray.Length);
            for (int i = 0; i < oldEveryType.StringArray.Length; i++)
                Assert.AreEqual(oldEveryType.StringArray[i], newEveryType.StringArray[i]);

            Assert.AreEqual(oldEveryType.Int16Array.Length, newEveryType.Int16Array.Length);
            for (int i = 0; i < oldEveryType.Int16Array.Length; i++)
                Assert.AreEqual(oldEveryType.Int16Array[i], newEveryType.Int16Array[i]);

            Assert.AreEqual(oldEveryType.UInt16Array.Length, newEveryType.UInt16Array.Length);
            for (int i = 0; i < oldEveryType.UInt16Array.Length; i++)
                Assert.AreEqual(oldEveryType.UInt16Array[i], newEveryType.UInt16Array[i]);

            Assert.AreEqual(oldEveryType.Int32Array.Length, newEveryType.Int32Array.Length);
            for (int i = 0; i < oldEveryType.Int32Array.Length; i++)
                Assert.AreEqual(oldEveryType.Int32Array[i], newEveryType.Int32Array[i]);

            Assert.AreEqual(oldEveryType.UInt32Array.Length, newEveryType.UInt32Array.Length);
            for (int i = 0; i < oldEveryType.UInt32Array.Length; i++)
                Assert.AreEqual(oldEveryType.UInt32Array[i], newEveryType.UInt32Array[i]);

            Assert.AreEqual(oldEveryType.Int64Array.Length, newEveryType.Int64Array.Length);
            for (int i = 0; i < oldEveryType.Int64Array.Length; i++)
                Assert.AreEqual(oldEveryType.Int64Array[i], newEveryType.Int64Array[i]);

            Assert.AreEqual(oldEveryType.UInt64Array.Length, newEveryType.UInt64Array.Length);
            for (int i = 0; i < oldEveryType.UInt64Array.Length; i++)
                Assert.AreEqual(oldEveryType.UInt64Array[i], newEveryType.UInt64Array[i]);

            Assert.AreEqual(oldEveryType.SingleArray.Length, newEveryType.SingleArray.Length);
            for (int i = 0; i < oldEveryType.SingleArray.Length; i++)
                Assert.AreEqual(oldEveryType.SingleArray[i], newEveryType.SingleArray[i]);

            Assert.AreEqual(oldEveryType.DoubleArray.Length, newEveryType.DoubleArray.Length);
            for (int i = 0; i < oldEveryType.DoubleArray.Length; i++)
                Assert.AreEqual(oldEveryType.DoubleArray[i], newEveryType.DoubleArray[i]);

            Assert.AreEqual(oldEveryType.DecimalArray.Length, newEveryType.DecimalArray.Length);
            for (int i = 0; i < oldEveryType.DecimalArray.Length; i++)
                Assert.AreEqual(oldEveryType.DecimalArray[i], newEveryType.DecimalArray[i]);

            Assert.AreEqual(oldEveryType.EnumArray.Length, newEveryType.EnumArray.Length);
            for (int i = 0; i < oldEveryType.EnumArray.Length; i++)
                Assert.AreEqual(oldEveryType.EnumArray[i], newEveryType.EnumArray[i]);

            Assert.AreEqual(oldEveryType.ClassArray.Length, newEveryType.ClassArray.Length);
            for (int i = 0; i < oldEveryType.ClassArray.Length; i++)
                Assert.AreEqual(oldEveryType.ClassArray[i].Name, newEveryType.ClassArray[i].Name);
        }
    }
}
