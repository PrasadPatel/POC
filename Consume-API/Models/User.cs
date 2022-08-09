namespace Consume_API.Models
{
    /// <summary>
    /// Model for Address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets/Sets street property
        /// </summary>
        public string? street { get; set; }
        /// <summary>
        /// Gets/Sets suite property
        /// </summary>
        public string? suite { get; set; }
        /// <summary>
        /// Gets/Sets city property
        /// </summary>
        public string? city { get; set; }
        /// <summary>
        /// Gets/Sets zipcode property
        /// </summary>
        public string? zipcode { get; set; }
        /// <summary>
        /// Gets/Sets geo property
        /// </summary>
        public Geo? geo { get; set; }
    }

    /// <summary>
    /// Model for Company
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Gets/Sets name property
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// Gets/Sets catchPhrase property
        /// </summary>
        public string? catchPhrase { get; set; }
        /// <summary>
        /// Gets/Sets bs property
        /// </summary>
        public string? bs { get; set; }
    }

    /// <summary>
    /// Model for Geo
    /// </summary>
    public class Geo
    {
        /// <summary>
        /// Gets/Sets lat property
        /// </summary>
        public string? lat { get; set; }
        /// <summary>
        /// Gets/Sets lng property
        /// </summary>
        public string? lng { get; set; }
    }

    /// <summary>
    /// Model for User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets/Sets id property
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// Gets/Sets name property
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// Gets/Sets username property
        /// </summary>
        public string? username { get; set; }
        /// <summary>
        /// Gets/Sets email property
        /// </summary>
        public string? email { get; set; }
        /// <summary>
        /// Gets/Sets address property
        /// </summary>
        public Address? address { get; set; }
        /// <summary>
        /// Gets/Sets phone property
        /// </summary>
        public string? phone { get; set; }
        /// <summary>
        /// Gets/Sets website property
        /// </summary>
        public string? website { get; set; }
        /// <summary>
        /// Gets/Sets company property
        /// </summary>
        public Company? company { get; set; }
    }
}