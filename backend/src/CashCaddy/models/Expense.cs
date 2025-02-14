
using System.Text.Json.Serialization;

namespace CashCaddy.API.Models;
public class Expense
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }