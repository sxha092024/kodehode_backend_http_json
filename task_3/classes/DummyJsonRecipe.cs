using System.Text;

// https://dummyjson.com/docs/recipes
namespace DummyJson
{
    // Field names mirror API
    // All fields are guaranteed to be returned by the API. I don't neccessarily 
    // like exposing encapsulated data directly, however without digging much further into
    // Microsoft's annotations or compiler directives related to their JSON implementation
    // this is what I ran into in terms of data validation, requiring the required keyword
    // to hard throw an exception upon data which cannot be de-serialized.
    public class DummyJsonRecipe
    {
        
        public required uint id { get; set; }
        public required string name { get; set; }
        public required List<string> ingredients { get; set; }
        public required List<string> instructions { get; set; }
        public required uint prepTimeMinutes { get; set; }
        public required uint cookTimeMinutes { get; set; }
        public required uint servings { get; set; }
        // Should have been a value enum, however C# enums are restrictive compared to Rust enums
        // TODO: Enum (auto) parsed from string value
        public required string difficulty { get; set; }
        // TODO: Enum (auto) parsed from string value
        public required string cuisine { get; set; }
        public required uint caloriesPerServing { get; set; }
        public required List<string> tags { get; set; }
        public required uint userId { get; set; }
        public required string image { get; set; }
        public required decimal rating { get; set; }
        public required uint reviewCount { get; set; }
        // TODO: Enum (auto) parsed from string value
        public required List<string> mealType { get; set; }

        // Ideally C# would have an auto impl that flattens nested collections into a string repr, 
        // in a debug representation. Since it does not, manual flattening.
        override public string ToString()
        {
            var s = new StringBuilder();
            s.Append($"Recipe {{id: {id}, name: {name}, ingredients: [");
            foreach (var ingredient in ingredients)
            {
                s.Append($"{ingredient}, ");
            }
            s.Remove(s.Length - 2, 2);
            s.Append("], instructions: [");
            foreach (var instruction in instructions)
            {
                s.Append($"{instruction} ");
            }
            s.Remove(s.Length - 1, 1);
            s.Append($"], prepTimeMinutes: {prepTimeMinutes}, cookTimeMinutes: {cookTimeMinutes}, ");
            s.Append($"servings: {servings}, difficulty: {difficulty}, cuisine: {cuisine}, ");
            s.Append($"caloriesPerServing: {caloriesPerServing}, tags: [");
            foreach (var tag in tags)
            {
                s.Append($"{tag}, ");
            }
            s.Remove(s.Length - 2, 2);
            s.Append($"], userId: {userId}, image: {image}, rating: {rating}, reviewCount: {reviewCount}, mealType: [");
            foreach (var _type in mealType)
            {
                s.Append($"{_type}, ");
            }
            s.Remove(s.Length - 2, 2);
            s.Append("]}");
            return s.ToString();
        }
    }

    public class Results
    {
        public required List<DummyJsonRecipe> recipes { get; set; }
        public required uint total { get; set; }
        public required uint skip { get; set; }
        public required uint limit { get; set; }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Results {recipes: [");
            foreach (var recipe in recipes)
            {
                s.Append($"{recipe}, ");
            }
            // This remove could be avoided by having a manual for loop with a conditional special case for the last element
            // honestly? Laziness
            s.Remove(s.Length - 2, 2);
            s.Append($"], total: {total}, skip: {skip}, limit: {limit}}}");
            return s.ToString();
        }
    }
}