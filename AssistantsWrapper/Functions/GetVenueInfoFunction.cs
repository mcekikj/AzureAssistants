using System.Text.Json;
using Azure.AI.OpenAI;

namespace AssistantsWrapper.Functions
{
    public class GetVenueInfoFunction
    {
        static public string Name = "get_venue_info";

        // Return the function metadata
        static public FunctionDefinition GetFunctionDefinition()
        {
            return new FunctionDefinition()
            {
                Name = Name,
                Description = "Get the venue information and details",
                Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new
                    {
                        Name = new
                        {
                            Type = "string",
                            Description = "The name of the venue, ex. Base42",
                        }
                    },
                    Required = new[] { "name" },
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };
        }

        // The function implementation. It always return predefined output for now.
        static public string GetVenue(string location)
        {
            switch (location)
            {
                case "Base42":
                    return "Base42 in located on Rimska 25 in Skopje and provides a collaborative environment for " +
                        "freelancers, startups, and small businesses. The space typically offers amenities like high-speed internet, " +
                        "meeting rooms, and event spaces, fostering a community of innovation and networking. " +
                        "Additionally, Base42 often hosts workshops, events, and networking opportunities, making it " +
                        "a hub for entrepreneurs and creatives in the area. ";
                case "MCC":
                    return "MKC or MCC stands for Macedonian Cultural Center and representes a vibrant hub for arts and culture, " +
                        "dedicated to promoting and preserving Macedonian heritage. It hosts a variety of events, including concerts, " +
                        "theater performances, and art exhibitions, providing a platform for local and international artists. " +
                        "With facilities for workshops and educational programs, MKC engages the community and fosters creativity. " +
                        "Centrally located, it serves as a vital resource for cultural dialogue and artistic expression in Skopje.";
                default:
                    return string.Empty;
            }
        }

        // Argument for the function
        public class VenueInput
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}