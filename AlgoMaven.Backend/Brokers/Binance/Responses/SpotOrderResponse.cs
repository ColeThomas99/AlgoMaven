using System;
using System.Text.Json.Serialization;

namespace AlgoMaven.Backend.Brokers.Binance.Responses
{
	public class SpotOrderResponse
	{
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("orderId")]
        public string OrderID { get; set; }
        [JsonPropertyName("orderListId")]
        public int OrderListID { get; set; }
        [JsonPropertyName("symbol")]
        public string ClientOrderID { get; set; }
        [JsonPropertyName("clientOrderId")]
        public long TransactionTime { get; set; }
        [JsonPropertyName("transactionTime")]
        public decimal Price { get; set; }
        [JsonPropertyName("origQty")]
        public decimal OriginalQuantity { get; set; }
        [JsonPropertyName("executedQty")]
        public decimal ExecutedQuantity { get; set; }
        [JsonPropertyName("cummulativeQuoteQty")]
        public decimal CummulativeQuoteQuantity { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("timeInForce")]
        public TimeInForce TimeInForce { get; set; }
        [JsonPropertyName("type")]
        public OrderType Type { get; set; }
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        [JsonPropertyName("workingTime")]
        public long WorkingTime { get; set; }
        [JsonPropertyName("selfTradePreventionMode")]
        public SelfTradePreventionMode SelfTradePreventionMode { get; set; }
        [JsonPropertyName("fills")]
        public List<Fill> Fills { get; set; }
    }
}

